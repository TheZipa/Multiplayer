using Cysharp.Threading.Tasks;
using MultiplayerGame.Code.Core.UI;
using MultiplayerGame.Code.Services.Factories.BaseFactory;
using UnityEngine;

namespace MultiplayerGame.Code.Services.Factories.UIFactory
{
    public interface IUIFactory : IBaseFactory
    {
        UniTask<GameObject> CreateRootCanvas();
        UniTask WarmUpMainMenu();
        UniTask<TopPanelView> CreateTopPanel(Transform parent);
        UniTask WarmUpGameplay();
        UniTask WarmUpPersistent();
    }
}