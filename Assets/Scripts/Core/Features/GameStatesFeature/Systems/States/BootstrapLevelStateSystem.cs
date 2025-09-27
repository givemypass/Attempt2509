using Core.Services;
using Cysharp.Threading.Tasks;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Unity.Generated;
using SelfishFramework.Src.Unity.UI.Systems;
using Systems;

namespace Core.Features.GameStatesFeature.Systems.States
{
    [Injectable]
    public sealed partial class BootstrapLevelStateSystem : BaseGameStateSystem
    {
        private const string SCENE_NAME = "Level";
        
        [Inject] private ColorPaletteService _colorPaletteService;
        [Inject] private SceneService _sceneManager;
        [Inject] private UIService _uiService;

        protected override int State => GameStateIdentifierMap.BootstrapLevelState;

        public override void InitSystem()
        {
        }

        protected override void ProcessState(int from, int to)
        {
            ProcessStateAsync().Forget();
        }

        private async UniTask ProcessStateAsync()
        {
            _colorPaletteService.GeneratePalette();
            
            await _sceneManager.LoadSceneAsync(SCENE_NAME);
            await _uiService.ShowUIAsync(UIIdentifierMap.GameScreen_UIIdentifier);
        }
    }
}