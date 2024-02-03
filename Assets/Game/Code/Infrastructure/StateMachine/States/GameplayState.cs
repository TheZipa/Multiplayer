using Game.Code.Infrastructure.StateMachine.GameStateMachine;
using Game.Code.Services.EntityContainer;
using Game.Code.Services.LoadingCurtain;

namespace Game.Code.Infrastructure.StateMachine.States
{
    public class GameplayState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IEntityContainer _entityContainer;
        private readonly ILoadingCurtain _loadingCurtain;

        public GameplayState(IGameStateMachine gameStateMachine, IEntityContainer entityContainer, ILoadingCurtain loadingCurtain)
        {
            _gameStateMachine = gameStateMachine;
            _entityContainer = entityContainer;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter()
        {
            _loadingCurtain.Hide();
        }

        public void Exit()
        {
            
        }
    }
}