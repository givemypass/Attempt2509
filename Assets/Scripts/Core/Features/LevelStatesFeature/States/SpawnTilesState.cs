using Core.Features.GameScreenFeature.Components;
using Core.Features.GameScreenFeature.Mono;
using Core.Models;
using Core.Services;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.StateMachine;
using SelfishFramework.Src.Unity.Generated;
using UnityEngine;

namespace Core.Features.LevelStatesFeature.States
{
    [Injectable]
    public partial class SpawnTilesState : BaseFSMState
    {
        [Inject] private GlobalConfigProvider _globalConfigProvider;
        [Inject] private ColorPaletteService _colorPaletteService;
        
        private readonly Filter _filter;
        public override int StateID => LevelStateIdentifierMap.SpawnTilesState;

        public SpawnTilesState(StateMachine stateMachine) : base(stateMachine)
        {
            _filter = stateMachine.World.Filter.With<GameScreenTagComponent>().With<GridMonoProviderComponent>().Build();
        }

        public override void Enter(Entity entity)
        {
            foreach (var screenEntity in _filter)
            {
                ref var gridMonoProviderComponent = ref screenEntity.Get<GridMonoProviderComponent>();
                var grid = gridMonoProviderComponent.Grid;
                if (grid.TryGetFreeCell(out var x, out var y, out var position))
                {
                    var tilePrefab = _globalConfigProvider.Get.TilePrefab;
                    var tile = Object.Instantiate(tilePrefab, position, Quaternion.identity, grid.transform);
                    var color = _colorPaletteService.RandomColorFromCurrentPalette();
                    tile.Image.color = color;
                    tile.Color = color;
                    grid.Tiles[(x, y)] = tile;
                }
                EndState();
                break;
            }
        }

        public override void Exit(Entity entity)
        {
            
        }

        public override void Update(Entity entity)
        {
            
        }
    }
}