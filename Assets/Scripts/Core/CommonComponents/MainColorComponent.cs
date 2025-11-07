using System;
using SelfishFramework.Src.Core.Components;
using UnityEngine;

namespace Core.CommonComponents
{
    [Serializable]
    public struct MainColorComponent : IComponent
    {
        public Color Color;
    }
}