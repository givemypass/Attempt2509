using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.Services
{
    public interface ISceneService
    {
        UniTask LoadSceneAsync(string sceneName);
    }

    public sealed class SceneService : ISceneService
    {
        private string _loadedScene;

        public async UniTask LoadSceneAsync(string sceneName)
        {
            if (_loadedScene != null)
                SceneManager.UnloadSceneAsync(_loadedScene);
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).ToUniTask();
            _loadedScene = sceneName;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_loadedScene));
        } 
    }
}