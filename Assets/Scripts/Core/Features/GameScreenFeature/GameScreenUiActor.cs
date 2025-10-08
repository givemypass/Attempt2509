using Core.Features.GameScreenFeature.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity.Features.UI.Actors;

namespace Core.Features.GameScreenFeature
{
    public partial class GameScreenUiActor : UIActor
    {
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<ChangeColorWhenSwipeSystem>();
            Entity.AddSystem<GameScreenUISystem>();
        }
    }
}