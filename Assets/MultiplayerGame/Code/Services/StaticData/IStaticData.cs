using MultiplayerGame.Code.Data.StaticData;

namespace MultiplayerGame.Code.Services.StaticData
{
    public interface IStaticData
    {
        GameConfiguration GameConfiguration { get; }
        WorldData WorldData { get; }
    }
}