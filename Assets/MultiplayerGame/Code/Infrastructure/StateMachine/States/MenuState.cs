using System;
using MultiplayerGame.Code.Core.UI.MainMenu;
using MultiplayerGame.Code.Data.StaticData;
using MultiplayerGame.Code.Infrastructure.StateMachine.GameStateMachine;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.LoadingCurtain;
using MultiplayerGame.Code.Services.Multiplayer;
using MultiplayerGame.Code.Services.SaveLoad;
using MultiplayerGame.Code.Services.StaticData;
using Photon.Pun;

namespace MultiplayerGame.Code.Infrastructure.StateMachine.States
{
    public class MenuState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISaveLoad _saveLoad;
        private readonly IMultiplayerService _multiplayerService;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IEntityContainer _entityContainer;
        private readonly IStaticData _staticData;

        private MainMenuView _mainMenuView;

        public MenuState(IGameStateMachine stateMachine, ISaveLoad saveLoad, IMultiplayerService multiplayerService,
            ILoadingCurtain loadingCurtain, IEntityContainer entityContainer, IStaticData staticData)
        {
            _stateMachine = stateMachine;
            _saveLoad = saveLoad;
            _multiplayerService = multiplayerService;
            _loadingCurtain = loadingCurtain;
            _entityContainer = entityContainer;
            _staticData = staticData;
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
            _mainMenuView.OnFreeGameClick -= CreateFreeGameRoom;
            _multiplayerService.OnRoomJoined -= SwitchToFreeGame;
        }

        private void Subscribe()
        {
            _mainMenuView.OnPlayClick += ValidatePlayerNickname;
            _mainMenuView.OnFreeGameClick += CreateFreeGameRoom;
            _multiplayerService.OnRoomJoined += SwitchToFreeGame;
        }

        private void ValidatePlayerNickname()
        {
            if (!_mainMenuView.ValidatePlayer(out string nickname)) return;
            PhotonNetwork.NickName = _saveLoad.Progress.Nickname = nickname;
            SwitchToRoomList();
        }

        private void CacheEntities() => _mainMenuView = _entityContainer.GetEntity<MainMenuView>();
        
        private void SwitchToRoomList() => _stateMachine.Enter<RoomState>();

        private void SwitchToFreeGame() => _stateMachine.Enter<LoadGameState, MapData>(_staticData.WorldData.Maps[0]);

        private void CreateFreeGameRoom()
        {
            _loadingCurtain.Show();
            _multiplayerService.CreateAndJoinRoom(String.Empty, 0, 1, false);
        }
    }
}