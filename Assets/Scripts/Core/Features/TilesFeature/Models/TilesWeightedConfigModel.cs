using System;
using Newtonsoft.Json;
using SelfishFramework.Src.Features.Features.Serialization;

namespace Core.Features.TilesFeature.Models
{
    [JsonObject]
    [Serializable]
    public class TilesWeightedConfigModel
    {
        [JsonProperty("weight")]
        public int Weight;
        [JsonProperty("tile")]
        [JsonConverter(typeof(EmbeddedTypePolyConverter), typeof(ITileModel))]
        public ITileModel TileModel;
    }
}