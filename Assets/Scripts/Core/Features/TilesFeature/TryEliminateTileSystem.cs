using Core.CommonComponents;
using Core.Features.GameScreenFeature;
using Core.Features.GameScreenFeature.Components;
using Core.Features.LevelStatesFeature.Component;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;

namespace Core.Features.TilesFeature
{
    public sealed partial class TryEliminateTileSystem : BaseSystem, IUpdatable
    {
        private Filter _filter;
        private Filter _screenFilter;

        public override void InitSystem()
        {
            _filter = World.Filter.With<TileCommonComponent>().With<TryEliminateComponent>().Build();
            
            _screenFilter = World.Filter
                .With<LevelScreenUiActorComponent>()
                .With<GridMonoProviderComponent>()
                .With<ColorComponent>()
                .Build();
        }

        public void Update()
        {
            foreach (var screenEntity in _screenFilter)
            {
                var currentColor = screenEntity.Get<ColorComponent>().Color;
                
                foreach (var entity in _filter)
                {
                    entity.Remove<TryEliminateComponent>();
                    var color = entity.Get<ColorComponent>().Color;
                    if (color != currentColor)
                    {
                        continue;
                    }
                    entity.Set(new EliminateComponent());
                }
            }
        }
    }
}