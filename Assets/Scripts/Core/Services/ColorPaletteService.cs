using System;
using Core.Models;
using SelfishFramework.Src.Core.Attributes;
using UnityEngine;

namespace Core.Services
{
    public interface IColorPaletteService
    {
        void GeneratePalette();
        ColorPaletteConfig GetCurrentPalette();
        Color GetColor(Vector2Int direction);
        Color RandomColorFromCurrentPaletteExcept(Color? except = null, Color? except2 = null, Color? except3 = null);
        Color GetColor(int id);
    }

    [Injectable]
    public partial class ColorPaletteService : IColorPaletteService
    {
        [Inject] private ColorPaletteConfigProvider _configProvider;
        
        private ColorPaletteConfig _currentPalette;
        
        public void GeneratePalette()
        {
            _currentPalette = _configProvider.GetRandomPalette();
        }
        
        public ColorPaletteConfig GetCurrentPalette() => _currentPalette;
        
        public Color GetColor(Vector2Int direction)
        {
            if (_currentPalette == null)
                GeneratePalette();
        
            return direction switch
            {
                _ when direction == Vector2Int.up => _currentPalette.Color1,
                _ when direction == Vector2Int.right => _currentPalette.Color2,
                _ when direction == Vector2Int.down => _currentPalette.Color3,
                _ when direction == Vector2Int.left => _currentPalette.Color4,
                _ => throw new ArgumentException("Invalid direction"),
            };
        }

        public Color RandomColorFromCurrentPaletteExcept(Color? except = null, Color? except2 = null, Color? except3 = null)
        {
            if (_currentPalette == null)
                GeneratePalette();

            Color color;
            do
            {
                var id = UnityEngine.Random.Range(0, 4);
                color = GetColor(id);

            } while (
                (except != null && color == except.Value) ||
                (except2 != null && color == except2.Value) ||
                (except3 != null && color == except3.Value));

            return color;
        }

        public Color GetColor(int id)
        {
            if (_currentPalette == null)
                GeneratePalette();
        
            return id switch
            {
                0 => _currentPalette.Color1,
                1 => _currentPalette.Color2,
                2 => _currentPalette.Color3,
                3 => _currentPalette.Color4,
                _ => throw new ArgumentException("Invalid color id"),
            };
        }
    }
}