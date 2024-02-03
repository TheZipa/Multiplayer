using Cysharp.Threading.Tasks;
using Game.Code.Core.UI;
using Game.Code.Services.Factories.BaseFactory;
using UnityEngine;

namespace Game.Code.Services.Factories.UIFactory
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