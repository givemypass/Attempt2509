using System.Collections.Generic;
using Core.CommonComponents;
using Core.Features.GameScreenFeature.Mono;
using Core.Features.TilesFeature.Models;
using Core.Features.TilesFeature.TileWithInner;
using Core.Models;
using Core.Services;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Unity;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Features.TilesFeature.Services
{
    public interface ITileFactoryService
    {
        Actor GetTile(ITileModel model, Vector2 position, Transform parent, Color exceptColor);
        Actor GetTile(ITileModel model, Vector2 position, Transform parent);
    }

    [Injectable]
    public partial class TileFactoryService : ITileFactoryService
    {
        [Inject] private IColorPaletteService _colorPaletteService;
        [Inject] private GlobalConfigProvider _globalConfigProvider;

        private delegate Actor GetTileDelegate(ITileModel model, Vector2 position, Transform parent, Color? overrideColor);
        
        private readonly Dictionary<string, GetTileDelegate> _tileFactories;

        public TileFactoryService()
        {
            _tileFactories = new Dictionary<string, GetTileDelegate>
            {
                { nameof(SimpleTileModel), CreateSimple },
                { nameof(ComplexTileModel), CreateComplex },
            }; 
        }

        public Actor GetTile(ITileModel model, Vector2 position, Transform parent, Color exceptColor)
        {
            var factory = _tileFactories[model.GetType().Name];
            var color = _colorPaletteService.RandomColorFromCurrentPaletteExcept(exceptColor);
            return factory(model, position, parent, color);
        }
        public Actor GetTile(ITileModel model, Vector2 position, Transform parent)
        {
            var factory = _tileFactories[model.GetType().Name];
            return factory(model, position, parent, null);
        }
        
        private Actor CreateSimple(ITileModel model, Vector2 position, Transform parent, Color? overrideColor)
        {
            var simpleModel = (SimpleTileModel)model;
            
            var prefab = _globalConfigProvider.Get.SimpleTilePrefab;
            var tileActor = Object.Instantiate(prefab, position, Quaternion.identity, parent);
            tileActor.TryInitialize();
            tileActor.transform.localScale = Vector3.zero;
            var monoComponent = tileActor.GetComponent<SimpleTileMonoComponent>();
            var color = overrideColor ?? _colorPaletteService.GetColor(simpleModel.ColorId);
            monoComponent.Image.color = color;
            tileActor.Entity.Set(new ColorComponent
            {
                Color = color,
            });
            return tileActor;
        }

        private Actor CreateComplex(ITileModel model, Vector2 position, Transform parent, Color? overrideColor)
        {
            var complexModel = (ComplexTileModel)model;
            
            var prefab = _globalConfigProvider.Get.ComplexTileActorPrefab;
            var tileActor = Object.Instantiate(prefab, position, Quaternion.identity, parent);
            tileActor.TryInitialize();
            var monoComponent = tileActor.GetComponent<ComplexTileMonoComponent>();
            var color = overrideColor ?? _colorPaletteService.GetColor(complexModel.ColorId);
            monoComponent.Image.color = color;
            
            var subTile = overrideColor == null ?
                GetTile(complexModel.SubTile, position, monoComponent.InnerParent) : 
                GetTile(complexModel.SubTile, position, monoComponent.InnerParent, color);
            subTile.transform.localScale = Vector3.one;
            subTile.transform.localPosition = Vector3.zero;
            tileActor.Entity.Set(new ColorComponent
            {
                Color = color,
            });
            return tileActor;
        }
    }
}