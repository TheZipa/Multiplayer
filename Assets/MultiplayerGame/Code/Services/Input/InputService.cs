using System;
using UnityEngine;

namespace MultiplayerGame.Code.Services.Input
{
    public class InputService : IInputService
    {
        public event Action OnJump;
        public event Action OnEscape;
        private readonly float _sensitivity = 0.15f;
        
        public Vector2 MovementAxes => new(_playerInput.PlayerMap.HorizontalMovement.ReadValue<float>(),
            _playerInput.PlayerMap.VerticalMovement.ReadValue<float>());

        private readonly PlayerInput _playerInput = new PlayerInput();

        public InputService()
        {
            _playerInput.PlayerMap.Jump.performed += _ => OnJump?.Invoke();
            _playerInput.Navigation.Escape.performed += _ => OnEscape?.Invoke();
            _playerInput.Enable();
        }

        public void Enable() => _playerInput.PlayerMap.Enable();

        public void Disable() => _playerInput.PlayerMap.Disable();
        
        public float GetAxisValue(int axis) => axis == 0
                ? _playerInput.PlayerMap.MouseX.ReadValue<float>() * _sensitivity
                : _playerInput.PlayerMap.MouseY.ReadValue<float>() * _sensitivity;
    }
}