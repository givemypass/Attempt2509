using Core.Features.LevelStatesFeature.States;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.StateMachine;
using SelfishFramework.Src.Unity.Generated;

namespace Core.Features.LevelStatesFeature
{
    public sealed partial class LevelsFsmSystem : BaseSystem
    {
        public override void InitSystem()
        {
            ref var fsmComponent = ref Owner.Get<LevelsFsmComponent>();
            var fsm = new StateMachine(Owner);
            
            fsm.AddState(new CheckConditionsState(fsm));
            fsm.AddState(new ChangeColorState(fsm));
            fsm.AddState(new EliminateTilesState(fsm));
            fsm.AddStateTransition(LevelStateIdentifierMap.ChangeColorState, new DefaultTransition(LevelStateIdentifierMap.EliminateTilesState));
            fsm.AddStateTransition(LevelStateIdentifierMap.EliminateTilesState, new DefaultTransition(LevelStateIdentifierMap.CheckConditionsState));
            fsm.AddStateTransition(LevelStateIdentifierMap.CheckConditionsState, new DefaultTransition(LevelStateIdentifierMap.ChangeColorState));
            
            fsm.ChangeState(LevelStateIdentifierMap.ChangeColorState);
            
            fsmComponent.Fsm = fsm;
        }
    }
}