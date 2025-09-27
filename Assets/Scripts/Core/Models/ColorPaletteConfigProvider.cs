using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configs/ColorPaletteConfigProvider", fileName = "ColorPaletteConfigProvider", order = 0)]
    public class ColorPaletteConfigProvider : ScriptableObject
    {
        [SerializeField] private ColorPaletteConfig[] _palettes;
        
        public IEnumerable<ColorPaletteConfig> Palettes => _palettes;
        
        public ColorPaletteConfig GetRandomPalette()
        {
            if (_palettes.Length == 0)
                throw new Exception("No color palettes available in the provider.");
            
            var randomIndex = UnityEngine.Random.Range(0, _palettes.Length);
            return _palettes[randomIndex];
        }
    }
}