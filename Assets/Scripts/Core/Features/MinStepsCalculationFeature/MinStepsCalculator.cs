using System;
using System.Collections.Generic;

namespace Core.Features.MinStepsCalculationFeature
{
    public class Attempt
    {
        public State State;
        public int Depth;
        public readonly List<int> Path = new();
    }

    public class State
    {
        public readonly List<List<int>> Data = new();

        public override int GetHashCode()
        {
            //order of inner lists matters
            //order of lists int Data not matters
            unchecked
            {
                var hashes = new List<int>();

                foreach (var inner in Data)
                {
                    int innerHash = 17;
                    foreach (var value in inner)
                        innerHash = innerHash * 31 + value.GetHashCode();

                    hashes.Add(innerHash);
                }

                hashes.Sort();

                int hash = 19;
                foreach (var h in hashes)
                    hash = hash * 31 + h;

                return hash;
            }
        }
    }

    public class MinStepsCalculator
    {
        private readonly Queue<Attempt> _queue = new();
        private readonly HashSet<int> _seen = new(1024);
        
        private readonly Queue<State> _statePool = new();
        private readonly Queue<Attempt> _attemptPool = new();
        private readonly Queue<List<int>> _stateDataPool = new();
        
        public void InitializePools(int initialCapacity)
        {
            for (int i = 0; i < initialCapacity; i++)
            {
                _statePool.Enqueue(new State());
                _attemptPool.Enqueue(new Attempt());
                _stateDataPool.Enqueue(new List<int>());
            }
        }

        public (int steps, int[] path) MinSteps(State state)
        {
            ClearAttemptQueue();
            _seen.Clear();
            
            var startKey = state.GetHashCode();
            var attempt = GetPooledAttempt();
            attempt.State = state;
            attempt.Depth = 0;
            
            _queue.Enqueue(attempt);
            _seen.Add(startKey);

            while (_queue.Count > 0)
            {
                var current = _queue.Dequeue();

                if (current.State.Data.Count == 0)
                    return (current.Depth, current.Path.ToArray());

                for (int choice = 0; choice <= 3; choice++)
                {
                    var newState = ApplyChoice(current.State, choice);
                    if (current.GetHashCode().Equals(newState.GetHashCode()))
                        continue;

                    var key = newState.GetHashCode();
                    if (!_seen.Add(key))
                        continue;

                    var newAttempt = GetPooledAttempt();
                    newAttempt.State = newState;
                    newAttempt.Depth = current.Depth + 1;
                    newAttempt.Path.AddRange(current.Path);
                    newAttempt.Path.Add(choice);
                    _queue.Enqueue(newAttempt);
                }
                ReturnPooledAttempt(current);
            }

            return (-1, Array.Empty<int>());
        }

        private void ClearAttemptQueue()
        {
            while (_queue.TryDequeue(out var attempt))
            {
                ReturnPooledAttempt(attempt); 
            }

            _queue.Clear();
        }

        private State ApplyChoice(State state, int choice)
        {
            var result = GetPooledState();

            for (int i = 0; i < state.Data.Count; i++)
            {
                var seq = state.Data[i];
                if (seq.Count == 0)
                    continue;

                var tile = GetPooledStateData();
                if (seq[0] == choice)
                {
                    if (seq.Count <= 1) continue;
                    for (int j = 1; j < seq.Count; j++)
                    {
                        tile.Add(seq[j]);
                    }
                }
                else
                {
                    for (int j = 0; j < seq.Count; j++)
                    {
                        tile.Add(seq[j]);
                    }
                }

                result.Data.Add(tile);
            }

            return result;
        }
        
        private State GetPooledState()
        {
            return _statePool.Count > 0 ? _statePool.Dequeue() : new State();
        }

        private void ReturnPooledState(State state)
        {
            foreach (var tile in state.Data)
            {
                tile.Clear();
                _stateDataPool.Enqueue(tile);
            }
            state.Data.Clear();
            _statePool.Enqueue(state);
        }

        private Attempt GetPooledAttempt()
        {
            return _attemptPool.Count > 0 ? _attemptPool.Dequeue() : new Attempt();
        }

        private void ReturnPooledAttempt(Attempt attempt)
        {
            if(attempt.State != null)
                ReturnPooledState(attempt.State);
            attempt.State = null;
            attempt.Depth = 0;
            attempt.Path.Clear();
            _attemptPool.Enqueue(attempt);
        }
        
        private List<int> GetPooledStateData()
        {
            return _stateDataPool.Count > 0 ? _stateDataPool.Dequeue() : new List<int>();
        }
    }
}