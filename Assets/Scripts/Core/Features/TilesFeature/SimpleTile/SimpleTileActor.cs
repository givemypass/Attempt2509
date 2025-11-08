using Core.Features.GameScreenFeature.Components;
using Core.Features.GameScreenFeature.Mono;
using Core.Features.TilesFeature;
using SelfishFramework.Src.Unity;

namespace Core.Features.GameScreenFeature
{
    public partial class SimpleTileActor : Actor
    {
        public TileMonoProviderComponent<SimpleTileMonoComponent> TileMonoProviderComponent;
        public TileCommonComponent TileCommonComponent;
        
        protected override void BeforeInitialize()
        {
            TileMonoProviderComponent.Get = GetComponent<SimpleTileMonoComponent>();
            base.BeforeInitialize();
        }
    }
}