using System;
using System.Collections.Generic;
using MultiplayerGame.Code.Infrastructure.StateMachine.States;

namespace MultiplayerGame.Code.Infrastructure.StateMachine.GameStateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly IDictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine() => _states = new Dictionary<Type, IExitableState>(10);

        public void Enter<TState>() where TState : class, IState => ChangeState<TState>().Enter();

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload> =>
            ChangeState<TState>().Enter(payload);

        public void AddState<TState>(TState instance) where TState : class, IState =>
            _states.Add(typeof(TState), instance);

        public void AddState<TState, TPayload>(TState instance) where TState : class, IPayloadedState<TPayload> =>
            _states.Add(typeof(TState), instance);

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;

        ~GameStateMachine() => _activeState.Exit();
    }
}