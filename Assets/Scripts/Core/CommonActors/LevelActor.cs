using Core.Features.LevelStatesFeature;
using Core.Features.ScoreFeature;
using Core.Features.StepsFeature;
using Core.Features.TilesFeature;
using Core.Features.TilesFeature.SimpleTile;
using Core.Features.TilesFeature.TileWithInner;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;

namespace Core.CommonActors
{
    public partial class LevelActor : Actor
    {
        public ScoreComponent ScoreComponent = new();
        public StepsComponent StepsComponent = new();
        public LevelsFsmComponent LevelsFsmComponent = new();
            
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<LevelsFsmSystem>();
            Entity.AddSystem<TryEliminateTileSystem>();
            Entity.AddSystem<EliminateSimpleTileSystem>();
            Entity.AddSystem<EliminateComplexTileSystem>();
            Entity.AddSystem<ScoreSystem>();
            Entity.AddSystem<StepsSystem>();
        }
    }
}