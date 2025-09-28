using Core.Features.GameScreenFeature.Mono;
using Core.Models;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Unity;
using UnityEngine;

namespace Core.Features.TilesFeature.SimpleTile
{
    [Injectable]
    public partial class SimpleTileFactoryService
    {
        [Inject] private GlobalConfigProvider _globalConfigProvider;
        
        public Actor GetTile(Vector2 position, Transform parent, Color color)
        {
            var simpleTileActor = _globalConfigProvider.Get.SimpleTilePrefab;
            var tileActor = Object.Instantiate(simpleTileActor, position, Quaternion.identity, parent);
            tileActor.TryInitialize();
            tileActor.transform.localScale = Vector3.zero;
            var monoComponent = tileActor.GetComponent<SimpleTileMonoComponent>();
            monoComponent.Image.color = color;
            return tileActor;
        }
    }
}