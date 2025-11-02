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

        [JsonProperty("color_id")]
        public int ColorId;

        public object Clone()
        {
            return new ComplexTileModel
            {
                ColorId = ColorId,
                SubTile = (ITileModel)SubTile.Clone(),
            };
        }
    }
}