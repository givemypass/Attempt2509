using Core.Features.GameScreenFeature.Components;
using Core.Features.GameScreenFeature.Systems;
using Core.Features.LevelStatesFeature;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity.Features.UI.Actors;

namespace Core.Features.GameScreenFeature
{
    public partial class GameScreenUiActor : UIActor
    {
        public GridMonoProviderComponent GridMonoProviderComponent;
        public LevelsFsmComponent LevelsFsmComponent;
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<LevelsFsmSystem>();
            Entity.AddSystem<ChangeColorWhenSwipeSystem>();
            Entity.AddSystem<GameScreenUISystem>();
        }
    }
}