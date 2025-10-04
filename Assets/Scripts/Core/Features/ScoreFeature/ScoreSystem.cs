using Core.Features.TilesFeature.Commands;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;

namespace Core.Features.ScoreFeature
{
    public sealed partial class ScoreSystem : BaseSystem, IReactGlobal<TileEliminatedCommand>
    {
        public override void InitSystem()
        {
        }

        void IReactGlobal<TileEliminatedCommand>.ReactGlobal(TileEliminatedCommand command)
        {
            ref var scoreComponent = ref Owner.Get<ScoreComponent>();
            scoreComponent.CurrentScore += 1;
        }
    }
}