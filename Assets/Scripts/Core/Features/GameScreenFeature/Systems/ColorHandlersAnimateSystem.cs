using Core.CommonCommands;
using Core.Features.GameScreenFeature.Mono;
using Core.Services;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity;

namespace Core.Features.GameScreenFeature.Systems
{
    [Injectable]
    public sealed partial class ColorHandlersAnimateSystem : BaseSystem, IReactGlobal<MainColorChangedCommand>
    {
        [Inject] private IColorPaletteService _colorPaletteService;
        
        private LevelScreenMonoComponent _monoComponent;
        
        public override void InitSystem()
        {
            Owner.AsActor().TryGetComponent(out _monoComponent);
        }

        void IReactGlobal<MainColorChangedCommand>.ReactGlobal(MainColorChangedCommand command)
        {
            foreach (var grid in _monoComponent.Grids)
            {
                if (!grid.gameObject.activeSelf)
                {
                    continue;
                }

                foreach (var sign in grid.ColorSigns())
                {
                    sign.RewindState();
                }

                var dir = _colorPaletteService.GetDirection(command.NewColorId);
                var selectedSign = grid.GetSignImage(dir);
                selectedSign.PlaySelectedState();
            }
        }
    }
}