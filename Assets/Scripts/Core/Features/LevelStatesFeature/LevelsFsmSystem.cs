using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.StateMachine;
using SelfishFramework.Src.Unity.Generated;

namespace Core.Features.LevelStatesFeature
{
    public sealed partial class LevelsFsmSystem : BaseSystem, IUpdatable
    {
        public override void InitSystem()
        {
            ref var fsmComponent = ref Owner.Get<LevelsFsmComponent>();
            var fsm = new StateMachine(Owner);
            fsm.AddState(new SpawnTilesState(fsm));
            fsm.AddState(new ChangeColorState(fsm));
            fsm.ChangeState(LevelStateIdentifierMap.SpawnTilesState);
            
            fsmComponent.Fsm = fsm;
        }

        public void Update()
        {
            
        }
    }
}