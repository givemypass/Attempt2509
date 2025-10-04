using Core.Features.GameStatesFeature.Systems;
using Core.Features.GameStatesFeature.Systems.States;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Features.GameFSM.Components;
using SelfishFramework.Src.Unity;

namespace Core.Features.GameStatesFeature
{
    public partial class GameStatesActor : Actor
    {
        public GameStateComponent GameStateComponent = new();
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<GameStateMachineSystem>();
            Entity.AddSystem<BootstrapStateSystem>();
            Entity.AddSystem<BootstrapLevelStateSystem>();
            Entity.AddSystem<LevelStateSystem>();
            Entity.AddSystem<GameOverStateSystem>();
        }
    }
}