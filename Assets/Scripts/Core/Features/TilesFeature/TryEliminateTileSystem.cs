using Core.CommonComponents;
using Core.Features.GameScreenFeature;
using Core.Features.GameScreenFeature.Components;
using Core.Features.LevelStatesFeature.Component;
using Core.Features.TilesFeature.Models;
using Core.Services;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using UnityEngine.SubsystemsImplementation;

namespace Core.Features.TilesFeature
{
    [Injectable]
    public sealed partial class TryEliminateTileSystem : BaseSystem, IUpdatable
    {
        [Inject] private IColorPaletteService _colorPaletteService;
        
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
                var currentColorId = _colorPaletteService.GetColorId(currentColor);
                
                foreach (var entity in _filter)
                {
                    entity.Remove<TryEliminateComponent>();
                    var model = entity.Get<TileCommonComponent>().Model;
                    switch (model)
                    {
                        case SimpleTileModel simpleTileModel:
                            if (!simpleTileModel.Colors.ContainsColor(currentColorId))
                            {
                                continue;
                            }
                            break;
                        case ComplexTileModel complexTileModel:
                            var colorId = complexTileModel.ColorId;
                            if (colorId != currentColorId)
                            {
                                continue;
                            }
                            break;
                        default:
                            continue;
                    }
                    entity.Set(new EliminateComponent());
                }
            }
        }
    }
}