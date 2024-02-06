using MultiplayerGame.Code.Services.Input;
using UnityEngine;

namespace MultiplayerGame.Code.Core.Player
{
    public class ThirdPersonPlayerCamera : MonoBehaviour
    {
        [SerializeField] private Transform _orientation;
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _view;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _rotationSpeed;
        private IInputService _inputService;
        
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            SetViewDirection();
            if (_inputService is null) return;
            ApplyInputRotation();
        }

        private void SetViewDirection()
        {
            Vector3 viewDirection = _player.position - new Vector3(transform.position.x, _player.position.y, transform.position.z);
            _orientation.forward = viewDirection.normalized;
        }

        private void ApplyInputRotation()
        {
            Vector2 input = _inputService.MovementAxes;
            Vector3 inputDirection = _orientation.forward * input.y +_orientation.right * input.x;
            if (inputDirection != Vector3.zero)
                _view.forward = Vector3.Slerp(_view.forward, inputDirection.normalized,
                    Time.deltaTime * _rotationSpeed);
        }
    }
}