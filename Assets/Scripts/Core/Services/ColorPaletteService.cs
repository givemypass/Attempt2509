using System;
using Core.Features.SwipeDetection.Commands;
using Core.Models;
using SelfishFramework.Src.Core.Attributes;
using UnityEngine;

namespace Core.Services
{
    [Injectable]
    public partial class ColorPaletteService
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
    }
}