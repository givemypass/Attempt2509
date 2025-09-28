using System;
using System.Collections.Generic;
using Core.CommonComponents;
using Core.Features.GameScreenFeature.Components;
using Core.Features.GameScreenFeature.Mono;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Features.CommonComponents;
using SelfishFramework.Src.StateMachine;
using SelfishFramework.Src.Unity.Generated;
using UnityEngine;

namespace Core.Features.LevelStatesFeature.States
{
    public class EliminateTilesState : BaseFSMState
    {
        private readonly Filter _filter;
        public override int StateID => LevelStateIdentifierMap.EliminateTilesState;

        private readonly HashSet<Tween> _tweens = new();

        public EliminateTilesState(StateMachine stateMachine) : base(stateMachine)
        {
            _filter = stateMachine.World.Filter
                .With<GameScreenTagComponent>()
                .With<GridMonoProviderComponent>()
                .With<ColorComponent>()
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
                if (screenEntity.Has<VisualInProgressComponent>())
                {
                    Eliminate(screenEntity);
                }
                else if (_tweens.Count == 0)
                {
                    EndState();
                }
            }
        }

        private void Eliminate(Entity screenEntity)
        {
            ref var gridMonoProviderComponent = ref screenEntity.Get<GridMonoProviderComponent>();
            var grid = gridMonoProviderComponent.Grid;
            var currentColor = screenEntity.Get<ColorComponent>().Color;
            Span<(int, int)> toEliminate = stackalloc (int, int)[grid.Tiles.Count];
            int count = 0;
                
            foreach (var kv in grid.Tiles)
            {
                if (kv.Value.Image.color == currentColor)
                {
                    toEliminate[count++] = kv.Key;
                } 
            }
            for (int i = 0; i < count; i++)
            {
                var (x, y) = toEliminate[i];
                var tile = grid.Tiles[(x, y)];
                tile.Image.color = Color.white;
                grid.Tiles.Remove((x, y));
                Destroy(tile).Forget();
            }
        }

        private async UniTask Destroy(TileMonoComponent tile)
        {
            var tween = tile.transform.DOScale(Vector3.zero, 0.2f).SetLink(tile.gameObject).OnComplete(() =>
            {
                UnityEngine.Object.Destroy(tile.gameObject);
            });
            _tweens.Add(tween);
            await tween;
            _tweens.Remove(tween);
        }
    }
}