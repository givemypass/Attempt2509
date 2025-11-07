using Core.CommonComponents;
using Core.Features.GameScreenFeature.Commands;
using Core.Features.GameScreenFeature.Components;
using Core.Features.StepsFeature;
using Core.Features.SwipeDetection.Commands;
using Core.Services;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;

namespace Core.Features.GameScreenFeature.Systems
{
    [Injectable]
    public sealed partial class ChangeColorWhenSwipeSystem : BaseSystem,
        IReactGlobal<SwipeDetectedCommand>
    {
        [Inject] private IColorPaletteService _colorPaletteService;
        
        private Single<StepsComponent> _stepsSingleComponent;

        public override void InitSystem()
        {
            _stepsSingleComponent = new Single<StepsComponent>(World);
        }

        void IReactGlobal<SwipeDetectedCommand>.ReactGlobal(SwipeDetectedCommand command)
        {
            if (!Owner.Has<WaitForChangingColorComponent>())
            {
                return;
            }
            Owner.Remove<WaitForChangingColorComponent>();
            
            var color = _colorPaletteService.GetColor(command.Direction);
            var currentColor = Owner.Get<MainColorComponent>().Color;
            if (color == currentColor)
            {
                return;
            }
            
            ref var stepsComponent = ref _stepsSingleComponent.Get();
            stepsComponent.Steps -= 1;           
            Owner.Command(new ChangeColorCommand
            {
                Color = color,
                Direction = command.Direction,
            });
        }
    }
}