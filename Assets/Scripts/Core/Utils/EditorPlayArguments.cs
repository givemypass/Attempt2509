#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.Utils
{
#if UNITY_EDITOR
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
#endif
}