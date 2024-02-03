using MultiplayerGame.Code.Infrastructure.StateMachine.States;
using VContainer;

namespace MultiplayerGame.Code.Services.Factories.StateFactory
{
    public class StateFactory : IStateFactory
    {
        private readonly IObjectResolver _resolver;

        public StateFactory(IObjectResolver resolver) => _resolver = resolver;

        public TState CreateState<TState>() where TState : class, IState => _resolver.Resolve<TState>();

        public TState CreateState<TState, TPayload>() where TState : class, IPayloadedState<TPayload> =>
            _resolver.Resolve<TState>();
    }
}