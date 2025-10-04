using Core.Features.TilesFeature.Commands;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;

namespace Core.Features.StepsFeature
{
    public sealed partial class StepsSystem : BaseSystem, IReactGlobal<TileEliminatedCommand>
    {
        public override void InitSystem()
        {
        }

        void IReactGlobal<TileEliminatedCommand>.ReactGlobal(TileEliminatedCommand command)
        {
            ref var stepsComponent = ref Owner.Get<StepsComponent>();
            stepsComponent.Steps += 1;
        }
    }
}