using Cysharp.Threading.Tasks;
using MultiplayerGame.Code.Core.UI.Settings;
using MultiplayerGame.Code.Infrastructure.StateMachine.GameStateMachine;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.Factories.GameFactory;
using MultiplayerGame.Code.Services.Factories.UIFactory;
using MultiplayerGame.Code.Services.LoadingCurtain;
using MultiplayerGame.Code.Services.SceneLoader;
using UnityEngine;

namespace MultiplayerGame.Code.Infrastructure.StateMachine.States
{
    public class LoadGameState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IEntityContainer _entityContainer;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private const string GameScene = "Demo_Day";

        public LoadGameState(IGameStateMachine gameStateMachine, IUIFactory uiFactory, IGameFactory gameFactory,
            IEntityContainer entityContainer, ISceneLoader sceneLoader, ILoadingCurtain loadingCurtain)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _gameFactory = gameFactory;
            _entityContainer = entityContainer;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter()
        {
            _loadingCurtain.Show();
            _sceneLoader.LoadScene(GameScene, CreateGame);
        }

        public void Exit()
        {
        }

        private async void CreateGame()
        {
            await InitializeUI();
            await InitializeGameplay();
            FinishLoad();
        }

        private async UniTask InitializeUI()
        {
            await _uiFactory.WarmUpGameplay();
            GameObject rootCanvas = await _uiFactory.CreateRootCanvas();
            await _uiFactory.InstantiateAsRegistered<InGameMenuPanel>(rootCanvas.transform);
        }

        private async UniTask InitializeGameplay()
        {
            await _gameFactory.WarmUp();
            _gameFactory.CreatePlayer();
            await _gameFactory.CreatePlayerCamera();
        }

        private void FinishLoad() => _gameStateMachine.Enter<GameplayState>();
    }
}