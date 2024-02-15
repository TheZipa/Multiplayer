using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MultiplayerGame.Code.Core.UI.MainMenu;
using MultiplayerGame.Code.Core.UI.Rooms;
using MultiplayerGame.Code.Core.UI.Settings;
using MultiplayerGame.Code.Services.Assets;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.Multiplayer;
using MultiplayerGame.Code.Services.SaveLoad;
using MultiplayerGame.Code.Services.Sound;
using MultiplayerGame.Code.Services.StaticData;
using UnityEngine;
using UnityEngine.Pool;

namespace MultiplayerGame.Code.Services.Factories.UIFactory
{
    public class UIFactory : BaseFactory.BaseFactory, IUIFactory
    {
        private readonly IStaticData _staticData;
        private readonly ISoundService _soundService;
        private readonly ISaveLoad _saveLoad;
        private readonly IMultiplayerService _multiplayerService;
        private const string RootCanvasKey = "RootCanvas";

        public UIFactory(IStaticData staticData, IAssets assets, IEntityContainer entityContainer, 
            ISoundService soundService, ISaveLoad saveLoad, IMultiplayerService multiplayerService)
        : base(assets, entityContainer)
        {
            _staticData = staticData;
            _soundService = soundService;
            _saveLoad = saveLoad;
            _multiplayerService = multiplayerService;
        }

        public async UniTask WarmUpPersistent() => await _assets.LoadPersistent<GameObject>(RootCanvasKey);

        public async UniTask WarmUpMainMenu()
        {
            await _assets.Load<GameObject>(nameof(MainMenuView));
            await _assets.Load<GameObject>(nameof(RoomListScreen));
            await _assets.Load<GameObject>(nameof(RoomConnectField));
            await _assets.Load<GameObject>(nameof(RoomCreateScreen));
            await _assets.Load<GameObject>(nameof(RoomPlayerField));
            await _assets.Load<GameObject>(nameof(RoomScreen));
        }

        public async UniTask WarmUpGameplay()
        {
            await _assets.Load<GameObject>(nameof(InGameMenuPanel));
        }

        public async UniTask<GameObject> CreateRootCanvas() => await _assets.Instantiate<GameObject>(RootCanvasKey);

        public async UniTask<MainMenuView> CreateMainMenu(Transform root)
        {
            MainMenuView mainMenuView = await InstantiateAsRegistered<MainMenuView>(root);
            mainMenuView.SetSavedNickname(_saveLoad.Progress.Nickname);
            return mainMenuView;
        }

        public async UniTask<RoomListScreen> CreateRoomListScreen(Transform root)
        {
            RoomListScreen roomListScreen = await InstantiateAsRegistered<RoomListScreen>(root);
            IObjectPool<RoomConnectField> objectPool = new ObjectPool<RoomConnectField>(() =>
                    Instantiate<RoomConnectField>(roomListScreen.RoomsContent).GetAwaiter().GetResult(), 
                roomField => roomField.Show(), roomField => roomField.Hide());
            roomListScreen.Construct(objectPool);
            return roomListScreen;
        }

        public async UniTask<RoomCreateScreen> CreateRoomCreateScreen(Transform root)
        {
            RoomCreateScreen roomCreateScreen = await InstantiateAsRegistered<RoomCreateScreen>(root);
            return roomCreateScreen;
        }

        public async UniTask<RoomScreen> CreateRoomScreen(Transform root)
        {
            RoomScreen roomScreen = await InstantiateAsRegistered<RoomScreen>(root);
            roomScreen.Construct(_multiplayerService, await CreateRoomPlayerFields(roomScreen.PlayerFieldContent),
                _staticData.GameConfiguration.MaxPlayers, _staticData.GameConfiguration.MinPlayersForStart);
            return roomScreen;
        }

        private async UniTask<Stack<RoomPlayerField>> CreateRoomPlayerFields(Transform content)
        {
            Stack<RoomPlayerField> roomPlayerFields = new Stack<RoomPlayerField>(_staticData.GameConfiguration.MaxPlayers);
            for (int i = 0; i < _staticData.GameConfiguration.MaxPlayers; i++)
            {
                RoomPlayerField roomPlayerField = await Instantiate<RoomPlayerField>(content);
                roomPlayerField.Hide();
                roomPlayerFields.Push(roomPlayerField);
            }
            return roomPlayerFields;
        }
    }
}