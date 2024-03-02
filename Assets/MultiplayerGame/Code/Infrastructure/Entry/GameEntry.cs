using MultiplayerGame.Code.Data.StaticData;
using MultiplayerGame.Code.Infrastructure.StateMachine.GameStateMachine;
using MultiplayerGame.Code.Infrastructure.StateMachine.States;
using MultiplayerGame.Code.Services.Factories.StateFactory;
using VContainer.Unity;

namespace MultiplayerGame.Code.Infrastructure.Entry
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
            _gameStateMachine.AddState<LoadGameState, MapData>(_stateFactory.CreateState<LoadGameState, MapData>());
            _gameStateMachine.AddState(_stateFactory.CreateState<LoadApplicationState>());
            _gameStateMachine.AddState(_stateFactory.CreateState<LoadMenuState>());
            _gameStateMachine.AddState(_stateFactory.CreateState<MenuState>());
            _gameStateMachine.AddState(_stateFactory.CreateState<RoomState>());
            _gameStateMachine.AddState(_stateFactory.CreateState<GameplayState>());
        }
    }
}