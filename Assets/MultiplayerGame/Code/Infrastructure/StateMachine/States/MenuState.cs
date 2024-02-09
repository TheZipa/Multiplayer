using Cysharp.Threading.Tasks;
using MultiplayerGame.Code.Core.UI.MainMenu;
using MultiplayerGame.Code.Infrastructure.StateMachine.GameStateMachine;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.Factories.UIFactory;
using MultiplayerGame.Code.Services.LoadingCurtain;
using MultiplayerGame.Code.Services.Multiplayer;
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
        private readonly IEntityContainer _entityContainer;
        private readonly ILoadingCurtain _loadingCurtain;

        private MainMenuView _mainMenuView;
        private const string MenuScene = "Menu";

        public MenuState(IGameStateMachine stateMachine, IUIFactory uiFactory, IMultiplayerService _multiplayerService,
            ISceneLoader sceneLoader, IEntityContainer entityContainer, ILoadingCurtain loadingCurtain)
        {
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
            this._multiplayerService = _multiplayerService;
            _sceneLoader = sceneLoader;
            _entityContainer = entityContainer;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter() => _sceneLoader.LoadScene(MenuScene, PrepareMenu);

        public void Exit()
        {
            _mainMenuView.OnPlayClick -= TryShowRooms;
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
            _mainMenuView =  await _uiFactory.InstantiateAsRegistered<MainMenuView>(rootCanvas.transform);
        }

        private void TryShowRooms()
        {
            if (!_mainMenuView.ValidatePlayer()) return;
            Debug.Log("Rooms panel show");
        }

        private void Subscribe()
        {
            _mainMenuView.OnPlayClick += TryShowRooms;
        }

        private void LoadGame() => _stateMachine.Enter<LoadGameState>();
    }
}