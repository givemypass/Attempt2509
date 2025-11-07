using Core.CommonCommands;
using Core.Features.GameScreenFeature;
using Core.Features.GameScreenFeature.Components;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules.CommandBusModule;
using SelfishFramework.Src.Features.CommonComponents;
using SelfishFramework.Src.StateMachine;
using SelfishFramework.Src.Unity.Generated;

namespace Core.Features.LevelStatesFeature.States
{
    public class ChangeColorState : BaseFSMState, IReactGlobal<MainColorChangedCommand>
    {
        public override int StateID => LevelStateIdentifierMap.ChangeColorState;
    
        private readonly Filter _filter;

        public ChangeColorState(StateMachine stateMachine) : base(stateMachine)
        {
            _filter = stateMachine.World.Filter.With<BackColorScreenUiActorComponent>().Build();
        }

        public override void Enter(Entity fsmEntity)
        {
            stateMachine.World.ModuleRegistry.GetModule<GlobalCommandModule>().Register(this);
        }
    
        public override void Exit(Entity fsmEntity)
        {
            stateMachine.World.ModuleRegistry.GetModule<GlobalCommandModule>().Unregister(this);
        }
    
        public override void Update(Entity fsmEntity)
        {
            foreach (var entity in _filter)
            {
                if (!entity.Has<WaitForChangingColorComponent>() && !entity.Has<VisualInProgressComponent>())
                {
                    entity.Set(new WaitForChangingColorComponent()); 
                }
            }
        }

        void IReactGlobal<MainColorChangedCommand>.ReactGlobal(MainColorChangedCommand command)
        {
            EndState();
        }
    }
}