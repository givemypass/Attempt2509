using Core.CommonComponents;
using Core.Models;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Unity;
using UnityEngine;

namespace Core.Features.TilesFeature.TileWithInner
{
    [Injectable]
    public partial class TileWithInnerFactoryService
    {
        [Inject] private GlobalConfigProvider _globalConfigProvider;
        
        public Actor GetTile(Vector2 position, Transform parent, Color color, Actor innerTile)
        {
            var prefab = _globalConfigProvider.Get.TileWithInnerActorPrefab;
            var tileActor = Object.Instantiate(prefab, position, Quaternion.identity, parent);
            tileActor.TryInitialize();
            var monoComponent = tileActor.GetComponent<TileWithSimpleInnerMonoComponent>();
            monoComponent.Image.color = color;
            innerTile.transform.SetParent(monoComponent.InnerParent, false);
            innerTile.transform.localScale = Vector3.one;
            innerTile.transform.localPosition = Vector3.zero;

            tileActor.Entity.Set(new ColorComponent
            {
                Color = color,
            });
            return tileActor;
        }
    }
}