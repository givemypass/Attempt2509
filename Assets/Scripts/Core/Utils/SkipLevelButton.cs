#if UNITY_EDITOR
using Core.Features.StepsFeature;
using SelfishFramework.Src.Core;
using UnityEditor;

namespace Core.Utils
{
    public static class SkipLevelButton
    {
        [MenuItem("Colors/Skip Level")]
        public static void SkipLevel()
        {
            var world = SManager.World;
            world.Command(new LevelCompletedCommand());
        }
    }
}
#endif