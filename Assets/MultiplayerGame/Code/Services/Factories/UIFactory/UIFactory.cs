using Cysharp.Threading.Tasks;
using MultiplayerGame.Code.Core.UI;
using MultiplayerGame.Code.Core.UI.MainMenu;
using MultiplayerGame.Code.Services.Assets;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.SaveLoad;
using MultiplayerGame.Code.Services.Sound;
using MultiplayerGame.Code.Services.StaticData;
using UnityEngine;

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
            await _assets.LoadPersistent<GameObject>(nameof(TopPanelView));
        }

        public async UniTask WarmUpMainMenu()
        {
            await _assets.Load<GameObject>(nameof(MainMenuView));
        }

        public async UniTask WarmUpGameplay()
        {
            
        }

        public async UniTask<GameObject> CreateRootCanvas() => await _assets.Instantiate<GameObject>(RootCanvasKey);

        public async UniTask<TopPanelView> CreateTopPanel(Transform parent)
        {
            TopPanelView topPanelView = await InstantiateAsRegistered<TopPanelView>(parent);
            topPanelView.Construct(_soundService);
            return topPanelView;
        }
    }
}