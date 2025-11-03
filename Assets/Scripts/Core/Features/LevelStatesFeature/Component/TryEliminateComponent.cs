using System;
using SelfishFramework.Src.Core.Components;
using UnityEngine;

namespace Core.Features.LevelStatesFeature.Component
{
    [Serializable]
    public struct TryEliminateComponent : IComponent
    {
        public int ColorId;
    }
}