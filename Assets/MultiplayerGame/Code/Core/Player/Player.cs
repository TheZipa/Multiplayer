using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.Input;
using UnityEngine;

namespace MultiplayerGame.Code.Core.Player
{
    public class Player : MonoBehaviour, IFactoryEntity
    {
        public Transform Orientation;
        public Transform PlayerTransform;
        public Transform View;
        public Rigidbody Rigidbody;
        [Space]
        [SerializeField] private PlayerMovement _playerMovement;

        public void Construct(IInputService inputService)
        {
            _playerMovement.Construct(inputService, Orientation, Rigidbody);
            _playerMovement.enabled = true;
            inputService.Enable();
        }
    }
}