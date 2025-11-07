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
        private readonly Single<MainColorComponent> _mainColorSingle;
        public override int StateID => LevelStateIdentifierMap.EliminateTilesState;

        public EliminateTilesState(StateMachine stateMachine) : base(stateMachine)
        {
            var world = stateMachine.World;
            _tileFilter = world.Filter.With<TileCommonComponent>().With<GridPositionComponent>().Build();
            _mainColorSingle = new Single<MainColorComponent>(world);
        }

        public override void Enter(Entity fsmEntity)
        {
            _mainColorSingle.ForceUpdate();
            var currentColor = _mainColorSingle.Get().Color;
            var currentColorId = _colorPaletteService.GetColorId(currentColor);

            foreach (var tileEntity in _tileFilter)
            {
                tileEntity.Set(new TryEliminateComponent { ColorId =  currentColorId});
            }
        }

        public override void Exit(Entity fsmEntity)
        {
            
        }

        public override void Update(Entity fsmEntity)
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