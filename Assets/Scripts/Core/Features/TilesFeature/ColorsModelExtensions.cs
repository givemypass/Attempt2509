using System;
using Core.Features.TilesFeature.Models;

namespace Core.Features.TilesFeature
{
    public static class ColorsModelExtensions
    {
        public static unsafe Span<int?> AsSpan(this ref ColorsModel colorsModel)
        {
            fixed (int?* ptr = &colorsModel.ColorId1)
            {
                return new Span<int?>(ptr, 3);
            }
        }

        public static bool ContainsColor(this ColorsModel colorsModel, int colorId)
        {
            foreach (var color in colorsModel.AsSpan())
            {
                if (color == colorId)
                {
                    return true;
                } 
            }
            return false;
        }

        public static int Count(this ColorsModel colorsModel)
        {
            int count = 0;
            foreach (var color in colorsModel.AsSpan())
            {
                if (color.HasValue)
                {
                    count++;
                }
            }
            return count;
        }
    }
}