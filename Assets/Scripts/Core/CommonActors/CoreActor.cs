using Core.CommonSystems;
using Core.Features.SwipeDetection.Components;
using Core.Features.SwipeDetection.Systems;
using Core.Features.TilesFeature.SimpleTile;
using Core.Features.TilesFeature.TileWithInner;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.Features.InputFeature.Components;
using SelfishFramework.Src.Unity.Features.InputFeature.Systems;

namespace Core.CommonActors
{
    public partial class CoreActor : Actor
    {
        public InputActionsComponent InputActionsComponent = new();
        public InputListenerTagComponent InputListenerTagComponent = new();
        public SwipeDetectionComponent SwipeDetectionComponent = new();
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<SwipeDirectionDetectorSystem>();
            Entity.AddSystem<KeyboardSwipeDetectionSystem>();
            Entity.AddSystem<InputListenSystem>();
            Entity.AddSystem<EliminateSimpleTileSystem>();
            Entity.AddSystem<EliminateTileWithInnerSystem>();
        }
    }
}
