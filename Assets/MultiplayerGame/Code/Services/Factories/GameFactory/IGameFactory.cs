using Cysharp.Threading.Tasks;
using MultiplayerGame.Code.Core.Player;
using MultiplayerGame.Code.Services.Factories.BaseFactory;

namespace MultiplayerGame.Code.Services.Factories.GameFactory
{
    public interface IGameFactory : IBaseFactory
    {
        UniTask WarmUp();
        Player CreatePlayer();
        UniTask<ThirdPersonPlayerCamera> CreatePlayerCamera();
    }
}