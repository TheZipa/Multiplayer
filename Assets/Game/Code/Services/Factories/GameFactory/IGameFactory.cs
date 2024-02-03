using Cysharp.Threading.Tasks;
using Game.Code.Services.Factories.BaseFactory;

namespace Game.Code.Services.Factories.GameFactory
{
    public interface IGameFactory : IBaseFactory
    {
        UniTask WarmUp();
    }
}