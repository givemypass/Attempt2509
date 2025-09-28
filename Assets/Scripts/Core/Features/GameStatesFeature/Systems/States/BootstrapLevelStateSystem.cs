using Core.CommonComponents;
using Core.Features.GameScreenFeature.Mono;
using Core.Services;
using Cysharp.Threading.Tasks;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Unity.Features.UI.Systems;
using SelfishFramework.Src.Unity.Generated;
using Systems;
using UnityEngine;

namespace Core.Features.GameStatesFeature.Systems.States
{
    [Injectable]
    public sealed partial class BootstrapLevelStateSystem : BaseGameStateSystem
    {
        private const string SCENE_NAME = "Level";
        
        [Inject] private IColorPaletteService _colorPaletteService;
        [Inject] private ISceneService _sceneManager;
        [Inject] private IUIService _uiService;

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
            var screen = await _uiService.ShowUIAsync(UIIdentifierMap.GameScreen_UIIdentifier);
            screen.TryGetComponent(out GameScreenMonoComponent monoComponent);
            var color = _colorPaletteService.RandomColorFromCurrentPaletteExcept(Color.white);
            monoComponent.BackgroundImage.color = color;
            screen.Entity.Set(new ColorComponent
            {
                Color = color,
            });
            monoComponent.ColorSigns[0].color = _colorPaletteService.GetColor(Vector2Int.right);
            monoComponent.ColorSigns[1].color = _colorPaletteService.GetColor(Vector2Int.down);
            monoComponent.ColorSigns[2].color = _colorPaletteService.GetColor(Vector2Int.left);
            monoComponent.ColorSigns[3].color = _colorPaletteService.GetColor(Vector2Int.up);
        }
    }
}