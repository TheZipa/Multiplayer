using Cinemachine;
using Cysharp.Threading.Tasks;
using MultiplayerGame.Code.Core.Player;
using MultiplayerGame.Code.Services.Assets;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.Input;
using MultiplayerGame.Code.Services.Multiplayer;
using MultiplayerGame.Code.Services.SaveLoad;
using MultiplayerGame.Code.Services.Sound;
using MultiplayerGame.Code.Services.StaticData;
using Photon.Pun;
using UnityEngine;

namespace MultiplayerGame.Code.Services.Factories.GameFactory
{
    public class GameFactory : BaseFactory.BaseFactory, IGameFactory
    {
        private readonly IMultiplayerService _multiplayerService;
        private readonly ISoundService _soundService;
        private readonly IStaticData _staticData;
        private readonly ISaveLoad _saveLoad;
        private readonly IInputService _inputService;

        public GameFactory(IAssets assets, IEntityContainer entityContainer, ISoundService soundService, IMultiplayerService multiplayerService,
            IStaticData staticData, ISaveLoad saveLoad, IInputService inputService) : base(assets, entityContainer)
        {
            _multiplayerService = multiplayerService;
            _soundService = soundService;
            _staticData = staticData;
            _saveLoad = saveLoad;
            _inputService = inputService;
        }

        public async UniTask WarmUp()
        {
            await _assets.Load<GameObject>(nameof(CinemachineVirtualCamera));
        }

        public Player CreatePlayer(Vector3 spawnLocation)
        {
            Player player = PhotonNetwork.Instantiate("Player", 
                GetPlayerSpawnLocationInCircle(spawnLocation), Quaternion.identity)
                .GetComponent<Player>();
            player.Construct(_inputService, _saveLoad.Progress.Nickname);
            player.transform.LookAt(spawnLocation);
            _entityContainer.RegisterEntity(player);
            return player;
        }

        public async UniTask<CinemachineVirtualCamera> CreatePlayerCamera()
        {
            CinemachineVirtualCamera playerCamera = await Instantiate<CinemachineVirtualCamera>();
            playerCamera.Follow = _entityContainer.GetEntity<Player>().PlayerCamera.CinemachineCameraTarget.transform;
            return playerCamera;
        }

        private Vector3 GetPlayerSpawnLocationInCircle(Vector3 spawnLocation)
        {
            float radians = 2 * Mathf.PI / _multiplayerService.GetCurrentPlayerId();
            float vertical = Mathf.Sin(radians);
            float horizontal = Mathf.Cos(radians);
            
            Vector3 spawnDirection = new Vector3(horizontal, 0, vertical);
            return spawnLocation + spawnDirection * _staticData.WorldData.PlayerSpawnRadius;
        }
    }
}