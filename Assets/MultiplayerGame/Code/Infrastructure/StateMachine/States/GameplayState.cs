using MultiplayerGame.Code.Core.UI.Settings;
using MultiplayerGame.Code.Extensions;
using MultiplayerGame.Code.Infrastructure.StateMachine.GameStateMachine;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.Input;
using MultiplayerGame.Code.Services.LoadingCurtain;
using Photon.Pun;

namespace MultiplayerGame.Code.Infrastructure.StateMachine.States
{
    public class GameplayState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IEntityContainer _entityContainer;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IInputService _inputService;

        private InGameMenuPanel _inGameMenuPanel;

        public GameplayState(IGameStateMachine gameStateMachine, IEntityContainer entityContainer, 
            ILoadingCurtain loadingCurtain, IInputService inputService)
        {
            _gameStateMachine = gameStateMachine;
            _entityContainer = entityContainer;
            _loadingCurtain = loadingCurtain;
            _inputService = inputService;
        }

        public void Enter()
        {
            _loadingCurtain.Hide();
            _inGameMenuPanel = _entityContainer.GetEntity<InGameMenuPanel>();
            _inGameMenuPanel.OnReturnToMainMenu += ReturnToMainMenu;
            _inputService.OnEscape += _inGameMenuPanel.ToggleEnabled;
            GameExtensions.DisableCursor();
        }

        public void Exit()
        {
            _inputService.OnEscape -= _inGameMenuPanel.ToggleEnabled;
            _inGameMenuPanel.OnReturnToMainMenu -= ReturnToMainMenu;
            PhotonNetwork.Disconnect();
        }

        private void ReturnToMainMenu()
        {
            _loadingCurtain.Show();
            _gameStateMachine.Enter<LoadMenuState>();
        }
    }
}