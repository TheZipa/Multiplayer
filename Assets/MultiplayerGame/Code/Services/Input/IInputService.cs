using UnityEngine;

namespace MultiplayerGame.Code.Services.Input
{
    public interface IInputService
    {
        void Enable();
        void Disable();
        Vector2 MouseDelta { get; }
        Vector2 MovementAxes { get; }
    }
}