using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEditor.Experimental.GraphView;

namespace Core.Features.TilesFeature.Models
{
    //model to avoid using arrays
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorsModel
    {
        [JsonProperty("color_1")]
        public int? ColorId1;
        [JsonProperty("color_2")]
        public int? ColorId2;
        [JsonProperty("color_3")]
        public int? ColorId3;
    }
}