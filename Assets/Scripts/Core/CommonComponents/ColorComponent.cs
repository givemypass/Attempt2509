using System;
using SelfishFramework.Src.Core.Components;
using UnityEngine;

namespace Core.CommonComponents
{
    [Serializable]
    public struct ColorComponent : IComponent
    {
        public Color Color;
    }
}