using Core.CommonComponents;
using Core.Features.GameScreenFeature;
using Core.Features.GameScreenFeature.Components;
using Core.Features.GameScreenFeature.Mono;
using Core.Features.TilesFeature;
using Core.Models;
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
        [Inject] private GlobalConfigProvider _globalConfigProvider;
        [Inject] private ColorPaletteService _colorPaletteService;
        
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
                    var simpleTileActor = _globalConfigProvider.Get.SimpleTilePrefab;
                    var tileActor = Object.Instantiate(simpleTileActor, position, Quaternion.identity, grid.transform);
                    tileActor.TryInitialize();
                    tileActor.transform.localScale = Vector3.zero;
                    var color = _colorPaletteService.RandomColorFromCurrentPaletteExcept(colorComponent.Color);
                    var monoComponent = tileActor.GetComponent<SimpleTileMonoComponent>();
                    monoComponent.Image.color = color;
                    tileActor.Entity.Set(new GridPositionComponent
                    {
                        Position = new Vector2Int(x, y),
                    });
                    tileActor.Entity.Set(new ColorComponent
                    {
                        Color = color,
                    });
                    grid.Tiles[(x, y)] = tileActor.Entity;
                    tileActor.transform.DOScale(Vector3.one, 0.2f).SetLink(tileActor.gameObject);
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