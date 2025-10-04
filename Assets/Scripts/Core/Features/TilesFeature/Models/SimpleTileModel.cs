using SelfishFramework.Src.Features.Features.Serialization;

namespace Core.Features.TilesFeature.Models
{
    [JsonPolyType(typeof(ITileModel), nameof(SimpleTileModel))]
    public class SimpleTileModel : ITileModel
    {
    }
}