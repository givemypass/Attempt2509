using Core.Features.TilesFeature.Models;
using Newtonsoft.Json;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity.Features.TemperatureWeightedRandomFeature;
using UnityEngine;

namespace Core.Features.TilesFeature.Services
{
    public class TileModelsRandomService
    {
        public TemperatureWeightedRandom<ITileModel> Random { get; private set; }

        public void Initialize()
        {
            var textAsset = Resources.Load<TextAsset>("weightedTiles");
            if (textAsset == null || string.IsNullOrEmpty(textAsset.text))
            {
                SLog.LogError("Failed to load weightedTiles from Resources.");
                return;
            }
            var models = JsonConvert.DeserializeObject<TilesWeightedConfigModel[]>(textAsset.text);
            var weightedItems = new WeightedItem<ITileModel>[models.Length];
            for (var i = 0; i < models.Length; i++)
            {
                weightedItems[i] = new WeightedItem<ITileModel>(models[i].TileModel, models[i].Weight);
            }
            Random = new TemperatureWeightedRandom<ITileModel>(weightedItems);
        }
    }
}