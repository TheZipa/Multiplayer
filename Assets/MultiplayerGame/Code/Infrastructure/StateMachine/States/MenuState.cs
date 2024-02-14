using Cysharp.Threading.Tasks;
using MultiplayerGame.Code.Core.UI.MainMenu;
using MultiplayerGame.Code.Core.UI.Rooms;
using MultiplayerGame.Code.Infrastructure.StateMachine.GameStateMachine;
using MultiplayerGame.Code.Services.Factories.UIFactory;
using MultiplayerGame.Code.Services.LoadingCurtain;
using MultiplayerGame.Code.Services.Multiplayer;
using MultiplayerGame.Code.Services.SaveLoad;
using MultiplayerGame.Code.Services.SceneLoader;
using UnityEngine;

namespace MultiplayerGame.Code.Infrastructure.StateMachine.States
{
    public class MenuState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IMultiplayerService _multiplayerService;
        private readonly ISceneLoader _sceneLoader;
        private readonly ISaveLoad _saveLoad;
        private readonly ILoadingCurtain _loadingCurtain;

        private MainMenuView _mainMenuView;
        private RoomListScreen _roomListScreen;
        private const string MenuScene = "Menu";

        public MenuState(IGameStateMachine stateMachine, IUIFactory uiFactory, IMultiplayerService _multiplayerService,
            ISceneLoader sceneLoader, ISaveLoad saveLoad, ILoadingCurtain loadingCurtain)
        {
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
            this._multiplayerService = _multiplayerService;
            _sceneLoader = sceneLoader;
            _saveLoad = saveLoad;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter() => _sceneLoader.LoadScene(MenuScene, PrepareMenu);

        public void Exit()
        {
            _mainMenuView.OnPlayClick -= ValidatePlayerNickname;
            _multiplayerService.OnRoomsUpdated -= _roomListScreen.RefreshRoomList;
        }

        private async void PrepareMenu()
        {
            await CreateMenuElements();
            Subscribe();
            _loadingCurtain.Hide();
        }

        private async UniTask CreateMenuElements()
        {
            await _uiFactory.WarmUpMainMenu();
            GameObject rootCanvas = await _uiFactory.CreateRootCanvas();
            _mainMenuView = await _uiFactory.InstantiateAsRegistered<MainMenuView>(rootCanvas.transform);
            _mainMenuView.SetSavedNickname(_saveLoad.Progress.Nickname);
            _roomListScreen = await _uiFactory.CreateRoomListScreen(rootCanvas.transform);
        }

        private void ValidatePlayerNickname()
        {
            if (!_mainMenuView.ValidatePlayer(out string nickname)) return;
            _saveLoad.Progress.Nickname = nickname;
            _roomListScreen.Show();
            LoadGame();
        }

        private void Subscribe()
        {
            _mainMenuView.OnPlayClick += ValidatePlayerNickname;
            _multiplayerService.OnRoomsUpdated += _roomListScreen.RefreshRoomList;
        }

        private void LoadGame() => _stateMachine.Enter<LoadGameState>();
    }
}