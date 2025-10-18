using Core.Features.GameScreenFeature.Components;
using Core.Features.StepsFeature;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.StateMachine;
using SelfishFramework.Src.Unity.Generated;

namespace Core.Features.LevelStatesFeature.States
{
    public class CheckConditionsState : BaseFSMState
    {
        public override int StateID => LevelStateIdentifierMap.CheckConditionsState;
    
        private readonly Filter _tilesFilter;
        private readonly Single<StepsComponent> _stepsSingleComponent;

        public CheckConditionsState(StateMachine stateMachine) : base(stateMachine)
        {
            _stepsSingleComponent = new Single<StepsComponent>(stateMachine.World);
            _tilesFilter = stateMachine.World.Filter.With<TileCommonComponent>().Build();
        }

        public override void Enter(Entity entity)
        {
            _tilesFilter.ForceUpdate();
            if (_tilesFilter.SlowCount() == 0)
            {
                stateMachine.World.Command(new LevelCompletedCommand());
                stateMachine.Pause(true);
                return;
            }
            
            ref var stepsComponent = ref _stepsSingleComponent.Get();
            if (stepsComponent.Steps <= 0)
            {
                stateMachine.World.Command(new StepsRanOutCommand());
                stateMachine.Pause(true);
                return;
            }
            EndState();
        }
    
        public override void Exit(Entity entity)
        {
        }
    
        public override void Update(Entity entity)
        {
        }
    }
}