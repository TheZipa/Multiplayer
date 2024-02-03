using Cysharp.Threading.Tasks;
using Game.Code.Core.UI;
using Game.Code.Infrastructure.StateMachine.GameStateMachine;
using Game.Code.Services.EntityContainer;
using Game.Code.Services.Factories.GameFactory;
using Game.Code.Services.Factories.UIFactory;
using Game.Code.Services.LoadingCurtain;
using Game.Code.Services.SceneLoader;
using UnityEngine;

namespace Game.Code.Infrastructure.StateMachine.States
{
    public class LoadGameState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IEntityContainer _entityContainer;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private const string GameScene = "Game";

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
            _entityContainer.GetEntity<TopPanelView>().ToggleGameplayStateView();
        }

        private async UniTask InitializeGameplay()
        {
            await _gameFactory.WarmUp();
        }

        private void FinishLoad() => _gameStateMachine.Enter<GameplayState>();
    }
}