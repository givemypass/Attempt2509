using System;
using Core.Features.GameScreenFeature.Mono;
using SelfishFramework.Src.Core.Components;
using UnityEngine;

namespace Core.Features.GameScreenFeature.Components
{
    [Serializable]
    public struct GridMonoProviderComponent : IComponent
    {
        [HideInInspector]
        public GridMonoComponent Grid;
    }
}