using Core.CommonComponents;
using Core.Features.GameScreenFeature.Mono;
using Core.Features.ScoreFeature;
using Core.Features.StepsFeature;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity;

namespace Core.Features.GameScreenFeature.Systems
{
    public sealed partial class GameScreenUISystem : BaseSystem, IUpdatable
    {
        private Single<ScoreComponent> _scoreSingleComponent;
        private Single<StepsComponent> _stepsSingleComponent;
        
        private GameScreenMonoComponent _monoComponent;
        
        private int _prevFrameScore;
        private int _prevFrameSteps;

        public override void InitSystem()
        {
            Owner.AsActor().TryGetComponent(out _monoComponent);
            _scoreSingleComponent = new Single<ScoreComponent>(World);
            _stepsSingleComponent = new Single<StepsComponent>(World);
            _monoComponent.ScoreText.text = "0";
            _monoComponent.StepsText.text = "0";
        }

        public void Update()
        {
            ref var scoreComponent = ref _scoreSingleComponent.Get();
            UpdateScore(scoreComponent.CurrentScore);
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
        
        private void UpdateScore(int score)
        {
            if (score == _prevFrameScore)
            {
                return;
            }
            
            _prevFrameScore = score;
            _monoComponent.ScoreText.text = _prevFrameScore.ToString();
        }
    }
}