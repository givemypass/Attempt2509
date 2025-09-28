﻿using Core.Features.GameScreenFeature;
using Core.Features.GameScreenFeature.Components;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Features.CommonComponents;
using SelfishFramework.Src.StateMachine;
using SelfishFramework.Src.Unity.Generated;
using UnityEditor.Localization.Plugins.XLIFF.V20;

namespace Core.Features.LevelStatesFeature.States
{
    public class ChangeColorState : BaseFSMState
    {
        public override int StateID => LevelStateIdentifierMap.ChangeColorState;
    
        private readonly Filter _filter;

        public ChangeColorState(StateMachine stateMachine) : base(stateMachine)
        {
            _filter = stateMachine.World.Filter.With<GameScreenUiActorComponent>().Build();
        }

        public override void Enter(Entity entity)
        {
        }
    
        public override void Exit(Entity entity)
        {
        }
    
        public override void Update(Entity entity)
        {
            foreach (var screenEntity in _filter)
            {
                if (screenEntity.Has<ColorChangedComponent>())
                {
                    screenEntity.Remove<ColorChangedComponent>();
                    EndState();
                    return;
                }

                if (!screenEntity.Has<WaitForChangingColorComponent>() && !screenEntity.Has<VisualInProgressComponent>())
                {
                    screenEntity.Set(new WaitForChangingColorComponent()); 
                }
            }
        }
    }
}