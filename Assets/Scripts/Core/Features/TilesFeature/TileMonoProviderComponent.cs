using System;
using SelfishFramework.Src.Core.Components;
using UnityEngine;

namespace Core.Features.TilesFeature
{
    [Serializable]
    public struct TileMonoProviderComponent<T> : IComponent where T : MonoBehaviour
    {
        public T Get;
    }
}