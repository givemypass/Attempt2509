using System;
using Core.Features.TilesFeature.Models;
using SelfishFramework.Src.Core.Components;

namespace Core.Features.GameScreenFeature.Components
{
    [Serializable]
    public struct TileCommonComponent : IComponent
    {
        public ITileModel Model;
    }
}