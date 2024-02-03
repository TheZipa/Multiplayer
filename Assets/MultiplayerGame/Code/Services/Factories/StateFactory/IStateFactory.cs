using MultiplayerGame.Code.Infrastructure.StateMachine.States;

namespace MultiplayerGame.Code.Services.Factories.StateFactory
{
    public interface IStateFactory
    {
        TState CreateState<TState>() where TState : class, IState;
        TState CreateState<TState, TPayload>() where TState : class, IPayloadedState<TPayload>;
    }
}