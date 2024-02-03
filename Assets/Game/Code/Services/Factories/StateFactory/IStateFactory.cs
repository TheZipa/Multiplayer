using Game.Code.Infrastructure.StateMachine.States;

namespace Game.Code.Services.Factories.StateFactory
{
    public interface IStateFactory
    {
        TState CreateState<TState>() where TState : class, IState;
        TState CreateState<TState, TPayload>() where TState : class, IPayloadedState<TPayload>;
    }
}