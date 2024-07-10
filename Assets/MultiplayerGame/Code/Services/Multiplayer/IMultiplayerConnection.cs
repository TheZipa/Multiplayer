using System;
using Photon.Realtime;

namespace MultiplayerGame.Code.Services.Multiplayer
{
    public interface IMultiplayerConnection
    {
        event Action OnConnectingSuccess;
        void Connect();
        event Action<DisconnectCause> OnConnectionClosed;
    }
}