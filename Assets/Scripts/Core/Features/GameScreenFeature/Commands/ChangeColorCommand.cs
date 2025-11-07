using SelfishFramework.Src.Core.CommandBus;
using UnityEngine;

namespace Core.Features.GameScreenFeature.Commands
{
    public struct ChangeColorCommand : ICommand
    {
        public Color Color;
        public Vector2Int Direction;
    }
}