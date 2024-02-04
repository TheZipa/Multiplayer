using MultiplayerGame.Code.Services.Input;
using Sirenix.Utilities;
using UnityEngine;

namespace MultiplayerGame.Code.Core.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Vector3 _maxVelocityChange;
        [SerializeField] private float _speed;
        private IInputService _inputService;
        
        public void Construct(IInputService inputService) => _inputService = inputService;

        private void FixedUpdate()
        {
            if (_inputService is null) return;
            _rigidbody.AddForce(GetVelocityFromInput(), ForceMode.VelocityChange);
        }

        private Vector3 GetVelocityFromInput()
        {
            Vector2 moveAxes = _inputService.MovementAxes;
            Vector3 targetVelocity = transform.TransformDirection(moveAxes);
            targetVelocity = new Vector2(targetVelocity.x + _speed, targetVelocity.y + _speed);

            return moveAxes.magnitude > 0.5f
                ? (targetVelocity - _rigidbody.velocity).Clamp(-_maxVelocityChange, _maxVelocityChange)
                : Vector3.zero;
        }
    }
}