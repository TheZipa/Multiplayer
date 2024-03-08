using Cysharp.Threading.Tasks;
using MultiplayerGame.Code.Core.UI;
using MultiplayerGame.Code.Infrastructure.StateMachine.GameStateMachine;
using MultiplayerGame.Code.Services.Factories.GameFactory;
using MultiplayerGame.Code.Services.Factories.UIFactory;
using MultiplayerGame.Code.Services.LoadingCurtain;
using MultiplayerGame.Code.Services.Multiplayer;
using MultiplayerGame.Code.Services.SaveLoad;
using MultiplayerGame.Code.Services.Sound;
using MultiplayerGame.Code.Services.StaticData;
using UnityEngine;

namespace MultiplayerGame.Code.Infrastructure.StateMachine.States
{
    public class LoadApplicationState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticData _staticData;
        private readonly ISaveLoad _saveLoad;
        private readonly IUIFactory _uiFactory;
        private readonly ISoundService _soundService;

        public LoadApplicationState(IGameStateMachine gameStateMachine, IStaticData staticData, 
            ISaveLoad saveLoad, IUIFactory uiFactory, ISoundService soundService)
        {
            _staticData = staticData;
            _saveLoad = saveLoad;
            _uiFactory = uiFactory;
            _soundService = soundService;
            _gameStateMachine = gameStateMachine;
        }
        
        public async void Enter()
        {
            _saveLoad.Load(_staticData.GameConfiguration.StartBalance, _staticData.GameConfiguration.DefaultSoundVolume);
            _soundService.Construct(_saveLoad, _staticData.SoundData);
            ApplySavedSettings();
            await CreatePersistentEntities();
            _gameStateMachine.Enter<LoadMenuState>();
        }

        public void Exit()
        {
        }

        private async UniTask CreatePersistentEntities()
        {
            await _uiFactory.WarmUpPersistent();
            GameObject persistentCanvas = await CreatePersistentCanvas();
        }

        private async UniTask<GameObject> CreatePersistentCanvas()
        {
            GameObject persistentCanvas = await _uiFactory.CreateRootCanvas();
            persistentCanvas.GetComponent<Canvas>().sortingOrder = 10;
            persistentCanvas.name = "PersistentCanvas";
            Object.DontDestroyOnLoad(persistentCanvas);
            return persistentCanvas;
        }

        private void ApplySavedSettings()
        {
            QualitySettings.SetQualityLevel(_saveLoad.Progress.Settings.Quality);
            Resolution resolution = Screen.resolutions[_saveLoad.Progress.Settings.Resolution];
            Screen.SetResolution(resolution.width, resolution.height, _saveLoad.Progress.Settings.IsFullscreen);
        }
    }
}