using System;
using System.Collections.Generic;
using System.Text;
using Core.Features.LevelsFeature.Models;
using Core.Features.TilesFeature.Models;

namespace Core
{
    public static class MinStepCalculatorUtils
    {
        //     var start = new List<int[]>
        //     {
        //         new [] { 0, 1 },
        //         new [] { 1, 2, 3 },
        //         new [] { 3, 1 },
        //         new [] { 3 }
        //     };
        public static (int steps, List<int> path) CalculateMinSteps(LevelConfigModel level)
        {
            var setup = new List<int[]>();

            var list = new List<int>();
            foreach (var tile in level.Tiles)
            {
                list.Clear();
                AddColor(tile.Tile, list);
                setup.Add(list.ToArray());
            } 
            return MinSteps(setup);
        }

        private static void AddColor(ITileModel tile, List<int> list)
        {
            switch (tile)
            {
                case SimpleTileModel simpleTile:
                    list.Add(simpleTile.ColorId);
                    break;
                case ComplexTileModel complexTile:
                    list.Add(complexTile.ColorId);
                    AddColor(complexTile.SubTile, list);
                    break;
            }
        }

        private static (int steps, List<int> path) MinSteps(List<int[]> start)
        {
            var queue = new Queue<State>();
            var seen = new HashSet<string>(1024);

            var startKey = Encode(start);
            queue.Enqueue(new State(start, 0, Array.Empty<int>()));
            seen.Add(startKey);

            var newPathBuffer = new int[64]; 

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.Data.Count == 0)
                    return (current.Depth, new List<int>(current.Path));

                for (int choice = 0; choice <= 3; choice++)
                {
                    var newState = ApplyChoice(current.Data, choice);
                    if (Encode(newState) == Encode(current.Data))
                        continue; // нет изменений — можно пропустить

                    var key = Encode(newState);
                    if (!seen.Add(key))
                        continue;

                    int len = current.Path.Length;
                    if (len >= newPathBuffer.Length)
                        Array.Resize(ref newPathBuffer, len * 2);
                    Array.Copy(current.Path, newPathBuffer, len);
                    newPathBuffer[len] = choice;

                    var pathCopy = new int[len + 1];
                    Array.Copy(newPathBuffer, pathCopy, len + 1);

                    queue.Enqueue(new State(newState, current.Depth + 1, pathCopy));
                }
            }

            return (-1, new List<int>());
        }

        static List<int[]> ApplyChoice(List<int[]> state, int choice)
        {
            var result = new List<int[]>(state.Count);

            for (int i = 0; i < state.Count; i++)
            {
                var seq = state[i];
                if (seq.Length == 0)
                    continue;

                if (seq[0] == choice)
                {
                    if (seq.Length > 1)
                    {
                        var newSeq = new int[seq.Length - 1];
                        Array.Copy(seq, 1, newSeq, 0, newSeq.Length);
                        result.Add(newSeq);
                    }
                }
                else
                {
                    result.Add(seq); // можно переиспользовать ссылку
                }
            }

            // сортировка для стабильного ключа (лексикографически)
            result.Sort(CompareSequences);
            return result;
        }

        static int CompareSequences(int[] a, int[] b)
        {
            int min = Math.Min(a.Length, b.Length);
            for (int i = 0; i < min; i++)
            {
                int cmp = a[i].CompareTo(b[i]);
                if (cmp != 0) return cmp;
            }

            return a.Length.CompareTo(b.Length);
        }

        public static string Encode(List<int[]> state)
        {
            var sb = new StringBuilder(state.Count * 4);
            for (int i = 0; i < state.Count; i++)
            {
                var seq = state[i];
                for (int j = 0; j < seq.Length; j++)
                {
                    sb.Append(seq[j]);
                    if (j < seq.Length - 1) sb.Append(',');
                }

                if (i < state.Count - 1) sb.Append('|');
            }

            return sb.ToString();
        }
        
        public static string Encode(List<int> state)
        {
            var sb = new StringBuilder(state.Count * 4);
            for (int i = 0; i < state.Count; i++)
            {
                sb.Append(state[i]);
                if (i < state.Count - 1) sb.Append(',');
            }

            return sb.ToString();
        }

        private struct State
        {
            public readonly List<int[]> Data;
            public readonly int Depth;
            public readonly int[] Path;

            public State(List<int[]> s, int d, int[] p)
            {
                Data = s;
                Depth = d;
                Path = p;
            }
        }
    }
}