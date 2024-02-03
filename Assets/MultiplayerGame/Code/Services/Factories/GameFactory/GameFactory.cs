using Cysharp.Threading.Tasks;
using MultiplayerGame.Code.Services.Assets;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.SaveLoad;
using MultiplayerGame.Code.Services.Sound;
using MultiplayerGame.Code.Services.StaticData;

namespace MultiplayerGame.Code.Services.Factories.GameFactory
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