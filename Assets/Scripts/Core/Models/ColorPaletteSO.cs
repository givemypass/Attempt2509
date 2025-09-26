using System.Collections.Generic;
using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configs/ColorPalette", fileName = "ColorPaletteSO", order = 0)]
    public class ColorPaletteSO : ScriptableObject
    {
        public Color Color1;
        public Color Color2;
        public Color Color3;
        public Color Color4;

        public IEnumerable<Color> AllColors
        {
            get
            {
                yield return Color1;
                yield return Color2;
                yield return Color3;
                yield return Color4;
            }
        }
    }
}