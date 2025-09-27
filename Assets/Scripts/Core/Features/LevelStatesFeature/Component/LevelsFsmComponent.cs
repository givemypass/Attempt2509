using System;
using SelfishFramework.Src.Core.Components;
using SelfishFramework.Src.StateMachine;

namespace Core.Features.LevelStatesFeature
{
    [Serializable]
    public struct LevelsFsmComponent : IComponent
    {
        public StateMachine Fsm;
    }
}