using System;
using Core.CommonActors;
using Core.Features.GameScreenFeature;
using Core.Features.GameScreenFeature.Mono;
using Core.Features.TilesFeature.TileWithInner;
using SelfishFramework.Src.Unity.Features.TemperatureWeightedRandomFeature;

namespace Core.Models
{
    [Serializable]
    public struct GlobalConfig
    {
        public SimpleTileActor SimpleTilePrefab;
        public ComplexTileActor ComplexTileActorPrefab;
        public LevelActor LevelActor;
    }
}