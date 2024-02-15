using MultiplayerGame.Code.Core.UI.MainMenu;
using MultiplayerGame.Code.Infrastructure.StateMachine.GameStateMachine;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.LoadingCurtain;
using MultiplayerGame.Code.Services.SaveLoad;
using Photon.Pun;

namespace MultiplayerGame.Code.Infrastructure.StateMachine.States
{
    public class MenuState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISaveLoad _saveLoad;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IEntityContainer _entityContainer;

        private MainMenuView _mainMenuView;

        public MenuState(IGameStateMachine stateMachine, ISaveLoad saveLoad,
            ILoadingCurtain loadingCurtain, IEntityContainer entityContainer)
        {
            _stateMachine = stateMachine;
            _saveLoad = saveLoad;
            _loadingCurtain = loadingCurtain;
            _entityContainer = entityContainer;
        }

        public void Enter()
        {
            CacheEntities();
            Subscribe();
            _loadingCurtain.Hide();
        } 

        public void Exit()
        {
            _mainMenuView.OnPlayClick -= ValidatePlayerNickname;
        }

        private void Subscribe() => _mainMenuView.OnPlayClick += ValidatePlayerNickname;

        private void ValidatePlayerNickname()
        {
            if (!_mainMenuView.ValidatePlayer(out string nickname)) return;
            PhotonNetwork.NickName = _saveLoad.Progress.Nickname = nickname;
            SwitchToRoomList();
        }

        private void CacheEntities() => _mainMenuView = _entityContainer.GetEntity<MainMenuView>();
        
        private void SwitchToRoomList() => _stateMachine.Enter<RoomState>();
    }
}