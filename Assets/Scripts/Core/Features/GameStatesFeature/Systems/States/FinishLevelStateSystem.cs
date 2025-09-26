using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Unity.Generated;
using Systems;

namespace Core.Features.GameStatesFeature.Systems.States
{
    [Injectable]
    public sealed partial class FinishLevelStateSystem : BaseGameStateSystem
    {
        
        private Filter _levelFilter;

        protected override int State => GameStateIdentifierMap.FinishLevelState;

        public override void InitSystem()
        {
        }

        protected override void ProcessState(int from, int to)
        {
        }
    }
}