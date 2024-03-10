using System;
using MultiplayerGame.Code.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MultiplayerGame.Code.Services.Input
{
	public class KeyboardMouseInput : IInputService
	{
		public event Action OnBack;
		
		public Vector2 Move => _userInput.Player.Move.ReadValue<Vector2>();
		public Vector2 Look => _userInput.Player.Look.ReadValue<Vector2>();
		public bool IsJump => _userInput.Player.Jump.IsPressed();
		public bool IsSprint => _userInput.Player.Sprint.IsPressed();
		public bool IsCrouch { get; private set; }
		
		private readonly UserInput _userInput;
		private const string Escape = "Back";

		public KeyboardMouseInput()
		{
			_userInput = new UserInput();
			_userInput.Player.Back.performed += context => OnBack?.Invoke();
			_userInput.Player.Crouch.performed += context => IsCrouch = !IsCrouch;
			_userInput.Enable();
		}

		public void Enable()
		{
			foreach (InputAction action in _userInput.asset.actionMaps[0].actions)
			{
				if (action.name == Escape) continue;
				action.Enable();
			}
			GameExtensions.DisableCursor();
		}

		public void Disable()
		{
			foreach (InputAction action in _userInput.asset.actionMaps[0].actions)
			{
				if (action.name == Escape) continue;
				action.Disable();
			}
			GameExtensions.EnableCursor();
		}
	}
}