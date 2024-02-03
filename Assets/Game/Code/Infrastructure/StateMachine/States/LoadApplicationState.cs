using Cysharp.Threading.Tasks;
using Game.Code.Core.UI;
using Game.Code.Infrastructure.StateMachine.GameStateMachine;
using Game.Code.Services.Factories.GameFactory;
using Game.Code.Services.Factories.UIFactory;
using Game.Code.Services.LoadingCurtain;
using Game.Code.Services.SaveLoad;
using Game.Code.Services.Sound;
using Game.Code.Services.StaticData;
using UnityEngine;

namespace Game.Code.Infrastructure.StateMachine.States
{
    public class LoadApplicationState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticData _staticData;
        private readonly ISaveLoad _saveLoad;
        private readonly IGameFactory _gameFactory;
        private readonly IUIFactory _uiFactory;
        private readonly ISoundService _soundService;
        private readonly ILoadingCurtain _loadingCurtain;

        public LoadApplicationState(IGameStateMachine gameStateMachine, IStaticData staticData, ISaveLoad saveLoad,
            IGameFactory gameFactory, IUIFactory uiFactory, ISoundService soundService, ILoadingCurtain loadingCurtain)
        {
            _staticData = staticData;
            _saveLoad = saveLoad;
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
            _soundService = soundService;
            _loadingCurtain = loadingCurtain;
            _gameStateMachine = gameStateMachine;
        }
        
        public async void Enter()
        {
            _saveLoad.Load(_staticData.GameConfiguration.StartBalance, _staticData.GameConfiguration.DefaultSoundVolume);
            _soundService.Construct(_saveLoad, _staticData.SoundData);
            await CreatePersistentEntities();
            _gameStateMachine.Enter<MenuState>();
        }

        public void Exit()
        {
        }

        private async UniTask CreatePersistentEntities()
        {
            await _uiFactory.WarmUpPersistent();
            GameObject persistentCanvas = await CreatePersistentCanvas();
            TopPanelView topPanelView = await _uiFactory.CreateTopPanel(persistentCanvas.transform);
            topPanelView.SetBackAction(() =>
            {
                _loadingCurtain.Show();
                _gameStateMachine.Enter<MenuState>();
            });
        }

        private async UniTask<GameObject> CreatePersistentCanvas()
        {
            GameObject persistentCanvas = await _uiFactory.CreateRootCanvas();
            persistentCanvas.GetComponent<Canvas>().sortingOrder = 10;
            persistentCanvas.name = "PersistentCanvas";
            Object.DontDestroyOnLoad(persistentCanvas);
            return persistentCanvas;
        }
    }
}