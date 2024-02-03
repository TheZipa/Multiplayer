using Cysharp.Threading.Tasks;
using Game.Code.Core.UI;
using Game.Code.Core.UI.MainMenu;
using Game.Code.Services.Assets;
using Game.Code.Services.EntityContainer;
using Game.Code.Services.SaveLoad;
using Game.Code.Services.Sound;
using Game.Code.Services.StaticData;
using UnityEngine;

namespace Game.Code.Services.Factories.UIFactory
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