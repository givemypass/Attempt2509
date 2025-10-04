using Core.Features.StepsFeature;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Features.GameFSM.Commands;
using SelfishFramework.Src.Unity.Generated;
using Systems;

namespace Core.Features.GameStatesFeature.Systems.States
{
    [Injectable]
    public sealed partial class GameOverStateSystem : BaseGameStateSystem
    {
        protected override int State => GameStateIdentifierMap.GameOverState;

        public override void InitSystem()
        {
        }

        protected override void ProcessState(int from, int to)
        {
        }

        protected override void OnExitState()
        {
        }

        public void ReactGlobal(StepsRanOutCommand command)
        {
            World.Command(new ForceGameStateTransitionGlobalCommand
            {
                GameState = GameStateIdentifierMap.GameOverState,
            });
        }
    }
}