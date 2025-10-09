using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
        public List<TileModelConfigWrapper> Tiles;
    }
}