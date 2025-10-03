using Core.CommonComponents;
using Core.Features.GameScreenFeature;
using Core.Features.GameScreenFeature.Components;
using Core.Features.GameScreenFeature.Mono;
using Core.Features.TilesFeature;
using Core.Features.TilesFeature.SimpleTile;
using Core.Features.TilesFeature.TileWithInner;
using Core.Services;
using DG.Tweening;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.StateMachine;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.Generated;
using UnityEngine;

namespace Core.Features.LevelStatesFeature.States
{
    [Injectable]
    public partial class SpawnTilesState : BaseFSMState
    {
        [Inject] private SimpleTileFactoryService _simpleTileFactoryService;
        [Inject] private ComplexTileFactoryService _complexTileFactoryService;
        
        [Inject] private IColorPaletteService _colorPaletteService;
        
        private readonly Filter _filter;
        public override int StateID => LevelStateIdentifierMap.SpawnTilesState;

        public SpawnTilesState(StateMachine stateMachine) : base(stateMachine)
        {
            _filter = stateMachine.World.Filter.With<GameScreenUiActorComponent>().With<GridMonoProviderComponent>().Build();
        }

        public override void Enter(Entity entity)
        {
            foreach (var screenEntity in _filter)
            {
                ref var gridMonoProviderComponent = ref screenEntity.Get<GridMonoProviderComponent>();
                ref var colorComponent = ref screenEntity.Get<ColorComponent>();
                var grid = gridMonoProviderComponent.Grid;
                if (grid.TryGetFreeCell(out var x, out var y, out var position))
                {
                    var tileActor = Random.value > 0.5
                        ? CreateTileWithSimpleInner(colorComponent.Color, position, grid)
                        : CreateTileWithInnerTileWithSimpleInner(colorComponent.Color, position, grid);
                    tileActor.transform.localScale = Vector3.zero;
                    tileActor.transform.DOScale(Vector3.one, 0.2f).SetLink(tileActor.gameObject);

                    grid.Tiles[(x, y)] = tileActor.Entity;
                    tileActor.Entity.Set(new GridPositionComponent
                    {
                        Position = new Vector2Int(x, y),
                    });
                }

                EndState();
                break;
            }
        }

        private Actor CreateTileWithSimpleInner(Color screenColor, Vector2 position, GridMonoComponent grid)
        {
            var color = _colorPaletteService.RandomColorFromCurrentPaletteExcept(screenColor);
                    
            var secondColor = _colorPaletteService.RandomColorFromCurrentPaletteExcept(screenColor, color);
            var simpleTileActor = _simpleTileFactoryService.GetTile(position, grid.transform, secondColor);
            simpleTileActor.Entity.Set(new ColorComponent
            {
                Color = secondColor,
            });
                    
            var tileActor = _complexTileFactoryService.GetTile(position, grid.transform, color, simpleTileActor);
            tileActor.Entity.Set(new ColorComponent
            {
                Color = color,
            });
            return tileActor;
        }
        
        //hehe
        private Actor CreateTileWithInnerTileWithSimpleInner(Color screenColor, Vector2 position, GridMonoComponent grid)
        {
            var color = _colorPaletteService.RandomColorFromCurrentPaletteExcept(screenColor);
            var secondColor = _colorPaletteService.RandomColorFromCurrentPaletteExcept(color);
            var thirdColor = _colorPaletteService.RandomColorFromCurrentPaletteExcept(secondColor);
            
            var thirdInnerTile = _simpleTileFactoryService.GetTile(position, grid.transform, thirdColor);
            thirdInnerTile.Entity.Set(new ColorComponent
            {
                Color = thirdColor,
            });
            
            var innerTile = _complexTileFactoryService.GetTile(position, grid.transform, secondColor, thirdInnerTile);
            innerTile.Entity.Set(new ColorComponent
            {
                Color = secondColor,
            });
                    
            var tileActor = _complexTileFactoryService.GetTile(position, grid.transform, color, innerTile);
            tileActor.Entity.Set(new ColorComponent
            {
                Color = color,
            });
            return tileActor;
        }
        
        private Actor CreateSimpleTile(Color screenColor, Vector2 position, GridMonoComponent grid)
        {
            var color = _colorPaletteService.RandomColorFromCurrentPaletteExcept(screenColor);
                    
            var tileActor = _simpleTileFactoryService.GetTile(position, grid.transform, color);
            tileActor.Entity.Set(new ColorComponent
            {
                Color = color,
            });
                    
            return tileActor;
        }

        public override void Exit(Entity entity)
        {
            
        }

        public override void Update(Entity entity)
        {
            
        }
    }
}