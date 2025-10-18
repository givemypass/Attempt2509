using System;
using Core.Features.TilesFeature.Models;
using Newtonsoft.Json;
using SelfishFramework.Src.Features.Features.Serialization;

namespace Core.Features.LevelsFeature.Models
{
    [JsonObject]
    [Serializable]
    public class TileModelConfigWrapper
    {
        [JsonProperty("tile")]
        [JsonConverter(typeof(EmbeddedTypePolyConverter), typeof(ITileModel))]
        public ITileModel Tile;
    }
}