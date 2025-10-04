using Newtonsoft.Json;
using SelfishFramework.Src.Features.Features.Serialization;
using UnityEngine;

namespace Core.Features.TilesFeature.Models
{
    [JsonPolyType(typeof(ITileModel), nameof(SimpleTileModel))]
    public class SimpleTileModel : ITileModel
    {
        [JsonProperty("color_id")]
        public int ColorId;
    }
}