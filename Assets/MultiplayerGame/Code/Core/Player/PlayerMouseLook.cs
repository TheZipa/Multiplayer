using MultiplayerGame.Code.Services.Input;
using UnityEngine;

namespace MultiplayerGame.Code.Core.Player
{
    public class PlayerMouseLook : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private Vector2 _clampInDegrees = new Vector2(360, 180);
        [SerializeField] private Vector2 _sensitivity = new Vector2(2, 2);
        [SerializeField] private Vector2 _smoothing = new Vector2(3, 3);
        [Header("Target")]
        [SerializeField] private GameObject _characterBody;
        [SerializeField] private bool _lockCursor = true;

        private IInputService _inputService;
        private Vector2 _targetDirection;
        private Vector2 _targetCharacterDirection;

        private Vector2 _mouseAbsolute;
        private Vector2 _smoothMouse;
        private Vector2 _mouseDelta;

        public void Construct(IInputService inputService) => _inputService = inputService;

        private void Start()
        {
            _targetDirection = transform.localRotation.eulerAngles;

            if (_characterBody != null)
                _targetCharacterDirection = _characterBody.transform.localRotation.eulerAngles;

            if (_lockCursor) LockCursor();
        }

        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (_inputService == null) return;
            SetMouseDelta();
            AddSmooth();
            ClampAngles();
            ApplyNewRotation(Quaternion.Euler(_targetDirection), Quaternion.Euler(_targetCharacterDirection));
        }

        private void ApplyNewRotation(Quaternion targetOrientation, Quaternion targetCharacterOrientation)
        {
            transform.localRotation =
                Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;
            Quaternion yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
            _characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
        }

        private void ClampAngles()
        {
            if (_clampInDegrees.x < 360)
                _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -_clampInDegrees.x * 0.5f, _clampInDegrees.x * 0.5f);

            if (_clampInDegrees.y < 360)
                _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -_clampInDegrees.y * 0.5f, _clampInDegrees.y * 0.5f);
        }

        private void AddSmooth()
        {
            _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, _mouseDelta.x, 1f / _smoothing.x);
            _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, _mouseDelta.y, 1f / _smoothing.y);
            _mouseAbsolute += _smoothMouse;
        }

        private void SetMouseDelta() =>
            _mouseDelta = Vector2.Scale(_inputService.MouseDelta,
                new Vector2(_sensitivity.x * _smoothing.x, _sensitivity.y * _smoothing.y));
    }
}