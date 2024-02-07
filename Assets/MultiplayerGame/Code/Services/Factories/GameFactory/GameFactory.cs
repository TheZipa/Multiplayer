using Cysharp.Threading.Tasks;
using MultiplayerGame.Code.Core.Player;
using MultiplayerGame.Code.Services.Assets;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.Input;
using MultiplayerGame.Code.Services.SaveLoad;
using MultiplayerGame.Code.Services.Sound;
using MultiplayerGame.Code.Services.StaticData;
using Photon.Pun;
using UnityEngine;

namespace MultiplayerGame.Code.Services.Factories.GameFactory
{
    public class GameFactory : BaseFactory.BaseFactory, IGameFactory
    {
        private readonly ISoundService _soundService;
        private readonly IStaticData _staticData;
        private readonly ISaveLoad _saveLoad;
        private readonly IInputService _inputService;

        public GameFactory(IAssets assets, IEntityContainer entityContainer, ISoundService soundService, 
            IStaticData staticData, ISaveLoad saveLoad, IInputService inputService) : base(assets, entityContainer)
        {
            _soundService = soundService;
            _staticData = staticData;
            _saveLoad = saveLoad;
            _inputService = inputService;
        }

        public async UniTask WarmUp()
        {
            await _assets.Load<GameObject>(nameof(ThirdPersonPlayerCamera));
        }

        public Player CreatePlayer()
        {
            Player player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity).GetComponent<Player>();
            player.Construct(_inputService);
            _entityContainer.RegisterEntity(player);
            return player;
        }

        public async UniTask<ThirdPersonPlayerCamera> CreatePlayerCamera()
        {
            ThirdPersonPlayerCamera playerCamera = await Instantiate<ThirdPersonPlayerCamera>();
            Player player = _entityContainer.GetEntity<Player>();
            playerCamera.Construct(_inputService, player.Orientation, player.PlayerTransform, player.View);
            return playerCamera;
        }
    }
}