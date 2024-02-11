using System;
using Cinemachine;
using UnityEngine;

namespace MultiplayerGame.Code.Services.Input
{
    public interface IInputService : AxisState.IInputAxisProvider
    {
        void Enable();
        void Disable();
        Vector2 MovementAxes { get; }
        event Action OnJump;
        event Action OnEscape;
    }
}