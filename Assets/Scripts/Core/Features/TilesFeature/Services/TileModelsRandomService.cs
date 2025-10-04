using Core.Features.TilesFeature.Models;
using SelfishFramework.Src.Unity.Features.TemperatureWeightedRandomFeature;

namespace Core.Features.TilesFeature.Services
{
    public class TileModelsRandomService
    {
        public TemperatureWeightedRandom<ITileModel> Random { get; private set; }

        public void Initialize(TilesWeightedConfigModel[] models)
        {
            var weightedItems = new WeightedItem<ITileModel>[models.Length];
            for (var i = 0; i < models.Length; i++)
            {
                weightedItems[i] = new WeightedItem<ITileModel>(models[i].TileModel, models[i].Weight);
            }
            Random = new TemperatureWeightedRandom<ITileModel>(weightedItems);
        }
    }
}