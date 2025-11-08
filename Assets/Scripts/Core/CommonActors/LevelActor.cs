using Core.Features.LevelStatesFeature;
using Core.Features.StepsFeature;
using Core.Features.TilesFeature;
using Core.Features.TilesFeature.ComplexTile;
using Core.Features.TilesFeature.SimpleTile;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;

namespace Core.CommonActors
{
    public partial class LevelActor : Actor
    {
        public StepsComponent StepsComponent = new();
        public LevelsFsmComponent LevelsFsmComponent = new();
            
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<LevelsFsmSystem>();
        }
    }
}