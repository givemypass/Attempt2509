using System;
using System.IO;
using Core.Utils;
using Newtonsoft.Json;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Features.Features.Serialization;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity.CommonCommands;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.Features.PlayerProgressFeature
{
    public sealed partial class SaveLoadPlayerProgressSystem : BaseSystem, IReactGlobal<LoadProgressCommand>, IReactGlobal<SaveCommand>
    {
        public override void InitSystem() { }

        void IReactGlobal<SaveCommand>.ReactGlobal(SaveCommand command) => Save();
        void IReactGlobal<LoadProgressCommand>.ReactGlobal(LoadProgressCommand command) => Load();

        private void Save()
        {
#if UNITY_EDITOR
            if (EditorPlayArguments.IsTestMode())
            {
                SLog.Log("Test mode active, skipping save.");
                return;
            }
#endif
            ref var playerProgress = ref Owner.Get<PlayerProgressComponent>(); 
            var saveModel = new SaveModel
            {
                LevelIndex = playerProgress.CurrentLevel,
            };
            var json = JsonConvert.SerializeObject(saveModel);
            JsonHelper.SaveJson(SavePath(), json);
            SLog.Log("Game saved successfully.");
        }

        private void Load()
        {
            int levelIndex = 0;
            if (JsonHelper.TryLoadJson(SavePath(), out var json))
            {
                var saveModel = JsonConvert.DeserializeObject<SaveModel>(json);
                levelIndex = saveModel.LevelIndex;
                SLog.Log("Game loaded successfully.");
            }
            else
            {
                levelIndex = 0;
                SLog.Log("No save data found.");
            }
#if UNITY_EDITOR
            if (EditorPlayArguments.IsTestMode())
            {
                levelIndex = 0;
            }
#endif
            Owner.Set(new PlayerProgressComponent
            {
                CurrentLevel = levelIndex,
            });
        }

        private static string SavePath()
        {
            return Path.Combine(Application.persistentDataPath, "save.json");
        }
        
        [JsonObject]
        [Serializable]
        private struct SaveModel
        {
            [JsonProperty("LevelIndex")]
            public int LevelIndex;
        }

    }
}