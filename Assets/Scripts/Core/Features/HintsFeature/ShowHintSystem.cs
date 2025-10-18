using System.Collections.Generic;
using Core.Features.GameScreenFeature.Components;
using Core.Features.GameScreenFeature.Mono;
using Core.Features.MinStepsCalculationFeature;
using Core.Features.TilesFeature;
using Core.Features.TilesFeature.Models;
using Core.Services;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity;

namespace Core.Features.HintsFeature
{
    [Injectable]
    public sealed partial class ShowHintSystem : BaseSystem, IReactGlobal<ShowHintCommand>
    {
        [Inject] private MinStepsCalculator _minStepsCalculator;
        [Inject] private IColorPaletteService _colorPaletteService;
        
        private Filter _filter;

        public override void InitSystem()
        {
            _filter = World.Filter.With<TileCommonComponent>().With<GridPositionComponent>().Build();
        }

        void IReactGlobal<ShowHintCommand>.ReactGlobal(ShowHintCommand command)
        {
            if (!Owner.Has<WaitForChangingColorComponent>())
            {
                return;
            }

            var tileModels = new List<ITileModel>();
            foreach (var tileEntity in _filter)
            {
                ref var tileCommonComponent = ref tileEntity.Get<TileCommonComponent>();
                tileModels.Add(tileCommonComponent.Model);
            }

            var state = MinStepCalculatorUtils.GetState(tileModels);
            var (_, path) = _minStepsCalculator.MinSteps(state);
            if (path.Length == 0)
            {
                SLog.LogError("No possible moves to show as hint");
            }

            var nextMove = path[0];
            var direction = _colorPaletteService.GetDirection(nextMove);
            
            var monoComponent = Owner.AsActor().GetComponent<LevelScreenMonoComponent>();
            foreach (var grid in monoComponent.Grids)
            {
                if (!grid.gameObject.activeSelf)
                {
                    continue; 
                }

                var sign = grid.GetSignImage(direction);
                sign.PlayHintState();
            }
        }
    }
}