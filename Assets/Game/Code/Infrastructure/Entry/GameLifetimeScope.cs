using Game.Code.Infrastructure.StateMachine.GameStateMachine;
using Game.Code.Infrastructure.StateMachine.States;
using Game.Code.Services.Assets;
using Game.Code.Services.EntityContainer;
using Game.Code.Services.Factories.GameFactory;
using Game.Code.Services.Factories.StateFactory;
using Game.Code.Services.Factories.UIFactory;
using Game.Code.Services.LoadingCurtain;
using Game.Code.Services.SaveLoad;
using Game.Code.Services.SceneLoader;
using Game.Code.Services.Sound;
using Game.Code.Services.StaticData;
using Game.Code.Services.StaticData.StaticDataProvider;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Code.Infrastructure.Entry
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private SoundService _soundService;
        [SerializeField] private LoadingCurtain _loadingCurtain;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameEntry>();
            builder.RegisterComponent(_soundService).AsImplementedInterfaces();
            builder.RegisterComponent(_loadingCurtain).AsImplementedInterfaces();

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
            builder.Register<LoadGameState>(Lifetime.Singleton);
            builder.Register<MenuState>(Lifetime.Singleton);
            builder.Register<GameplayState>(Lifetime.Singleton);
        }
    }
}