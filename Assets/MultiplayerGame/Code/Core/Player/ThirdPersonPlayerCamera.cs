using Cinemachine;
using MultiplayerGame.Code.Services.Input;
using UnityEngine;

namespace MultiplayerGame.Code.Core.Player
{
    public class ThirdPersonPlayerCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook _freelookCamera;
        [SerializeField] private float _rotationSpeed;
        private Transform _orientation;
        private Transform _player;
        private Transform _view;
        private IInputService _inputService;
        
        public void Construct(IInputService inputService, Transform orientation, Transform player, Transform view)
        {
            _inputService = inputService;
            _orientation = orientation;
            _player = player;
            _view = view;
            _freelookCamera.Follow = _freelookCamera.LookAt = view;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (_inputService is null) return;
            SetViewDirection();
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