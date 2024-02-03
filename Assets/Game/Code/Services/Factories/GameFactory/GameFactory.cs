using Cysharp.Threading.Tasks;
using Game.Code.Services.Assets;
using Game.Code.Services.EntityContainer;
using Game.Code.Services.SaveLoad;
using Game.Code.Services.Sound;
using Game.Code.Services.StaticData;

namespace Game.Code.Services.Factories.GameFactory
{
    public class GameFactory : BaseFactory.BaseFactory, IGameFactory
    {
        private readonly ISoundService _soundService;
        private readonly IStaticData _staticData;
        private readonly ISaveLoad _saveLoad;

        public GameFactory(IAssets assets, IEntityContainer entityContainer, ISoundService soundService, 
            IStaticData staticData, ISaveLoad saveLoad) : base(assets, entityContainer)
        {
            _soundService = soundService;
            _staticData = staticData;
            _saveLoad = saveLoad;
        }

        public async UniTask WarmUp()
        {
            
        }
    }
}