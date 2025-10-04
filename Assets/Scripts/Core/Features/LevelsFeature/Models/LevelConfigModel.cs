using System;
using System.Collections.Generic;
using Core.Features.TilesFeature.Models;
using Newtonsoft.Json;
using SelfishFramework.Src.Features.Features.Serialization;

namespace Core.Features.LevelsFeature.Models
{
    [JsonObject]
    [Serializable]
    public class LevelConfigModel
    {
        [JsonProperty("color_id")]
        public int ColorId;
        [JsonProperty("steps")]
        public int Steps;
        [JsonProperty("tiles")]
        public List<TileConfigModel> Tiles;
    }

    [JsonObject]
    [Serializable]
    public class TileConfigModel
    {
        [JsonProperty("tile")]
        [JsonConverter(typeof(EmbeddedTypePolyConverter), typeof(ITileModel))]
        public ITileModel Tile;
    }
}