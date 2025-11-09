using Core.CommonComponents;
using Core.Features.GameScreenFeature;
using Core.Features.GameScreenFeature.Components;
using Core.Features.LevelStatesFeature.Component;
using Core.Features.TilesFeature.Models;
using Core.Features.TilesFeature.TileWithInner;
using DG.Tweening;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Features.CommonComponents;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity;
using UnityEngine;

namespace Core.Features.TilesFeature.ComplexTile
{
    [Injectable]
    public sealed partial class EliminateComplexTileSystem : BaseSystem, IUpdatable
    {
        private Filter _gridFilter;
        private Filter _tilesFilter;

        public override void InitSystem()
        {
            _gridFilter = World.Filter
                .With<GridMonoProviderComponent>()
                .Build();
            
            _tilesFilter = World.Filter
                .With<ComplexTileActorComponent>()
                .With<TryEliminateComponent>()
                .Build();
        }

        public void Update()
        {
            foreach (var screenEntity in _gridFilter)
            {
                ref var gridMonoProviderComponent = ref screenEntity.Get<GridMonoProviderComponent>();
                var grid = gridMonoProviderComponent.Grid;

                foreach (var entity in _tilesFilter)
                {
                    ref var tryEliminateComponent = ref entity.Get<TryEliminateComponent>();
                    var targetColorId = tryEliminateComponent.ColorId;
                    entity.Remove<TryEliminateComponent>();
                    
                    var model = (ComplexTileModel)entity.Get<TileCommonComponent>().Model;
                    var colorId = model.ColorId;
                    if (colorId != targetColorId)
                    {
                        continue;
                    }
                
                    var position = entity.Get<GridPositionComponent>().Position;
                    if (!grid.Tiles.ContainsKey((position.x, position.y)))
                    {
                        SLog.LogError("Grid does not contain tile at position " + position);
                        continue;
                    }
                
                    entity.Set(new VisualInProgressComponent());
                
                    grid.Tiles.Remove((position.x, position.y));
                
                    var actor = entity.AsActor();
                    ref var tileMonoProviderComponent = ref entity.Get<TileMonoProviderComponent<ComplexTileMonoComponent>>();
                    var monoComponent = tileMonoProviderComponent.Get;
                    monoComponent.Image.color = Color.white;
                    var scale = actor.transform.localScale;
                    actor.transform.DOScale(Vector3.zero, 0.2f).SetLink(actor.gameObject).OnComplete(() =>
                    {
                        Object.Destroy(actor);
                    });
                    
                    var tileActor = monoComponent.InnerParent.GetChild(0).GetComponent<Actor>();
                    tileActor.transform.SetParent(grid.transform, true);
                    tileActor.transform.DOScale(scale, 0.1f).SetLink(tileActor.gameObject);
                    tileActor.Entity.Set(new GridPositionComponent
                    {
                        Position = position,
                    });
                    grid.Tiles[(position.x, position.y)] = tileActor.Entity;
                    tileActor.Entity.Set(new UpdateTileComponent());
                }
            } 
        }
    }
}