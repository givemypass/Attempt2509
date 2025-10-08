using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Core;
using Core.Features.LevelsFeature.Models;
using Core.Features.TilesFeature.Models;
using Core.Features.TilesFeature.Services;
using Newtonsoft.Json;
using SelfishFramework.Src.Unity.Editor.Helpers;
using TriInspector;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace Editor
{
    [Serializable]
    public class GenerateLevelWindow : SelfishEditorWindow<GenerateLevelWindow.SO>
    {
        [MenuItem("Tools/Level Generator")]
        public static void GetWindow()
        {
            var window = GetWindow<GenerateLevelWindow>("Level Generator");
            window.Show();
            window.Data.Init();
        }

        public class SO : ScriptableObject
        {
            public int TilesCount;
            public float Temperature = 1f;
            public string FileName;
            private TileModelsRandomService _tileRandom;
            private readonly Random _random = new();
            
            public void Init()
            {
                _tileRandom = new TileModelsRandomService();
                _tileRandom.Initialize();
                UpdateFileName();
            }

            [Button]
            private void UpdateFileName()
            {
                FileName = $"level_{DateTime.Now:yyyyMMdd_HHmmss}";
            }

            [Button]
            private void GenerateLevel()
            {
                if (_tileRandom == null)
                {
                    Debug.LogError("Random service not initialized");
                    return;
                }

                if (TilesCount <= 0)
                {
                    Debug.LogError("Set TilesCount > 0");
                    return;
                }

                _tileRandom.Random.Temperature = Temperature;

                var level = new LevelConfigModel
                {
                    ColorId = RandomColorExcept(-1),
                    Tiles = new List<TileConfigModel>(TilesCount),
                };
                for (var i = 0; i < TilesCount; i++)
                {
                    var tile = (ITileModel)_tileRandom.Random.Next().Clone();
                    SetColor(tile, level.ColorId);
                    level.Tiles.Add(new TileConfigModel { Tile = tile });
                }

                var data = MinStepCalculatorUtils.CalculateMinSteps(level);
                level.Steps = data.steps;
                var json = JsonConvert.SerializeObject(level, Formatting.Indented);
                SaveToResources(json, FileName);
            }

            private static void SaveToResources(string data, string fileName)
            {
                var path = Path.Combine(Application.dataPath, "BluePrints", "Levels", fileName + ".json");
                File.WriteAllText(path, data);
                AssetDatabase.Refresh(); 
                Debug.Log($"JSON сохранён: {path}");
            }

            private void SetColor(ITileModel tile, int externalColorId)
            {
                while (true)
                {
                    switch (tile)
                    {
                        case SimpleTileModel simpleTile:
                            simpleTile.ColorId = RandomColorExcept(externalColorId);
                            break;
                        case ComplexTileModel complexTileModel:
                            complexTileModel.ColorId = RandomColorExcept(externalColorId);
                            tile = complexTileModel.SubTile;
                            externalColorId = complexTileModel.ColorId;
                            continue;
                    }

                    break;
                }
            }

            private int RandomColorExcept(int except)
            {
                int color;
                do
                {
                    color = _random.Next(0, 4);
                } while (color == except);

                return color;
            }
        }
    }
}