using System;
using MultiplayerGame.Code.Services.Assets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiplayerGame.Code.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        private readonly IAssets _assets;

        public SceneLoader(IAssets assets) => _assets = assets;

        public void LoadScene(string sceneName, Action onLoaded = null)
        {
            AsyncOperation loadSceneAsyncOperation = SceneManager.LoadSceneAsync(sceneName);
            loadSceneAsyncOperation.completed += operation =>
            {
                _assets.CleanUp();
                onLoaded?.Invoke();
            };
        }
    }
}