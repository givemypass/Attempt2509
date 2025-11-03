using Core.CommonComponents;
using Core.Features.GameScreenFeature;
using Core.Features.GameScreenFeature.Components;
using Core.Features.LevelStatesFeature.Component;
using Core.Features.StepsFeature;
using Core.Features.TilesFeature;
using Core.Services;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Features.CommonComponents;
using SelfishFramework.Src.StateMachine;
using SelfishFramework.Src.Unity.Generated;

namespace Core.Features.LevelStatesFeature.States
{
    [Injectable]
    public partial class EliminateTilesState : BaseFSMState
    {
        [Inject] private IColorPaletteService _colorPaletteService;
        
        private readonly Filter _tileFilter;
        private readonly Filter _screenFilter;
        private readonly Single<StepsComponent> _stepsSingleComponent;
        public override int StateID => LevelStateIdentifierMap.EliminateTilesState;

        public EliminateTilesState(StateMachine stateMachine) : base(stateMachine)
        {
            var world = stateMachine.World;
            _tileFilter = world.Filter.With<TileCommonComponent>().With<GridPositionComponent>().Build();
            _screenFilter = stateMachine.World.Filter 
                .With<LevelScreenUiActorComponent>()
                .With<GridMonoProviderComponent>()
                .With<ColorComponent>()
                .Build();
        }

        public override void Enter(Entity entity)
        {
            foreach (var screenEntity in _screenFilter)
            {
                var currentColor = screenEntity.Get<ColorComponent>().Color;
                var currentColorId = _colorPaletteService.GetColorId(currentColor);

                foreach (var tileEntity in _tileFilter)
                {
                    tileEntity.Set(new TryEliminateComponent { ColorId =  currentColorId});
                }

                break;
            }
        }

        public override void Exit(Entity entity)
        {
            
        }

        public override void Update(Entity entity)
        {
            foreach (var tileEntity in _tileFilter)
            {
                if (tileEntity.Has<TryEliminateComponent>() ||
                    tileEntity.Has<VisualInProgressComponent>())
                {
                    return;
                }
            }
            EndState();
        }
    }
}