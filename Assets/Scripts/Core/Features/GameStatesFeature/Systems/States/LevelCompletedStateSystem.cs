using Core.Features.LevelCompletedScreenFeature;
using Core.Features.LevelsFeature.Services;
using Core.Features.PlayerProgressFeature;
using Cysharp.Threading.Tasks;
using SelfishFramework.Src.Core;
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
        [Inject] private ILevelsService _levelsService;
        
        private Single<PlayerProgressComponent> _playerProgressSingle;
        
        protected override int State => GameStateIdentifierMap.LevelCompletedState;

        public override void InitSystem()
        {
            _playerProgressSingle = new Single<PlayerProgressComponent>(World);
        }

        protected override void ProcessState(int from, int to)
        {
            ref var playerProgressComponent = ref _playerProgressSingle.Get();
            playerProgressComponent.CurrentLevel = (playerProgressComponent.CurrentLevel + 1) % _levelsService.LevelsCount;
            ProcessAsync().Forget();
        }

        private async UniTask ProcessAsync()
        {
            var actor = await _uiService.ShowUIAsync(UIIdentifierMap.LevelCompletedScreen_UIIdentifier, groupId : UIGroupIdentifierMap.LevelUIGroup);    
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
            _uiService.CloseAllUIGroup(UIGroupIdentifierMap.LevelUIGroup);
        }
    }
}