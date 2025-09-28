using Core.CommonComponents;
using Core.Features.GameScreenFeature.Mono;
using Core.Models;
using Core.Services;
using DG.Tweening;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Unity;
using UnityEngine;

namespace Core.Features.TilesFeature.Services
{
    [Injectable]
    public partial class SimpleTileFactoryService : ITileFactoryService
    {
        [Inject] private GlobalConfigProvider _globalConfigProvider;
        [Inject] private IColorPaletteService _colorPaletteService;
        
        public Actor GetTile(Vector2 position, Transform parent, Color screenColor, int x, int y)
        {
            var simpleTileActor = _globalConfigProvider.Get.SimpleTilePrefab;
            var tileActor = Object.Instantiate(simpleTileActor, position, Quaternion.identity, parent);
            tileActor.TryInitialize();
            tileActor.transform.localScale = Vector3.zero;
            var color = _colorPaletteService.RandomColorFromCurrentPaletteExcept(screenColor);
            var monoComponent = tileActor.GetComponent<SimpleTileMonoComponent>();
            monoComponent.Image.color = color;
            tileActor.Entity.Set(new GridPositionComponent
            {
                Position = new Vector2Int(x, y),
            });
            tileActor.Entity.Set(new ColorComponent
            {
                Color = color,
            });
            tileActor.transform.DOScale(Vector3.one, 0.2f).SetLink(tileActor.gameObject);
            return tileActor;
        }
    }
}