using Cysharp.Threading.Tasks;
using MultiplayerGame.Code.Core.UI.Settings;
using MultiplayerGame.Code.Data.StaticData;
using MultiplayerGame.Code.Infrastructure.StateMachine.GameStateMachine;
using MultiplayerGame.Code.Services.Factories.GameFactory;
using MultiplayerGame.Code.Services.Factories.UIFactory;
using MultiplayerGame.Code.Services.LoadingCurtain;
using MultiplayerGame.Code.Services.SceneLoader;
using MultiplayerGame.Code.Services.StaticData;
using UnityEngine;

namespace MultiplayerGame.Code.Infrastructure.StateMachine.States
{
    public class LoadGameState : IPayloadedState<MapData>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IStaticData _staticData;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private MapData _currentMap;

        public LoadGameState(IGameStateMachine gameStateMachine, IUIFactory uiFactory, IGameFactory gameFactory,
            IStaticData staticData, ISceneLoader sceneLoader, ILoadingCurtain loadingCurtain)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _gameFactory = gameFactory;
            _staticData = staticData;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter(MapData mapData)
        {
            _currentMap = mapData;
            _loadingCurtain.Show();
            _sceneLoader.LoadScene(_currentMap.SceneName, CreateGame);
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
            _gameFactory.CreatePlayer(_currentMap.PlayersSpawnLocation);
            await _gameFactory.CreatePlayerCamera();
        }

        private void FinishLoad() => _gameStateMachine.Enter<GameplayState>();
    }
}