using System;

namespace MultiplayerGame.Code.Services.Multiplayer
{
    public interface IMultiplayerService
    {
        void Connect();
        event Action OnRoomJoined;
    }
}