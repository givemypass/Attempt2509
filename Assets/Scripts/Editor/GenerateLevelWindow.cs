using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            public string[] TileConfigs;
            private readonly Random _random = new();
            
            private TileModelsService _tileModelsService;
            
            public void Init()
            {
                _tileModelsService = new TileModelsService();
                _tileModelsService.Initialize();
            }

            [Button]
            private void GenerateLevel()
            {
                var level = new LevelConfigModel
                {
                    ColorId = RandomColorExcept(-1),
                    Tiles = new List<TileModelConfigWrapper>(),
                };
                foreach (var tileConfig in TileConfigs)
                {
                    var args = tileConfig.Split("__");
                    int.TryParse(args[0], out var count);
                    for (int i = 0; i < count; i++)
                    {
                        var tile = (ITileModel)_tileModelsService.Models[args[1]].Clone();
                        SetColor(tile, level.ColorId);
                        level.Tiles.Add(new TileModelConfigWrapper{ Tile = tile}); 
                    }
                }

                var data = MinStepCalculatorUtils.CalculateMinSteps(level);
                level.Steps = data.steps;
                var json = JsonConvert.SerializeObject(level, Formatting.Indented);
                var fileName = new StringBuilder("level");
                foreach (var tileConfig in TileConfigs)
                {
                    var args = tileConfig.Split("__");
                    fileName.Append("__").Append(args[0]).Append("_").Append(args[1]); 
                }

                SaveToResources(json, fileName.ToString());
            }

            private static void SaveToResources(string data, string fileName)
            {
                var path = Path.Combine(Application.dataPath, "BluePrints", "Levels", fileName + ".json");
                var i = 1;
                while (File.Exists(path))
                {
                    i++;
                    path = Path.Combine(Application.dataPath, "BluePrints", "Levels", fileName + $"({i})" + ".json");
                }
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