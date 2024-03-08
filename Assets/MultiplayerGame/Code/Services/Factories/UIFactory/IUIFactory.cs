using Cysharp.Threading.Tasks;
using MultiplayerGame.Code.Core.UI;
using MultiplayerGame.Code.Core.UI.MainMenu;
using MultiplayerGame.Code.Core.UI.Rooms;
using MultiplayerGame.Code.Core.UI.Rooms.CreateRoom;
using MultiplayerGame.Code.Core.UI.Settings;
using MultiplayerGame.Code.Services.Factories.BaseFactory;
using UnityEngine;

namespace MultiplayerGame.Code.Services.Factories.UIFactory
{
    public interface IUIFactory : IBaseFactory
    {
        UniTask<GameObject> CreateRootCanvas();
        UniTask WarmUpMainMenu();
        UniTask WarmUpGameplay();
        UniTask WarmUpPersistent();
        UniTask<RoomListScreen> CreateRoomListScreen(Transform root);
        UniTask<MainMenuView> CreateMainMenu(Transform root);
        UniTask<RoomCreateScreen> CreateRoomCreateScreen(Transform root);
        UniTask<RoomScreen> CreateRoomScreen(Transform root);
        UniTask<SettingsPanel> CreateSettingsPanel(Transform root);
    }
}