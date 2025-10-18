using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;

namespace Core.Features.HintsFeature
{
    public sealed partial class ShowHintSystem : BaseSystem, IReactGlobal<ShowHintCommand>
    {
        public override void InitSystem()
        {
        }

        void IReactGlobal<ShowHintCommand>.ReactGlobal(ShowHintCommand command)
        {
              
        }
    }
}