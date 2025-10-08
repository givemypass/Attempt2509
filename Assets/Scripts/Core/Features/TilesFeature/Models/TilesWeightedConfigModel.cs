using System;
using Newtonsoft.Json;

namespace Core.Features.TilesFeature.Models
{
    [JsonObject]
    [Serializable]
    public class TilesWeightedConfigModel
    {
        [JsonProperty("weight")]
        public int Weight;
        [JsonProperty("tile_id")] 
        public string TileId;
    }
}