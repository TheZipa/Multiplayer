using Cysharp.Threading.Tasks;
using Game.Code.Core.UI;
using Game.Code.Core.UI.MainMenu;
using Game.Code.Infrastructure.StateMachine.GameStateMachine;
using Game.Code.Services.EntityContainer;
using Game.Code.Services.Factories.UIFactory;
using Game.Code.Services.LoadingCurtain;
using Game.Code.Services.SceneLoader;
using UnityEngine;

namespace Game.Code.Infrastructure.StateMachine.States
{
    public class MenuState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly ISceneLoader _sceneLoader;
        private readonly IEntityContainer _entityContainer;
        private readonly ILoadingCurtain _loadingCurtain;

        private MainMenuView _mainMenuView;
        private TopPanelView _topPanelView;
        private const string MenuScene = "Menu";

        public MenuState(IGameStateMachine stateMachine, IUIFactory uiFactory,
            ISceneLoader sceneLoader, IEntityContainer entityContainer, ILoadingCurtain loadingCurtain)
        {
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
            _entityContainer = entityContainer;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter() => _sceneLoader.LoadScene(MenuScene, PrepareMenu);

        public void Exit()
        {
            _mainMenuView.OnPlayClick -= LoadGame;
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
            _topPanelView = _entityContainer.GetEntity<TopPanelView>();
            _topPanelView.ToggleMainMenuStateView();
            _mainMenuView =  await _uiFactory.InstantiateAsRegistered<MainMenuView>(rootCanvas.transform);
        }

        private void Subscribe()
        {
            _mainMenuView.OnPlayClick += LoadGame;
        }

        private void LoadGame() => _stateMachine.Enter<LoadGameState>();
    }
}