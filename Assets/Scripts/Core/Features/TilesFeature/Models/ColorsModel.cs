using System;
using Newtonsoft.Json;

namespace Core.Features.TilesFeature.Models
{
    //model to avoid using arrays
    [Serializable]
    public struct ColorsModel
    {
        [JsonProperty("color_1")]
        public int? ColorId1;
        [JsonProperty("color_2")]
        public int? ColorId2;
        [JsonProperty("color_3")]
        public int? ColorId3;
        [JsonProperty("color_4")]
        public int? ColorId4;
    }
}