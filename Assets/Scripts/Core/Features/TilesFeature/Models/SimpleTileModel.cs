using Newtonsoft.Json;
using SelfishFramework.Src.Features.Features.Serialization;

namespace Core.Features.TilesFeature.Models
{
    [JsonPolyType(typeof(ITileModel), nameof(SimpleTileModel))]
    public class SimpleTileModel : ITileModel
    {
        [JsonProperty("colors")]
        public ColorsModel Colors;

        public object Clone()
        {
            return new SimpleTileModel
            {
                Colors = Colors,
            };
        }
    }
}