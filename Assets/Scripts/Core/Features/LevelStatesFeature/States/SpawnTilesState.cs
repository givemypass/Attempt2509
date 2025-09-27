using SelfishFramework.Src.Core;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.StateMachine;
using SelfishFramework.Src.Unity.Generated;

namespace Core.Features.LevelStatesFeature.States
{
    public class SpawnTilesState : BaseFSMState
    {
        public override int StateID => LevelStateIdentifierMap.SpawnTilesState;

        public SpawnTilesState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter(Entity entity)
        {
            SLog.Log("spawn tiles");
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