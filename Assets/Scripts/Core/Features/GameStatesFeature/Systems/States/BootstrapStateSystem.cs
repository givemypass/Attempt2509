using Core.Features.TilesFeature.Models;
using Core.Features.TilesFeature.Services;
using Newtonsoft.Json;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Features.Features.Serialization;
using SelfishFramework.Src.Unity.CommonCommands;
using SelfishFramework.Src.Unity.Generated;
using Systems;
using UnityEngine;

namespace Core.Features.GameStatesFeature.Systems.States
{
    [Injectable]
    public sealed partial class BootstrapStateSystem : BaseGameStateSystem
    {
        [Inject] private TileModelsRandomService _tileModelsRandomService;
        
        protected override int State => GameStateIdentifierMap.BootstrapState;

        public override void InitSystem()
        {
        }

        protected override void ProcessState(int from, int to)
        {
            Application.targetFrameRate = 60;
            World.Command(new LoadProgressCommand());
            JsonPolyTypeCache.Prewarm();
            _tileModelsRandomService.Initialize();
            EndState();
        }
    }
}