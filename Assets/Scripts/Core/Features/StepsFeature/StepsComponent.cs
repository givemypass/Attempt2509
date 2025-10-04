using System;
using SelfishFramework.Src.Core.Components;

namespace Core.Features.StepsFeature
{
    [Serializable]
    public struct StepsComponent : IComponent
    {
        public int Steps;
    }
}