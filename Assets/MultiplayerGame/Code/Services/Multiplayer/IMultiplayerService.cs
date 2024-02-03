using MultiplayerGame.Code.Services.Factories.GameFactory;

namespace MultiplayerGame.Code.Services.Multiplayer
{
    public interface IMultiplayerService
    {
        void Construct(IGameFactory gameFactory);
        void Connect();
    }
}