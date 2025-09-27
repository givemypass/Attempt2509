using System;
using Core.Features.GameScreenFeature.Mono;
using SelfishFramework.Src.Core.Components;

namespace Core.Features.GameScreenFeature.Components
{
    [Serializable]
    public struct GridMonoProviderComponent : IComponent
    {
        public GridMonoComponent Grid;
    }
}