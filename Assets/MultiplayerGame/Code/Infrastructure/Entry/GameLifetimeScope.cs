using MultiplayerGame.Code.Infrastructure.StateMachine.GameStateMachine;
using MultiplayerGame.Code.Infrastructure.StateMachine.States;
using MultiplayerGame.Code.Services.Assets;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.Factories.GameFactory;
using MultiplayerGame.Code.Services.Factories.StateFactory;
using MultiplayerGame.Code.Services.Factories.UIFactory;
using MultiplayerGame.Code.Services.Input;
using MultiplayerGame.Code.Services.LoadingCurtain;
using MultiplayerGame.Code.Services.Multiplayer;
using MultiplayerGame.Code.Services.SaveLoad;
using MultiplayerGame.Code.Services.SceneLoader;
using MultiplayerGame.Code.Services.Sound;
using MultiplayerGame.Code.Services.StaticData;
using MultiplayerGame.Code.Services.StaticData.StaticDataProvider;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MultiplayerGame.Code.Infrastructure.Entry
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private SoundService _soundService;
        [SerializeField] private LoadingCurtain _loadingCurtain;
        [SerializeField] private MultiplayerService _multiplayerService;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameEntry>();
            builder.RegisterComponent(_soundService).AsImplementedInterfaces();
            builder.RegisterComponent(_loadingCurtain).AsImplementedInterfaces();
            builder.RegisterComponent(_multiplayerService).AsImplementedInterfaces();

            RegisterServices(builder);
            RegisterFactories(builder);
            RegisterStates(builder);
        }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        private static void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<AssetProvider>(Lifetime.Singleton).As<IAssets>();
            builder.Register<SceneLoader>(Lifetime.Singleton).As<ISceneLoader>();
            builder.Register<GameStateMachine>(Lifetime.Singleton).As<IGameStateMachine>();
            builder.Register<InputService>(Lifetime.Singleton).As<IInputService>();
            builder.Register<SaveLoad>(Lifetime.Singleton).As<ISaveLoad>();
            builder.Register<StaticData>(Lifetime.Singleton).As<IStaticData>();
            builder.Register<StaticDataProvider>(Lifetime.Singleton).As<IStaticDataProvider>();
            builder.Register<EntityContainer>(Lifetime.Singleton).As<IEntityContainer>();
        }

        private static void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<StateFactory>(Lifetime.Singleton).As<IStateFactory>();
            builder.Register<GameFactory>(Lifetime.Singleton).As<IGameFactory>();
            builder.Register<UIFactory>(Lifetime.Singleton).As<IUIFactory>();
        }

        private static void RegisterStates(IContainerBuilder builder)
        {
            builder.Register<LoadApplicationState>(Lifetime.Singleton);
            builder.Register<LoadMenuState>(Lifetime.Singleton);
            builder.Register<LoadGameState>(Lifetime.Singleton);
            builder.Register<MenuState>(Lifetime.Singleton);
            builder.Register<RoomState>(Lifetime.Singleton);
            builder.Register<GameplayState>(Lifetime.Singleton);
        }
    }
}