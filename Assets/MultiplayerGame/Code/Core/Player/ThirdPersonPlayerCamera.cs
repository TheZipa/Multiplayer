using Cinemachine;
using MultiplayerGame.Code.Core.UI.Settings;
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
        private InGameMenuPanel _inGameMenuPanel;

        public void Construct(IInputService inputService, InGameMenuPanel inGameMenuPanel,
            Transform orientation, Transform player, Transform view)
        {
            _inputService = inputService;
            _inGameMenuPanel = inGameMenuPanel;
            _orientation = orientation;
            _player = player;
            _view = view;
            _freelookCamera.Follow = _freelookCamera.LookAt = view;
            _freelookCamera.m_XAxis.SetInputAxisProvider(0, _inputService);
            _freelookCamera.m_YAxis.SetInputAxisProvider(1, _inputService);
            _inGameMenuPanel.OnShow += DisableCameraInput;
            _inGameMenuPanel.OnHide += EnableCameraInput;
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
            if (inputDirection == Vector3.zero) return;

            Vector3 direction = Vector3.Slerp(_view.forward, inputDirection.normalized,
                Time.deltaTime * _rotationSpeed);
            direction.Set(direction.x, 0, direction.z);
            _view.forward = direction;
        }

        private void DisableCameraInput()
        {
            _inputService.Disable();
        }

        private void EnableCameraInput()
        {
            _inputService.Enable();
        }
    }
}