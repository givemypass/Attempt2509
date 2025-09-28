using System;
using Core.CommonComponents;
using Core.Features.GameScreenFeature.Components;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Features.CommonComponents;
using SelfishFramework.Src.StateMachine;
using SelfishFramework.Src.Unity.Generated;

namespace Core.Features.LevelStatesFeature.States
{
    public class EliminateTilesState : BaseFSMState
    {
        private readonly Filter _filter;
        public override int StateID => LevelStateIdentifierMap.EliminateTilesState;


        public EliminateTilesState(StateMachine stateMachine) : base(stateMachine)
        {
            _filter = stateMachine.World.Filter
                .With<GameScreenTagComponent>()
                .With<GridMonoProviderComponent>()
                .With<ColorComponent>()
                .Without<VisualInProgressComponent>()
                .Build();
        }

        public override void Enter(Entity entity)
        {
            _filter.ForceUpdate();
        }

        public override void Exit(Entity entity)
        {
            
        }

        public override void Update(Entity entity)
        {
            foreach (var screenEntity in _filter)
            {
                ref var gridMonoProviderComponent = ref screenEntity.Get<GridMonoProviderComponent>();
                var grid = gridMonoProviderComponent.Grid;
                var currentColor = screenEntity.Get<ColorComponent>().Color;
                Span<(int, int)> toEliminate = stackalloc (int, int)[grid.Tiles.Count];
                int count = 0;
                
                foreach (var kv in grid.Tiles)
                {
                    if (kv.Value.Color == currentColor)
                    {
                        toEliminate[count++] = kv.Key;
                    } 
                }
                for (int i = 0; i < count; i++)
                {
                    var (x, y) = toEliminate[i];
                    var tile = grid.Tiles[(x, y)];
                    UnityEngine.Object.Destroy(tile.gameObject);
                    grid.Tiles.Remove((x, y));
                }

                EndState();
                break;
            }
        }
    }
}