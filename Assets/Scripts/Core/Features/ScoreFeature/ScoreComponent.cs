using System;
using SelfishFramework.Src.Core.Components;

namespace Core.Features.ScoreFeature
{
    [Serializable]
    public struct ScoreComponent : IComponent
    {
        public int CurrentScore;
    }
}