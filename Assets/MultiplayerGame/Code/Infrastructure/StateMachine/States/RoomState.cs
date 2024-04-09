using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon;
using MultiplayerGame.Code.Core.UI;
using MultiplayerGame.Code.Core.UI.Rooms;
using MultiplayerGame.Code.Core.UI.Rooms.CreateRoom;
using MultiplayerGame.Code.Data;
using MultiplayerGame.Code.Data.StaticData;
using MultiplayerGame.Code.Infrastructure.StateMachine.GameStateMachine;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.LoadingCurtain;
using MultiplayerGame.Code.Services.Multiplayer;
using MultiplayerGame.Code.Services.StaticData;
using Photon.Pun;
using Photon.Realtime;

namespace MultiplayerGame.Code.Infrastructure.StateMachine.States
{
    public class RoomState : IState
    {
        private const byte StartGameEventCode = 1;
        private readonly IGameStateMachine _stateMachine;
        private readonly IEntityContainer _entityContainer;
        private readonly IMultiplayerService _multiplayerService;
        private readonly IStaticData _staticData;
        private readonly ILoadingCurtain _loadingCurtain;

        private RoomListScreen _roomListScreen;
        private RoomScreen _roomScreen;
        private RoomCreateScreen _roomCreateScreen;
        private MapData _currentMapData;

        public RoomState(IGameStateMachine stateMachine, IEntityContainer entityContainer, IMultiplayerService multiplayerService,
            IStaticData staticData, ILoadingCurtain loadingCurtain)
        {
            _stateMachine = stateMachine;
            _entityContainer = entityContainer;
            _multiplayerService = multiplayerService;
            _staticData = staticData;
            _loadingCurtain = loadingCurtain;
        }
        
        public void Enter()
        {
            CacheEntities();
            Subscribe();
            _roomListScreen.Show();
            _roomListScreen.StartRoomRefreshing();
        }

        public void Exit()
        {
            Unsubscribe();
            _roomListScreen.Hide();
        }

        private void CacheEntities()
        {
            _roomListScreen = _entityContainer.GetEntity<RoomListScreen>();
            _roomScreen = _entityContainer.GetEntity<RoomScreen>();
            _roomCreateScreen = _entityContainer.GetEntity<RoomCreateScreen>();
        }

        private void Subscribe()
        {
            _multiplayerService.OnRoomJoinFailed += DisplayRoomJoinFail;
            _multiplayerService.OnRoomJoined += SwitchToRoomScreen;
            _multiplayerService.OnEventReceived += HandleStartGameEvent;
            _roomScreen.OnRoomLeft += LeaveFromRoom;
            _roomScreen.OnStartGame += StartGame;
            _roomListScreen.OnRoomConnect += JoinToRoom;
            _roomListScreen.OnRoomCreateClick += _roomCreateScreen.Show;
            _roomListScreen.OnRoomListClose += ReturnToMainMenu;
            _roomCreateScreen.OnRoomCreated += CreateNewRoom;
        }
        
        private void Unsubscribe()
        {
            _multiplayerService.OnRoomJoinFailed -= DisplayRoomJoinFail;
            _multiplayerService.OnRoomJoined -= SwitchToRoomScreen;
            _multiplayerService.OnEventReceived -= HandleStartGameEvent;
            _roomScreen.OnRoomLeft -= LeaveFromRoom;
            _roomScreen.OnStartGame -= StartGame;
            _roomListScreen.OnRoomConnect -= JoinToRoom;
            _roomListScreen.OnRoomCreateClick -= _roomCreateScreen.Show;
            _roomListScreen.OnRoomListClose -= ReturnToMainMenu;
            _roomCreateScreen.OnRoomCreated -= CreateNewRoom;
        }

        private async void DisplayRoomJoinFail(string message)
        {
            await UniTask.Delay(1000);
            _loadingCurtain.Hide();
            _entityContainer.GetEntity<ErrorScreen>().ShowError(message);
        }

        private void SwitchToRoomScreen()
        {
            _roomListScreen.StopRoomRefreshing();
            _roomCreateScreen.Hide();
            _roomScreen.SetupRoom();
            _roomScreen.Show();
            _loadingCurtain.Hide();
        }

        private void CreateNewRoom(string roomName, int mapId)
        {
            _loadingCurtain.Show();
            _roomScreen.SetMapData(_currentMapData = _staticData.WorldData.Maps[mapId]);
            _multiplayerService.CreateAndJoinRoom(roomName, mapId, _staticData.GameConfiguration.MaxPlayers, true);
        }

        private void LeaveFromRoom()
        {
            _roomListScreen.StartRoomRefreshing();
            _multiplayerService.LeaveRoom();
        }

        private void StartGame()
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
            _multiplayerService.SendEvent(StartGameEventCode);
            _stateMachine.Enter<LoadGameState, MapData>(_currentMapData);
        }

        private void HandleStartGameEvent(EventData eventData)
        {
            if (eventData.Code != StartGameEventCode) return;
            _stateMachine.Enter<LoadGameState, MapData>(_currentMapData);
        }

        private void JoinToRoom(RoomInfo roomInfo)
        {
            _multiplayerService.JoinToRoom(roomInfo.Name);
            _currentMapData = _staticData.WorldData.Maps[(int)roomInfo.CustomProperties[RoomCustomDataKeys.MapId]];
            _roomScreen.SetMapData(_currentMapData);
            _loadingCurtain.Show();
        }

        private void ReturnToMainMenu() => _stateMachine.Enter<MenuState>();
    }
}