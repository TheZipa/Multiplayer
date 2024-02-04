using UnityEngine;

namespace MultiplayerGame.Code.Services.Input
{
    public class InputService : IInputService
    {
        public Vector2 MouseDelta => _playerInput.PlayerMap.MouseDelta.ReadValue<Vector2>();
        public Vector2 MovementAxes => new Vector2(_playerInput.PlayerMap.HorizontalMovement.ReadValue<float>(),
            _playerInput.PlayerMap.VerticalMovement.ReadValue<float>());

        private readonly PlayerInput _playerInput = new PlayerInput();

        public void Enable() => _playerInput.Enable();

        public void Disable() => _playerInput.Disable();
    }
}