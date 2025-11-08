using Core.Features.LevelStatesFeature.Component;
using Core.Features.TilesFeature.TileWithInner;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity;

namespace Core.Features.TilesFeature.ComplexTile
{
    public sealed partial class UpdateComplexTileSystem : BaseSystem, IUpdatable
    {
        private Filter _tilesFilter;

        public override void InitSystem()
        {
            _tilesFilter = World.Filter
                .With<ComplexTileActorComponent>()
                .With<GridPositionComponent>()
                .With<UpdateTileComponent>()
                .Build();
        }

        public void Update()
        {
            foreach (var entity in _tilesFilter)
            {
                entity.Remove<UpdateTileComponent>();
                UpdateTile(entity, 0);
            }  
        }

        private int UpdateTile(Entity entity, int depth)
        {
            ref var tileMonoProviderComponent = ref entity.Get<TileMonoProviderComponent<ComplexTileMonoComponent>>();
            var monoComponent = tileMonoProviderComponent.Get;

            //todo take from component
            var innerTile = monoComponent.InnerParent.GetChild(0).GetComponent<Actor>();
            var count = 1;
            if (innerTile.Entity.Has<ComplexTileActorComponent>())
            {
                count += UpdateTile(innerTile.Entity, depth + 1); 
            }
            
            if (depth > 1)
            {
                monoComponent.DeepText.gameObject.SetActive(true);
                monoComponent.DeepText.text = count.ToString();
                monoComponent.InnerParent.gameObject.SetActive(false);
            }
            else
            {
                 monoComponent.DeepText.gameObject.SetActive(false);
                 monoComponent.InnerParent.gameObject.SetActive(true);               
            }
            
            return count;
        }
    }
}