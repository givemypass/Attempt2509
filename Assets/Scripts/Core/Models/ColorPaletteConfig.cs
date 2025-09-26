using System.Collections.Generic;
using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configs/ColorPaletteConfig", fileName = "ColorPaletteConfig", order = 0)]
    public class ColorPaletteConfig : ScriptableObject
    {
        [SerializeField] private ColorPaletteSO[] _palettes;

        public IEnumerable<ColorPaletteSO> Palettes => _palettes;
    }
}