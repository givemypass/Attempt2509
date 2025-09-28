using System;
using System.Collections.Generic;
using Core.CommonComponents;
using Core.Features.GameScreenFeature;
using Core.Features.GameScreenFeature.Components;
using Core.Features.GameScreenFeature.Mono;
using Core.Features.LevelStatesFeature.Component;
using Core.Features.TilesFeature;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Features.CommonComponents;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.StateMachine;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.Generated;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

namespace Core.Features.LevelStatesFeature.States
{
    public class EliminateTilesState : BaseFSMState
    {
        private readonly Filter _filter;
        private readonly Filter _tileFilter;
        public override int StateID => LevelStateIdentifierMap.EliminateTilesState;

        public EliminateTilesState(StateMachine stateMachine) : base(stateMachine)
        {
            _filter = stateMachine.World.Filter
                .With<GameScreenUiActorComponent>()
                .With<GridMonoProviderComponent>()
                .With<ColorComponent>()
                .Build();
            _tileFilter = stateMachine.World.Filter.With<TileCommonTagComponent>().Build();
        }

        public override void Enter(Entity entity)
        {
            foreach (var tileEntity in _tileFilter)
            {
                tileEntity.Set(new TryEliminateComponent());
            }
        }

        public override void Exit(Entity entity)
        {
            
        }

        public override void Update(Entity entity)
        {
            foreach (var screenEntity in _filter)
            {
                Eliminate(screenEntity);

                foreach (var tileEntity in _tileFilter)
                {
                    if (tileEntity.Has<TryEliminateComponent>() || tileEntity.Has<VisualInProgressComponent>())
                    {
                        return;
                    }
                }
                EndState();
            }
        }

        private void Eliminate(Entity screenEntity)
        {
            ref var gridMonoProviderComponent = ref screenEntity.Get<GridMonoProviderComponent>();
            var grid = gridMonoProviderComponent.Grid;
            var currentColor = screenEntity.Get<ColorComponent>().Color;
            
            foreach (var entity in screenEntity.GetWorld().Filter.With<SimpleTileActorComponent>().With<TryEliminateComponent>().Build())
            {
                entity.Remove<TryEliminateComponent>();
                
                var color = entity.Get<ColorComponent>().Color;
                if (color != currentColor)
                {
                    continue;
                }
                var position = entity.Get<GridPositionComponent>().Position;
                if (!grid.Tiles.ContainsKey((position.x, position.y)))
                {
                    SLog.LogError("Grid does not contain tile at position " + position);
                    continue;
                }
                
                entity.Set(new VisualInProgressComponent());
                
                grid.Tiles.Remove((position.x, position.y));
                
                var actor = entity.AsActor();
                var monoComponent = actor.GetComponent<SimpleTileMonoComponent>();
                monoComponent.Image.color = Color.white;
                actor.transform.DOScale(Vector3.zero, 0.2f).SetLink(actor.gameObject).OnComplete(() =>
                {
                    UnityEngine.Object.Destroy(actor);
                });
            }
        }
    }
}