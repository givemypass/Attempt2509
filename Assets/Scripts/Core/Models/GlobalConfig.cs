using System;
using Core.Features.GameScreenFeature;
using Core.Features.GameScreenFeature.Mono;
using Core.Features.TilesFeature.TileWithInner;

namespace Core.Models
{
    [Serializable]
    public struct GlobalConfig
    {
        public SimpleTileActor SimpleTilePrefab;
        public TileWithInnerActor TileWithInnerActorPrefab;
    }
}