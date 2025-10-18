using Core.Features.GameScreenFeature.Systems;
using Core.Features.HintsFeature;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity.Features.UI.Actors;

namespace Core.Features.GameScreenFeature
{
    public partial class LevelScreenUiActor : UIActor
    {
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<ChangeColorWhenSwipeSystem>();
            Entity.AddSystem<ShowHintSystem>();
            Entity.AddSystem<GameScreenUISystem>();
        }
    }
}