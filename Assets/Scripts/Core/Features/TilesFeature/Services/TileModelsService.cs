using System.Collections.Generic;
using Core.Features.TilesFeature.Models;
using Newtonsoft.Json;
using SelfishFramework.Src.SLogs;
using UnityEngine;

namespace Core.Features.TilesFeature.Services
{
    public class TileModelsService
    {
        public Dictionary<string, ITileModel> Models { get; private set; }
        
        public void Initialize()
        {
            var textAsset = Resources.Load<TextAsset>("tilesConfig");
            if (textAsset == null || string.IsNullOrEmpty(textAsset.text))
            {
                SLog.LogError("Failed to load tilesConfig from Resources.");
                return;
            }
            var models = JsonConvert.DeserializeObject<TileConfigModel[]>(textAsset.text);
            Models = new Dictionary<string, ITileModel>(models.Length);
            foreach (var config in models)
            {
                Models[config.Id] = config.TileModel;
            }
        }
    }
}