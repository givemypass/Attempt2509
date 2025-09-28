using Core.CommonComponents;
using Core.Features.GameScreenFeature;
using Core.Features.GameScreenFeature.Components;
using Core.Features.GameScreenFeature.Mono;
using Core.Features.LevelStatesFeature.Component;
using Core.Features.TilesFeature;
using DG.Tweening;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Features.CommonComponents;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.StateMachine;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.Generated;
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

            _tileFilter = stateMachine.World.Filter.With<TileCommonTagComponent>().With<GridPositionComponent>().Build();
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
}