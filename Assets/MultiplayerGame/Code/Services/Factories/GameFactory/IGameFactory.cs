using Cinemachine;
using Cysharp.Threading.Tasks;
using MultiplayerGame.Code.Core.Player;
using MultiplayerGame.Code.Services.Factories.BaseFactory;
using UnityEngine;

namespace MultiplayerGame.Code.Services.Factories.GameFactory
{
    public interface IGameFactory : IBaseFactory
    {
        UniTask WarmUp();
        Player CreatePlayer(Vector3 spawnLocation);
        UniTask<CinemachineVirtualCamera> CreatePlayerCamera();
    }
}