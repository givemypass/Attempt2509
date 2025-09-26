using SelfishFramework.Src.Core.CommandBus;
using UnityEngine;

namespace Core.Features.SwipeDetection.Commands
{
    public struct SwipeDetectedCommand : IGlobalCommand
    {
        public Vector2Int Direction;
    }
}