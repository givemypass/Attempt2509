using System.Collections.Generic;
using System.Linq;
using Core.Features.LevelsFeature.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Features.LevelsFeature.Services
{
    public interface ILevelsService
    {
        int LevelsCount { get; }
        LevelConfigModel GetLevel(int index);
    }

    public class LevelsService : ILevelsService
    {
        private readonly List<LevelConfigModel> _levels;

        public LevelsService(LevelsConfig config)
        {
            _levels = config.LevelsJson.Select(a => JsonConvert.DeserializeObject<LevelConfigModel>(a.text)).ToList();
        }
        
        public int LevelsCount => _levels.Count;
        
        public LevelConfigModel GetLevel(int index)
        {
            if (index < 0 || index >= _levels.Count)
            {
                Debug.LogError($"Level index {index} is out of range. Total levels: {_levels.Count}");
                return null;
            }
            return _levels[index];
        }
    }
}