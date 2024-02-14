using Cysharp.Threading.Tasks;
using MultiplayerGame.Code.Core.UI.MainMenu;
using MultiplayerGame.Code.Core.UI.Rooms;
using MultiplayerGame.Code.Services.Assets;
using MultiplayerGame.Code.Services.EntityContainer;
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
        private const string RootCanvasKey = "RootCanvas";

        public UIFactory(IStaticData staticData, IAssets assets, IEntityContainer entityContainer, 
            ISoundService soundService, ISaveLoad saveLoad)
        : base(assets, entityContainer)
        {
            _staticData = staticData;
            _soundService = soundService;
            _saveLoad = saveLoad;
        }

        public async UniTask WarmUpPersistent()
        {
            await _assets.LoadPersistent<GameObject>(RootCanvasKey);
        }

        public async UniTask WarmUpMainMenu()
        {
            await _assets.Load<GameObject>(nameof(MainMenuView));
            await _assets.Load<GameObject>(nameof(RoomListScreen));
            await _assets.Load<GameObject>(nameof(RoomConnectField));
        }

        public async UniTask WarmUpGameplay()
        {
            
        }

        public async UniTask<GameObject> CreateRootCanvas() => await _assets.Instantiate<GameObject>(RootCanvasKey);

        public async UniTask<RoomListScreen> CreateRoomListScreen(Transform root)
        {
            RoomListScreen roomListScreen = await InstantiateAsRegistered<RoomListScreen>(root);
            IObjectPool<RoomConnectField> objectPool = new ObjectPool<RoomConnectField>(() =>
                    Instantiate<RoomConnectField>(roomListScreen.RoomsContent).GetAwaiter().GetResult(), 
                roomField => roomField.Show(), roomField => roomField.Hide());
            roomListScreen.Construct(objectPool);
            return roomListScreen;
        }
    }
}