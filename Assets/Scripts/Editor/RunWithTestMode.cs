using UnityEditor;

namespace Core.Utils
{
    [InitializeOnLoad]
    public static class RunWithTestMode
    {
        [MenuItem("Colors/RunWithTestMode")]
        public static void Run()
        {
            EditorPlayArguments.SetTestMode(true);
            EditorApplication.isPlaying = true;
        }
        
        static RunWithTestMode()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.EnteredEditMode)
                {
                    EditorPlayArguments.SetTestMode(false);
                }
            };
        }
    }
}