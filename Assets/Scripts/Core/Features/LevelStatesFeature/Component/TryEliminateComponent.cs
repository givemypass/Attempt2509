using System;
using SelfishFramework.Src.Core.Components;

namespace Core.Features.LevelStatesFeature.Component
{
    [Serializable]
    public struct TryEliminateComponent : IComponent
    {
        public int ColorId;
    }
}