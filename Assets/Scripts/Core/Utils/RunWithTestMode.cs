using UnityEditor;

namespace Core.Utils
{
    public static class EditorPlayArguments
    {
        private const string TEST_MODE_KEY = "TEST_MODE";

        public static void SetTestMode(bool status)
        {
            if (status)
            {
                EditorPrefs.SetBool(TEST_MODE_KEY, true);
            }
            else
            {
                EditorPrefs.DeleteKey(TEST_MODE_KEY);
            }
        }
        
        public static bool IsTestMode()
        {
            return EditorPrefs.GetBool(TEST_MODE_KEY, false);
        }
    }
    
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
                if (state == PlayModeStateChange.ExitingPlayMode)
                {
                    EditorPlayArguments.SetTestMode(false);
                }
            };
        }
    }
}