using Core.CommonComponents;
using Core.Features.GameScreenFeature.Components;
using Core.Features.GameScreenFeature.Mono;
using Core.Features.SwipeDetection.Commands;
using Core.Services;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;
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
            if (!Owner.Has<WaitForChangingColorComponent>())
            {
                return;
            }
            
            Owner.Remove<WaitForChangingColorComponent>();
            var color = _colorPaletteService.GetColor(command.Direction);
            Owner.Set(new ColorComponent
            {
                Color = color,
            });
            _monoComponent.BackgroundImage.color = color;
        }
    }
}