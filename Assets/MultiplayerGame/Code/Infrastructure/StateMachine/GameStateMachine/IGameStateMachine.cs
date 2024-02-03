using MultiplayerGame.Code.Infrastructure.StateMachine.States;

namespace MultiplayerGame.Code.Infrastructure.StateMachine.GameStateMachine
{
    public interface IGameStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
        void AddState<TState>(TState instance) where TState : class, IState;
        void AddState<TState, TPayload>(TState instance) where TState : class, IPayloadedState<TPayload>;
    }
}