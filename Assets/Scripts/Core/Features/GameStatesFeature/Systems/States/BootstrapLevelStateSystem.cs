using System;
using Core.CommonComponents;
using Core.Features.GameScreenFeature;
using Core.Features.GameScreenFeature.Commands;
using Core.Features.GameScreenFeature.Components;
using Core.Features.GameScreenFeature.Mono;
using Core.Features.HintsFeature;
using Core.Features.LevelsFeature.Models;
using Core.Features.LevelsFeature.Services;
using Core.Features.PlayerProgressFeature;
using Core.Features.StepsFeature;
using Core.Features.TilesFeature;
using Core.Features.TilesFeature.Services;
using Core.Models;
using Core.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Features.GameFSM.Commands;
using SelfishFramework.Src.Unity.Features.UI.Actors;
using SelfishFramework.Src.Unity.Features.UI.Systems;
using SelfishFramework.Src.Unity.Generated;
using Systems;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Features.GameStatesFeature.Systems.States
{
    [Injectable]
    public sealed partial class BootstrapLevelStateSystem : BaseGameStateSystem
    {
        private const string SCENE_NAME = "Level";
        
        [Inject] private IColorPaletteService _colorPaletteService;
        [Inject] private ISceneService _sceneManager;
        [Inject] private IUIService _uiService;
        [Inject] private GlobalConfigProvider _globalConfigProvider;
        [Inject] private ILevelsService _levelsService;
        [Inject] private ITileFactoryService _tileFactoryService;
        
        private Single<PlayerProgressComponent> _playerProgressSingle;
        private Single<BackColorScreenUiActorComponent> _backColorActorSingle;

        protected override int State => GameStateIdentifierMap.BootstrapLevelState;

        public override void InitSystem()
        {
            _playerProgressSingle = new Single<PlayerProgressComponent>(World);
            _backColorActorSingle = new Single<BackColorScreenUiActorComponent>(World);
        }

        protected override void ProcessState(int from, int to)
        {
            ProcessStateAsync().Forget();
        }

        private async UniTask ProcessStateAsync()
        {
            _colorPaletteService.GeneratePalette();
            
            await _sceneManager.LoadSceneAsync(SCENE_NAME);

            _playerProgressSingle.ForceUpdate();
            var levelId = _playerProgressSingle.Get().CurrentLevel;
            var level = _levelsService.GetLevel(levelId);
            
            await UniTask.WhenAll(ShowHandlersScreen(), ShowLevelScreen(level, levelId));
            
            InitLevelActor(level.Steps);
            EndState();
        }

        private async UniTask ShowLevelScreen(LevelConfigModel level, int levelId)
        {
            var color = _colorPaletteService.GetColor(level.ColorId);
            
            _backColorActorSingle.ForceUpdate();
            _backColorActorSingle.GetEnt().Command(new ChangeColorCommand
            {
                Color = color,
                Direction = _colorPaletteService.GetDirection(level.ColorId),
            });
            
            var screen = await _uiService.ShowUIAsync(UIIdentifierMap.LevelScreen_UIIdentifier,
                additionalCanvas: AdditionalCanvasIdentifierMap.GameCanvas, groupId: UIGroupIdentifierMap.LevelUIGroup);
            screen.TryGetComponent(out LevelScreenMonoComponent monoComponent);
            Array.Sort(monoComponent.Grids, (a, b) => a.MaxTiles.CompareTo(b.MaxTiles));
            foreach (var grid in monoComponent.Grids)
            {
                if (grid.MaxTiles < level.Tiles.Count)
                {
                    continue;
                }

                ShowGrid(grid, screen);
                break;
            }
            monoComponent.LevelText.text = $"{levelId + 1}";
            await SpawnTiles(screen, level);
        }

        private void ShowGrid(GridMonoComponent grid, UIActor screen)
        {
            if (grid.GridImage != null)
            {
                var color = grid.GridImage.color;
                color.a = 0;
                grid.GridImage.color = color;
                grid.GridImage.DOFade(1, 0.25f);
            }

            grid.gameObject.SetActive(true);
            screen.Entity.Set(new GridMonoProviderComponent
            {
                Grid = grid,
            });
            SetColorSign(grid, Vector2Int.right);
            SetColorSign(grid, Vector2Int.left);
            SetColorSign(grid, Vector2Int.up);
            SetColorSign(grid, Vector2Int.down);
        }

        private void SetColorSign(GridMonoComponent grid, Vector2Int dir)
        {
            var color = _colorPaletteService.GetColor(dir);
            var sign = grid.GetSignImage(dir);
            sign.SetColor(color);
            sign.PlayShowState();
        }

        private async UniTask ShowHandlersScreen()
        {
            var handlersScreen = await _uiService.ShowUIAsync(UIIdentifierMap.LevelHandlersScreen_UIIdentifier, groupId : UIGroupIdentifierMap.LevelUIGroup);
            var monoComponent = handlersScreen.GetComponent<LevelHandlersScreenMonoComponent>();
            monoComponent.ResetButton.onClick.AddListener(() =>
            {
                _uiService.CloseAllUIGroup(UIGroupIdentifierMap.LevelUIGroup);
                World.Command(new ForceGameStateTransitionGlobalCommand
                {
                    GameState = GameStateIdentifierMap.BootstrapLevelState,
                });
            });
            monoComponent.HintButton.onClick.AddListener(() =>
            {
                World.Command(new ShowHintCommand());
            });
        }

        private async UniTask SpawnTiles(UIActor screen, LevelConfigModel level)
        {
            var grid = screen.Entity.Get<GridMonoProviderComponent>().Grid;
            const int delay = 50;
            foreach (var tile in level.Tiles)
            {
                await UniTask.Delay(delay);
                if (grid.TryGetFreeCell(out var x, out var y, out var position))
                {
                    var tileActor = _tileFactoryService.GetTile(tile.Tile, position, grid.transform);
                    tileActor.transform.localScale = Vector3.zero;
                    tileActor.transform.DOScale(Vector3.one * grid.Scale, 0.2f).SetLink(tileActor.gameObject);

                    grid.Tiles[(x, y)] = tileActor.Entity;
                    tileActor.Entity.Set(new GridPositionComponent
                    {
                        Position = new Vector2Int(x, y),
                    });
                }
                else
                {
                    throw new Exception("No free cell found in grid while spawning initial tiles.");
                }
            }
        }

        private void InitLevelActor(int levelSteps)
        {
            var levelActor = Object.Instantiate(_globalConfigProvider.Get.LevelActor);
            levelActor.TryInitialize();
            ref var stepsComponent = ref levelActor.Entity.Get<StepsComponent>();
            stepsComponent.Steps = levelSteps;
        }
    }
}