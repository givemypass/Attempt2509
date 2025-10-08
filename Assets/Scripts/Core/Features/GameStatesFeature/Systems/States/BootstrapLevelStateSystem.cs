using System;
using System.Collections.Generic;
using Core.CommonComponents;
using Core.Features.GameScreenFeature.Components;
using Core.Features.GameScreenFeature.Mono;
using Core.Features.LevelsFeature.Models;
using Core.Features.LevelsFeature.Services;
using Core.Features.StepsFeature;
using Core.Features.TilesFeature;
using Core.Features.TilesFeature.Services;
using Core.Models;
using Core.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity.Features.UI.Actors;
using SelfishFramework.Src.Unity.Features.UI.Systems;
using SelfishFramework.Src.Unity.Generated;
using Systems;
using UnityEngine;
using UnityEngine.Tilemaps;
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
            
            var levelId = 0;
            var level = _levelsService.GetLevel(levelId);
            
            InitLevelActor(level.Steps);

            var screen = await _uiService.ShowUIAsync(UIIdentifierMap.GameScreen_UIIdentifier);
            screen.TryGetComponent(out GameScreenMonoComponent monoComponent);
            var color = _colorPaletteService.GetColor(level.ColorId);
            monoComponent.BackgroundImage.color = color;
            screen.Entity.Set(new ColorComponent
            {
                Color = color,
            });
            monoComponent.ColorSigns[0].color = _colorPaletteService.GetColor(Vector2Int.right);
            monoComponent.ColorSigns[1].color = _colorPaletteService.GetColor(Vector2Int.down);
            monoComponent.ColorSigns[2].color = _colorPaletteService.GetColor(Vector2Int.left);
            monoComponent.ColorSigns[3].color = _colorPaletteService.GetColor(Vector2Int.up);
            
            SpawnTiles(screen, level);
            var minSteps = MinStepCalculatorUtils.CalculateMinSteps(level);
            SLog.Log($"Min moves to complete level: {minSteps.steps} : {MinStepCalculatorUtils.Encode(minSteps.path)}");
        }

        private void SpawnTiles(UIActor screen, LevelConfigModel level)
        {
            ref var gridMonoProviderComponent = ref screen.Entity.Get<GridMonoProviderComponent>();
            var grid = gridMonoProviderComponent.Grid;
            foreach (var tile in level.Tiles)
            {
                if (grid.TryGetFreeCell(out var x, out var y, out var position))
                {
                    var tileActor = _tileFactoryService.GetTile(tile.Tile, position, grid.transform);
                    tileActor.transform.localScale = Vector3.zero;
                    tileActor.transform.DOScale(Vector3.one, 0.2f).SetLink(tileActor.gameObject);

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