using System;
using Core.Models;
using SelfishFramework.Src.Core.Attributes;
using UnityEngine;

namespace Core.Services
{
    public interface IColorPaletteService
    {
        void GeneratePalette();
        Color[] GetCurrentPalette();
        Color GetColor(Vector2Int direction);
        Color RandomColorFromCurrentPaletteExcept(Color? except = null, Color? except2 = null, Color? except3 = null);
        Color GetColor(int id);
        Vector2Int GetDirection(int colorId);
    }

    [Injectable]
    public partial class ColorPaletteService : IColorPaletteService
    {
        [Inject] private ColorPaletteConfigProvider _configProvider;
        
        private Color[] _currentPalette;
        
        public void GeneratePalette()
        {
            var palette = _configProvider.GetRandomPalette();
            var i = 0;
            _currentPalette ??= new Color[4];
            foreach (var color in palette.AllColors)
            {
                _currentPalette[i++] = color;
            }
        }
        
        public Color[] GetCurrentPalette() => _currentPalette;
        
        public Color GetColor(Vector2Int direction)
        {
            return direction switch
            {
                _ when direction == Vector2Int.up => GetColor(0),
                _ when direction == Vector2Int.right => GetColor(1),
                _ when direction == Vector2Int.down => GetColor(2),
                _ when direction == Vector2Int.left => GetColor(3),
                _ => throw new ArgumentException("Invalid direction"),
            };
        }

        public Vector2Int GetDirection(int colorId)
        {
            return colorId switch
            {
                0 => Vector2Int.up,
                1 => Vector2Int.right,
                2 => Vector2Int.down,
                3 => Vector2Int.left,
                _ => throw new ArgumentOutOfRangeException(nameof(colorId), "Color ID is out of range."),
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
            
            if (id < 0 || id >= _currentPalette!.Length)
                throw new ArgumentOutOfRangeException(nameof(id), "Color ID is out of range.");

            return _currentPalette![id];
        }
    }
}