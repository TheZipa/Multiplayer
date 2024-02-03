using System;

namespace Game.Code.Services.SceneLoader
{
    public interface ISceneLoader
    {
        void LoadScene(string sceneName, Action onLoaded = null);
    }
}