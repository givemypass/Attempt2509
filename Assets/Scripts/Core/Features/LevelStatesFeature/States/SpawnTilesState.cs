using Core.CommonComponents;
using Core.Features.GameScreenFeature;
using Core.Features.GameScreenFeature.Components;
using Core.Features.TilesFeature;
using Core.Features.TilesFeature.SimpleTile;
using Core.Features.TilesFeature.TileWithInner;
using Core.Services;
using DG.Tweening;
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
        [Inject] private SimpleTileFactoryService _simpleTileFactoryService;
        [Inject] private TileWithInnerFactoryService _tileWithInnerFactoryService;
        
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
                    //todo
                    var color = _colorPaletteService.RandomColorFromCurrentPaletteExcept(colorComponent.Color);
                    var simpleTileActor = _simpleTileFactoryService.GetTile(position, grid.transform, color);
                    simpleTileActor.Entity.Set(new ColorComponent
                    {
                        Color = color,
                    });
                    var secondColor = _colorPaletteService.RandomColorFromCurrentPaletteExcept(colorComponent.Color, color);
                    var tileActor = _tileWithInnerFactoryService.GetTile(position, grid.transform, secondColor, simpleTileActor);
                    tileActor.Entity.Set(new ColorComponent
                    {
                        Color = secondColor,
                    });
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

        public override void Exit(Entity entity)
        {
            
        }

        public override void Update(Entity entity)
        {
            
        }
    }
}