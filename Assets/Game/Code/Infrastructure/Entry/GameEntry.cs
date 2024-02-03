using Game.Code.Infrastructure.StateMachine.GameStateMachine;
using Game.Code.Infrastructure.StateMachine.States;
using Game.Code.Services.Factories.StateFactory;
using VContainer.Unity;

namespace Game.Code.Infrastructure.Entry
{
    public class GameEntry : IStartable
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStateFactory _stateFactory;

        public GameEntry(IGameStateMachine gameStateMachine, IStateFactory stateFactory)
        {
            _gameStateMachine = gameStateMachine;
            _stateFactory = stateFactory;
        }
        
        public void Start()
        {
            CreateStates();
            _gameStateMachine.Enter<LoadApplicationState>(); 
        }
        
        private void CreateStates()
        {
            _gameStateMachine.AddState(_stateFactory.CreateState<LoadApplicationState>());
            _gameStateMachine.AddState(_stateFactory.CreateState<LoadGameState>());
            _gameStateMachine.AddState(_stateFactory.CreateState<MenuState>());
            _gameStateMachine.AddState(_stateFactory.CreateState<GameplayState>());
        }
    }
}