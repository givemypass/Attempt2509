using SelfishFramework.Src.Core.CommandBus;
using UnityEngine;

namespace Core.CommonCommands
{
    public struct MainColorChangedCommand : IGlobalCommand
    {
        public Color NewColor;
        public int NewColorId;
    }
}