using Core.CommonComponents;
using Core.Features.GameScreenFeature;
using Core.Features.GameScreenFeature.Components;
using Core.Features.GameScreenFeature.Mono;
using Core.Features.LevelStatesFeature.Component;
using DG.Tweening;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Features.CommonComponents;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity;
using UnityEngine;

namespace Core.Features.TilesFeature
{
    public sealed partial class EliminateSimpleTileSystem : BaseSystem, IUpdatable
    {
        private Filter _filter;
        private Filter _simpleTilesFilter;

        public override void InitSystem()
        {
            _filter = World.Filter
                .With<GameScreenUiActorComponent>()
                .With<GridMonoProviderComponent>()
                .With<ColorComponent>()
                .Build();
            
            _simpleTilesFilter = World.Filter
                .With<SimpleTileActorComponent>()
                .With<TryEliminateComponent>()
                .Build();
        }

        public void Update()
        {
            foreach (var screenEntity in _filter)
            {
                ref var gridMonoProviderComponent = ref screenEntity.Get<GridMonoProviderComponent>();
                var grid = gridMonoProviderComponent.Grid;
                var currentColor = screenEntity.Get<ColorComponent>().Color;

                foreach (var entity in _simpleTilesFilter)
                {
                    entity.Remove<TryEliminateComponent>();
                
                    var color = entity.Get<ColorComponent>().Color;
                    if (color != currentColor)
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
                    var monoComponent = actor.GetComponent<SimpleTileMonoComponent>();
                    monoComponent.Image.color = Color.white;
                    actor.transform.DOScale(Vector3.zero, 0.2f).SetLink(actor.gameObject).OnComplete(() =>
                    {
                        Object.Destroy(actor);
                    });
                }
            } 
        }
    }
}