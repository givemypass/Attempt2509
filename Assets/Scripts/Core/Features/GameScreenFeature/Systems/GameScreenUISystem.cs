using Core.Features.GameScreenFeature.Mono;
using Core.Features.StepsFeature;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity;

namespace Core.Features.GameScreenFeature.Systems
{
    public sealed partial class GameScreenUISystem : BaseSystem, IUpdatable
    {
        private Single<StepsComponent> _stepsSingleComponent;
        
        private GameScreenMonoComponent _monoComponent;
        
        private int _prevFrameScore;
        private int _prevFrameSteps;

        public override void InitSystem()
        {
            Owner.AsActor().TryGetComponent(out _monoComponent);
            _stepsSingleComponent = new Single<StepsComponent>(World);
            _monoComponent.StepsText.text = "0";
        }

        public void Update()
        {
            if(!_stepsSingleComponent.Exists())
                return;
            ref var stepsComponent = ref _stepsSingleComponent.Get();
            UpdateSteps(stepsComponent.Steps);
        }

        private void UpdateSteps(int steps)
        {
            if (steps == _prevFrameSteps)
            {
                return;
            }
            
            _prevFrameSteps = steps;
            _monoComponent.StepsText.text = _prevFrameSteps.ToString();
        }
    }
}