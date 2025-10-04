using Core.Features.SwipeDetection.Commands;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity.Features.InputFeature.Commands;
using SelfishFramework.Src.Unity.Generated;
using UnityEngine;

namespace Core.CommonSystems
{
    public sealed partial class KeyboardSwipeDetectionSystem : BaseSystem,
        IReactLocal<InputPerformedCommand>
    {
        public override void InitSystem()
        {
        }

        public void ReactLocal(InputPerformedCommand command)
        {
            if (command.Index != InputIdentifierMap.Arrows)
            {
                return;
            }
            
            var direction = command.Context.ReadValue<Vector2>();
            if (direction.x != 0 && direction.y != 0)
            {
                return;
            }
            
            Owner.GetWorld().Command(new SwipeDetectedCommand
            {
                Direction = new Vector2Int(-(int)direction.x, -(int)direction.y),
            });
        }
    }
}