using Core.Features.GameScreenFeature;
using Core.Features.GameScreenFeature.Mono;
using Core.Features.StepsFeature;
using DG.Tweening;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Features.GameFSM.Commands;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.Features.UI.Systems;
using SelfishFramework.Src.Unity.Generated;
using Systems;

namespace Core.Features.GameStatesFeature.Systems.States
{
    [Injectable]
    public sealed partial class LevelStateSystem : BaseGameStateSystem, IReactGlobal<StepsRanOutCommand>, IReactGlobal<LevelCompletedCommand>
    {
        [Inject] private IUIService _uiService;
        
        private Filter _filter;

        protected override int State => GameStateIdentifierMap.LevelState;

        public override void InitSystem()
        {
            _filter = World.Filter.With<LevelScreenUiActorComponent>().Build();
        }

        protected override void ProcessState(int from, int to)
        {
        }

        protected override void OnExitState()
        {
            _uiService.CloseUI(UIIdentifierMap.LevelHandlersScreen_UIIdentifier);
            foreach (var entity in _filter)
            {
                var monoComponent = entity.AsActor().GetComponent<LevelScreenMonoComponent>();
                foreach (var grid in monoComponent.Grids)
                {
                    if (!grid.gameObject.activeSelf)
                    {
                        continue; 
                    }

                    foreach (var sign in grid.ColorSigns)
                    {
                        sign.transform.DOScale(0,0.3f).SetEase(Ease.InBack).SetLink(monoComponent.gameObject);
                    }
                }
            }
        }

        public void ReactGlobal(StepsRanOutCommand command)
        {
            if(!IsNeededStates())
            {
                return;
            }
            World.Command(new ForceGameStateTransitionGlobalCommand
            {
                GameState = GameStateIdentifierMap.GameOverState,
            });
        }

        public void ReactGlobal(LevelCompletedCommand command)
        {
            if(!IsNeededStates())
            {
                return;
            }
            World.Command(new ForceGameStateTransitionGlobalCommand
            {
                GameState = GameStateIdentifierMap.LevelCompletedState,
            });
        }
    }
}