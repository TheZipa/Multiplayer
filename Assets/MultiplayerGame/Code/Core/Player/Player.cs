using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.Input;
using UnityEngine;

namespace MultiplayerGame.Code.Core.Player
{
    public class Player : MonoBehaviour, IFactoryEntity
    {
        [SerializeField] private ThirdPersonPlayerCamera _thirdPersonCamera;
        [SerializeField] private PlayerMovement _playerMovement;

        public void Construct(IInputService inputService)
        {
            _thirdPersonCamera.Construct(inputService);
            _playerMovement.Construct(inputService);
            inputService.Enable();
        }
    }
}