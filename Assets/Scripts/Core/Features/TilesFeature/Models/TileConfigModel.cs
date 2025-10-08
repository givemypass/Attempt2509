using System;
using Newtonsoft.Json;
using SelfishFramework.Src.Features.Features.Serialization;

namespace Core.Features.TilesFeature.Models
{
    [JsonObject]
    [Serializable]
    public class TileConfigModel
    {
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("tile")]
        [JsonConverter(typeof(EmbeddedTypePolyConverter), typeof(ITileModel))]
        public ITileModel TileModel;
    }
}