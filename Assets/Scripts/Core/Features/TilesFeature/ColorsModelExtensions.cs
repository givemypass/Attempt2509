using Core.Features.TilesFeature.Models;

namespace Core.Features.TilesFeature
{
    public static class ColorsModelExtensions
    {
        public static bool ContainsColor(this ColorsModel colorsModel, int colorId)
        {
            return colorsModel.ColorId1 == colorId ||
                   colorsModel.ColorId2 == colorId ||
                   colorsModel.ColorId3 == colorId ||
                   colorsModel.ColorId4 == colorId;
        }
    }
}