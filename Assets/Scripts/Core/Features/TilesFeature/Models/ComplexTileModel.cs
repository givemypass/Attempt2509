using Newtonsoft.Json;
using SelfishFramework.Src.Features.Features.Serialization;

namespace Core.Features.TilesFeature.Models
{
    [JsonPolyType(typeof(ITileModel), nameof(ComplexTileModel))]
    public class ComplexTileModel : ITileModel
    {
        [JsonProperty("sub_tile")]
        [JsonConverter(typeof(EmbeddedTypePolyConverter), typeof(ITileModel))]
        public ITileModel SubTile;
    }
}