using Core.Features.GameOverScreenFeature.Mono;
using Cysharp.Threading.Tasks;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Features.GameFSM.Commands;
using SelfishFramework.Src.Unity.Features.UI.Systems;
using SelfishFramework.Src.Unity.Generated;
using Systems;

namespace Core.Features.GameStatesFeature.Systems.States
{
    [Injectable]
    public sealed partial class GameOverStateSystem : BaseGameStateSystem
    {
        [Inject] private IUIService _uiService;
        
        protected override int State => GameStateIdentifierMap.GameOverState;

        public override void InitSystem()
        {
        }

        protected override void ProcessState(int from, int to)
        {
            ProcessAsync().Forget();
        }

        private async UniTask ProcessAsync()
        {
            var actor = await _uiService.ShowUIAsync(UIIdentifierMap.GameOverScreen_UIIdentifier);    
            actor.GetComponent<GameOverMonoComponent>().Reset.onClick.AddListener(() =>
            {
                World.Command(new ForceGameStateTransitionGlobalCommand
                {
                    GameState = GameStateIdentifierMap.BootstrapLevelState,
                });
            });
        }

        protected override void OnExitState()
        {
            _uiService.CloseAllUI();
        }
    }
}