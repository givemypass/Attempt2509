using SelfishFramework.Src.Core;
using SelfishFramework.Src.StateMachine;
using SelfishFramework.Src.Unity.Generated;

namespace Core.Features.LevelStatesFeature.States
{
    public class EliminateTilesState : BaseFSMState
    {
        public override int StateID => LevelStateIdentifierMap.EliminateTilesState;


        public EliminateTilesState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter(Entity entity)
        {
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