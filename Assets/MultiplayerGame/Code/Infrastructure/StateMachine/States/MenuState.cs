using System;
using MultiplayerGame.Code.Core.UI.MainMenu;
using MultiplayerGame.Code.Core.UI.Settings;
using MultiplayerGame.Code.Data.StaticData;
using MultiplayerGame.Code.Infrastructure.StateMachine.GameStateMachine;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.LoadingCurtain;
using MultiplayerGame.Code.Services.Multiplayer;
using MultiplayerGame.Code.Services.SaveLoad;
using MultiplayerGame.Code.Services.StaticData;

namespace MultiplayerGame.Code.Infrastructure.StateMachine.States
{
    public class MenuState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISaveLoad _saveLoad;
        private readonly IMultiplayerRooms _multiplayerRooms;
        private readonly IMultiplayerCommon _multiplayerCommon;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IEntityContainer _entityContainer;
        private readonly IStaticData _staticData;

        private MainMenuView _mainMenuView;
        private SettingsPanel _settingsPanel;

        public MenuState(IGameStateMachine stateMachine, ISaveLoad saveLoad, IMultiplayerRooms multiplayerRooms,
            IMultiplayerCommon multiplayerCommon, ILoadingCurtain loadingCurtain, IEntityContainer entityContainer, IStaticData staticData)
        {
            _saveLoad = saveLoad;
            _stateMachine = stateMachine;
            _multiplayerRooms = multiplayerRooms;
            _multiplayerCommon = multiplayerCommon;
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
            _mainMenuView.OnPlayClick -= DefinePlayerAndShowRooms;
            _mainMenuView.OnFreeGameClick -= CreateFreeGameRoom;
            _mainMenuView.OnSettingsClick -= _settingsPanel.Show;
            _multiplayerRooms.OnRoomJoined -= SwitchToFreeGame;
        }

        private void Subscribe()
        {
            _mainMenuView.OnPlayClick += DefinePlayerAndShowRooms;
            _mainMenuView.OnFreeGameClick += CreateFreeGameRoom;
            _mainMenuView.OnSettingsClick += _settingsPanel.Show;
            _multiplayerRooms.OnRoomJoined += SwitchToFreeGame;
        }

        private void DefinePlayerAndShowRooms()
        {
            if (!_mainMenuView.TryGetNickname(out string nickname)) return;
            _saveLoad.Progress.Nickname = nickname;
            _multiplayerCommon.SetNickname(nickname);
            SwitchToRoomList();
        }

        private void CacheEntities()
        {
            _mainMenuView = _entityContainer.GetEntity<MainMenuView>();
            _settingsPanel = _entityContainer.GetEntity<SettingsPanel>();
        }

        private void SwitchToRoomList() => _stateMachine.Enter<RoomState>();

        private void SwitchToFreeGame() => _stateMachine.Enter<LoadGameState, MapData>(_staticData.WorldData.GetRandomMapData());

        private void CreateFreeGameRoom()
        {
            _loadingCurtain.Show();
            _multiplayerRooms.CreateAndJoinRoom(String.Empty, 0, 1, false);
        }
    }
}