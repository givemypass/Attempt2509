using System;
using SelfishFramework.Src.Core.Components;
using UnityEngine;

namespace Core.Features.SwipeDetection.Components
{
    [Serializable]
    public struct SwipeDetectionComponent : IComponent
    {
        public float MinSwipeDistance;
        [HideInInspector]
        public bool Detecting;
        [HideInInspector]
        public Vector2 StartPosition;
        [HideInInspector]
        public Vector2 Position;
    }
}