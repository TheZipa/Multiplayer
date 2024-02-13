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
        private readonly IMultiplayerService _multiplayerService;
        private readonly IUIFactory _uiFactory;
        private readonly ISoundService _soundService;

        public LoadApplicationState(IGameStateMachine gameStateMachine, IStaticData staticData, ISaveLoad saveLoad,
            IMultiplayerService multiplayerService, IUIFactory uiFactory, ISoundService soundService)
        {
            _staticData = staticData;
            _saveLoad = saveLoad;
            _multiplayerService = multiplayerService;
            _uiFactory = uiFactory;
            _soundService = soundService;
            _gameStateMachine = gameStateMachine;
        }
        
        public async void Enter()
        {
            _saveLoad.Load(_staticData.GameConfiguration.StartBalance, _staticData.GameConfiguration.DefaultSoundVolume);
            _soundService.Construct(_saveLoad, _staticData.SoundData);
            await CreatePersistentEntities();
            _multiplayerService.OnConnectingSuccess += EnterToMenu;
            _multiplayerService.Connect();
        }

        public void Exit() => _multiplayerService.OnConnectingSuccess -= EnterToMenu;

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

        private void EnterToMenu() => _gameStateMachine.Enter<MenuState>();
    }
}