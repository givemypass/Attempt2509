using Core.CommonCommands;
using Core.CommonComponents;
using Core.Features.GameScreenFeature.Commands;
using Core.Features.GameScreenFeature.Mono;
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
    public sealed partial class ChangeColorSystem : BaseSystem, IReactLocal<ChangeColorCommand>
    {
        [Inject] private IColorPaletteService _colorPaletteService;
        
        private BackColorScreenMonoComponent _monoComponent;
        
        public override void InitSystem()
        {
            Owner.AsActor().TryGetComponent(out _monoComponent);
        }

        public void ReactLocal(ChangeColorCommand command)
        {
            var color = command.Color;
            Owner.Set(new VisualInProgressComponent());
            World.Command(new MainColorChangedCommand
            {
                NewColor = color,
                NewColorId = _colorPaletteService.GetColorId(color),
            });
            Owner.Set(new MainColorComponent
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