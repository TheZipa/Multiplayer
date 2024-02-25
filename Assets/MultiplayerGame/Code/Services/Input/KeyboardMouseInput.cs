using System;
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
		
		private readonly UserInput _userInput;
		private const string Escape = "Escape";

		public KeyboardMouseInput()
		{
			_userInput = new UserInput();
			_userInput.Player.Back.performed += context => OnBack?.Invoke();
			_userInput.Enable();
		}

		public void Enable()
		{
			foreach (InputAction action in _userInput.asset.actionMaps[0].actions)
			{
				if (action.name == Escape) return;
				action.Enable();
			}
		}

		public void Disable()
		{
			foreach (InputAction action in _userInput.asset.actionMaps[0].actions)
			{
				if (action.name == Escape) return;
				action.Disable();
			}
		}
	}
}