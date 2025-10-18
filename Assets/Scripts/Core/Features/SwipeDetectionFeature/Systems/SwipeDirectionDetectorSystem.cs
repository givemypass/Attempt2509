using Core.Features.SwipeDetection.Commands;
using Core.Features.SwipeDetection.Components;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity.Features.InputFeature.Commands;
using SelfishFramework.Src.Unity.Generated;
using Unity.Mathematics;
using UnityEngine;

namespace Core.Features.SwipeDetection.Systems
{
    public sealed partial class SwipeDirectionDetectorSystem :
        BaseSystem,
        IReactLocal<InputStartedCommand>,
        IReactLocal<InputEndedCommand>,
        IReactLocal<InputPerformedCommand>
    {
        public override void InitSystem()
        {
        }

        public void ReactLocal(InputStartedCommand command)
        {
            if (command.Index != InputIdentifierMap.Touch)
            {
                return;
            }

            ref var swipeDetectionComponent = ref Owner.Get<SwipeDetectionComponent>();
            swipeDetectionComponent.StartPosition = command.Context.ReadValue<Vector2>();
            swipeDetectionComponent.Detecting = true;
        }

        public void ReactLocal(InputEndedCommand command)
        {
            if (command.Index != InputIdentifierMap.Touch)
            {
                return;
            }

            ref var component = ref Owner.Get<SwipeDetectionComponent>();
            component.Detecting = false;
            var minSwipeDistance = component.MinSwipeDistance;
            var dif = component.Position - component.StartPosition;
            var dpi = Screen.dpi;
            if(dpi == 0) dpi = 96; // default dpi
            
            Vector2Int direction;
            if (math.abs(dif.x) / dpi > minSwipeDistance || math.abs(dif.y) / dpi > minSwipeDistance)
            {
                var normalized = dif.normalized;
                if (math.abs(normalized.x) > math.abs(normalized.y))
                {
                    direction = normalized.x > 0 ? new Vector2Int(1, 0) : new Vector2Int(-1, 0);
                }
                else
                {
                    direction = normalized.y > 0 ? new Vector2Int(0, 1) : new Vector2Int(0, -1);
                }
            }
            else
            {
                return;
            }
            
            Owner.GetWorld().Command(new SwipeDetectedCommand
            {
                Direction = direction,
            });
            SLog.Log($"swipe detected {direction.x} {direction.y}");
        }

        public void ReactLocal(InputPerformedCommand command)
        {
            if (command.Index != InputIdentifierMap.Touch)
            {
                return;
            }
            var pos = command.Context.ReadValue<Vector2>();
            SLog.Log($"touch pos {pos}");

            ref var swipeDetectionComponent = ref Owner.Get<SwipeDetectionComponent>();
            if (!swipeDetectionComponent.Detecting)
            {
                return;
            }

            swipeDetectionComponent.Position = pos;
        }
    }
}