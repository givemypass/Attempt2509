using Core.Features.LevelCompletedScreenFeature;
using Cysharp.Threading.Tasks;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Unity.Features.UI.Systems;
using SelfishFramework.Src.Unity.Generated;
using Systems;

namespace Core.Features.GameStatesFeature.Systems.States
{
    [Injectable]
    public sealed partial class LevelCompletedStateSystem : BaseGameStateSystem
    {
        [Inject] private IUIService _uiService;
        
        protected override int State => GameStateIdentifierMap.LevelCompletedState;

        public override void InitSystem()
        {
        }

        protected override void ProcessState(int from, int to)
        {
            ProcessAsync().Forget();
        }

        private async UniTask ProcessAsync()
        {
            var actor = await _uiService.ShowUIAsync(UIIdentifierMap.LevelCompletedScreen_UIIdentifier);    
            actor.GetComponent<LevelCompletedMonoComponent>().NextLevel.onClick.AddListener(() =>
            {
                World.Command(new SelfishFramework.Src.Features.GameFSM.Commands.ForceGameStateTransitionGlobalCommand
                {
                    GameState = GameStateIdentifierMap.BootstrapLevelState,
                });
            });
        }

        protected override void OnExitState()
        {
            _uiService.CloseUI(UIIdentifierMap.LevelCompletedScreen_UIIdentifier);
            _uiService.CloseUI(UIIdentifierMap.GameScreen_UIIdentifier);
        }
    }
}