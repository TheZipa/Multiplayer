using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Realtime;

namespace MultiplayerGame.Code.Services.Multiplayer
{
    public interface IMultiplayerCommon
    {
        event Action<EventData> OnEventReceived;
        void SetNickname(string nickname);
        void SendEvent(byte eventCode);
        int GetCurrentPlayerId();
    }
}