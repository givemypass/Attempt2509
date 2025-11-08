using Core.Features.GameScreenFeature.Components;
using SelfishFramework.Src.Unity;

namespace Core.Features.TilesFeature.TileWithInner
{
    public partial class ComplexTileActor : Actor
    {
        public TileMonoProviderComponent<ComplexTileMonoComponent> TileMonoProviderComponent;
        public TileCommonComponent TileCommonComponent;

        protected override void BeforeInitialize()
        {
            TileMonoProviderComponent.Get = GetComponent<ComplexTileMonoComponent>();
            base.BeforeInitialize();
        }
    }
}