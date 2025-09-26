using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.Services
{
    public sealed class SceneService
    {
        private string _loadedScene;

        public async UniTask LoadScene(string sceneName)
        {
            if (_loadedScene != null)
                SceneManager.UnloadSceneAsync(_loadedScene);
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).ToUniTask();
            _loadedScene = sceneName;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_loadedScene));
        } 
    }
}