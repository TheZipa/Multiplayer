using MultiplayerGame.Code.Data.StaticData;

namespace MultiplayerGame.Code.Services.StaticData.StaticDataProvider
{
    public interface IStaticDataProvider
    {
        GameConfiguration LoadGameConfiguration();
        WorldData LoadLocationData();
    }
}