using Core.Features.GameScreenFeature.Components;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.StateMachine;
using SelfishFramework.Src.Unity.Generated;

namespace Core.Features.LevelStatesFeature.States
{
    public class ChangeColorState : BaseFSMState
    {
        public override int StateID => LevelStateIdentifierMap.ChangeColorState;
    
        private readonly Filter _filter;

        public ChangeColorState(StateMachine stateMachine) : base(stateMachine)
        {
            _filter = stateMachine.World.Filter.With<GameScreenTagComponent>()
                .Without<WaitForChangingColorComponent>().Build();
        }

        public override void Enter(Entity entity)
        {
            foreach (var screenEntity in _filter)
            {
                screenEntity.Set(new WaitForChangingColorComponent()); 
            }
            _filter.ForceUpdate();
        }
    
        public override void Exit(Entity entity)
        {
        }
    
        public override void Update(Entity entity)
        {
            if (_filter.IsNotEmpty())
            {
                EndState();
            }
        }
    }
}