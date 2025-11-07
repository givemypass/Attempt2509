using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Core.Features.TilesFeature.Models
{
    //model to avoid using arrays
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorsModel : IEquatable<ColorsModel>
    {
        [JsonProperty("color_1")]
        public int? ColorId1;
        [JsonProperty("color_2")]
        public int? ColorId2;
        [JsonProperty("color_3")]
        public int? ColorId3;

        public bool Equals(ColorsModel other)
        {
            return ColorId1 == other.ColorId1 && ColorId2 == other.ColorId2 && ColorId3 == other.ColorId3;
        }

        public override bool Equals(object obj)
        {
            return obj is ColorsModel other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ColorId1, ColorId2, ColorId3);
        }
    }
}