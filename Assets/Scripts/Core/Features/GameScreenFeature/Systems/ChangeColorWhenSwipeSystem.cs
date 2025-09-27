using Core.Features.GameScreenFeature.Mono;
using Core.Features.SwipeDetection.Commands;
using Core.Services;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity;

namespace Core.Features.GameScreenFeature.Systems
{
    [Injectable]
    public sealed partial class ChangeColorWhenSwipeSystem : BaseSystem,
        IReactGlobal<SwipeDetectedCommand>
    {
        [Inject] private ColorPaletteService _colorPaletteService;
        
        private GameScreenMonoComponent _monoComponent;
        
        public override void InitSystem()
        {
            Owner.AsActor().TryGetComponent(out _monoComponent);
        }

        void IReactGlobal<SwipeDetectedCommand>.ReactGlobal(SwipeDetectedCommand command)
        {
            var color = _colorPaletteService.GetColor(command.Direction);
            SLog.Log($"Change color to {color}");
            
            _monoComponent.BackgroundImage.color = color;
        }
    }
}