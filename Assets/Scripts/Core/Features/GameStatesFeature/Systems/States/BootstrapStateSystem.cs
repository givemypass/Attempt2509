using SelfishFramework.Src.Unity.CommonCommands;
using SelfishFramework.Src.Unity.Generated;
using Systems;
using UnityEngine;

namespace Core.Features.GameStatesFeature.Systems.States
{
    public sealed partial class BootstrapStateSystem : BaseGameStateSystem
    {
        protected override int State => GameStateIdentifierMap.BootstrapState;

        public override void InitSystem()
        {
        }

        protected override void ProcessState(int from, int to)
        {
            Application.targetFrameRate = 60;
            World.Command(new LoadProgressCommand());
            EndState();
        }
    }
}