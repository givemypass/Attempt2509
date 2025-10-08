using Core.Features.TilesFeature.Models;
using Newtonsoft.Json;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity.Features.TemperatureWeightedRandomFeature;
using UnityEngine;

namespace Core.Features.TilesFeature.Services
{
    [Injectable]
    public partial class TileModelsRandomService
    {
        [Inject] private TileModelsService _tileModelsService;
        
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
                if (!_tileModelsService.Models.TryGetValue(models[i].TileId, out var tileModel))
                {
                    SLog.LogError($"Tile ID {models[i].TileId} not found in TileModelsService.");
                    continue;
                }
                weightedItems[i] = new WeightedItem<ITileModel>((ITileModel)tileModel.Clone(), models[i].Weight);
            }
            Random = new TemperatureWeightedRandom<ITileModel>(weightedItems);
        }
    }
}