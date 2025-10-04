using Core.CommonComponents;
using Core.Features.GameScreenFeature.Components;
using Core.Features.GameScreenFeature.Mono;
using Core.Features.StepsFeature;
using Core.Features.SwipeDetection.Commands;
using Core.Services;
using DG.Tweening;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Features.CommonComponents;
using SelfishFramework.Src.Unity;
using UnityEngine;

namespace Core.Features.GameScreenFeature.Systems
{
    [Injectable]
    public sealed partial class ChangeColorWhenSwipeSystem : BaseSystem,
        IReactGlobal<SwipeDetectedCommand>
    {
        [Inject] private IColorPaletteService _colorPaletteService;
        
        private GameScreenMonoComponent _monoComponent;
        private Single<StepsComponent> _stepsSingleComponent;

        public override void InitSystem()
        {
            Owner.AsActor().TryGetComponent(out _monoComponent);
            _stepsSingleComponent = new Single<StepsComponent>(World);
        }

        void IReactGlobal<SwipeDetectedCommand>.ReactGlobal(SwipeDetectedCommand command)
        {
            if (!Owner.Has<WaitForChangingColorComponent>())
            {
                return;
            }
            ref var stepsComponent = ref _stepsSingleComponent.Get();
            stepsComponent.Steps -= 1;
            
            var color = _colorPaletteService.GetColor(command.Direction);
            var currentColor = Owner.Get<ColorComponent>().Color;
            if (color == currentColor)
            {
                return;
            }
            
            Owner.Remove<WaitForChangingColorComponent>();
            Owner.Set(new VisualInProgressComponent());
            Owner.Set(new ColorChangedComponent());
            
            Owner.Set(new ColorComponent
            {
                Color = color,
            });
            var transitionImage = _monoComponent.TransitionImage;
            var transform = (RectTransform)transitionImage.transform;
            var rect = transform.rect;
            transform.anchoredPosition = Vector2.Scale(-1 * command.Direction, new Vector2(rect.size.x, rect.size.y)); 
            transitionImage.transform.gameObject.SetActive(true);
            transitionImage.color = color;
            transitionImage.transform.DOLocalMove(Vector2.zero, 0.5f).SetLink(transform.gameObject).OnComplete(() =>
            {
                _monoComponent.BackgroundImage.color = color;
                transitionImage.transform.gameObject.SetActive(false);
                if (World.IsDisposed(Owner))
                {
                    return;
                }
                Owner.Remove<VisualInProgressComponent>();  
            });
        }
    }
}