using System;
using SelfishFramework.Src.Core.Components;
using UnityEngine;

namespace Core.Features.TilesFeature
{
    [Serializable]
    public struct GridPositionComponent : IComponent
    {
        public Vector2Int Position;
    }
}