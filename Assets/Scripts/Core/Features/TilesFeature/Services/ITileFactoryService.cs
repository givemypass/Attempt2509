using SelfishFramework.Src.Unity;
using UnityEngine;

namespace Core.Features.TilesFeature.Services
{
    public interface ITileFactoryService
    {
        Actor GetTile(Vector2 position, Transform parent, Color screenColor, int x, int y);
    }
}