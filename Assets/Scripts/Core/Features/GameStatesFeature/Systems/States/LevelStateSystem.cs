using Core.Features.StepsFeature;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Features.GameFSM.Commands;
using SelfishFramework.Src.Unity.Features.UI.Systems;
using SelfishFramework.Src.Unity.Generated;
using Systems;

namespace Core.Features.GameStatesFeature.Systems.States
{
    [Injectable]
    public sealed partial class LevelStateSystem : BaseGameStateSystem, IReactGlobal<StepsRanOutCommand>
    {
        [Inject] private IUIService _uiService;
        
        protected override int State => GameStateIdentifierMap.LevelState;

        public override void InitSystem()
        {
        }

        protected override void ProcessState(int from, int to)
        {
        }

        protected override void OnExitState()
        {
            _uiService.CloseUI(UIIdentifierMap.GameOverScreen_UIIdentifier);
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