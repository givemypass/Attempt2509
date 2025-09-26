using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Unity.Generated;
using Systems;

namespace Core.Features.GameStatesFeature.Systems.States
{
    [Injectable]
    public sealed partial class LevelStateSystem : BaseGameStateSystem
    {
        protected override int State => GameStateIdentifierMap.LevelState;

        public override void InitSystem()
        {
        }

        protected override void ProcessState(int from, int to)
        {
        }

        protected override void OnExitState()
        {
        }
    }
}