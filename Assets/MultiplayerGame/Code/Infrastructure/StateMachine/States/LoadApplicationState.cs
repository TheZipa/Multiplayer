using MultiplayerGame.Code.Infrastructure.StateMachine.GameStateMachine;
using MultiplayerGame.Code.Services.Factories.UIFactory;
using MultiplayerGame.Code.Services.Quality;
using MultiplayerGame.Code.Services.SaveLoad;
using MultiplayerGame.Code.Services.StaticData;

namespace MultiplayerGame.Code.Infrastructure.StateMachine.States
{
    public class LoadApplicationState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticData _staticData;
        private readonly ISaveLoad _saveLoad;
        private readonly IUIFactory _uiFactory;
        private readonly IQualityService _qualityService;

        public LoadApplicationState(IGameStateMachine gameStateMachine, IStaticData staticData, 
            ISaveLoad saveLoad, IUIFactory uiFactory, IQualityService qualityService)
        {
            _staticData = staticData;
            _saveLoad = saveLoad;
            _uiFactory = uiFactory;
            _qualityService = qualityService;
            _gameStateMachine = gameStateMachine;
        }
        
        public async void Enter()
        {
            _saveLoad.Load(_staticData.GameConfiguration.StartBalance, _staticData.GameConfiguration.DefaultSoundVolume);
            _qualityService.ApplySavedSettings();
            await _uiFactory.WarmUpPersistent();
            _gameStateMachine.Enter<LoadMenuState>();
        }

        public void Exit()
        {
        }
    }
}