using System;
using SelfishFramework.Src.Core.Components;

namespace Core.Features.PlayerProgressFeature
{
    [Serializable]
    public struct PlayerProgressComponent : IComponent
    {
        public int CurrentLevel;
    }
}