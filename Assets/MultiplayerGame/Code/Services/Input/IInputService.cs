using System;
using UnityEngine;

namespace MultiplayerGame.Code.Services.Input
{
    public interface IInputService
    {
        event Action OnBack;
        Vector2 Move { get; }
        Vector2 Look { get; }
        bool IsJump { get; }
        bool IsSprint { get; }
        void Enable();
        void Disable();
    }
}