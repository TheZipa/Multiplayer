using Cysharp.Threading.Tasks;
using Game.Code.Services.Assets;
using Game.Code.Services.EntityContainer;
using UnityEngine;

namespace Game.Code.Services.Factories.BaseFactory
{
    public abstract class BaseFactory : IBaseFactory
    {
        protected readonly IAssets _assets;
        protected readonly IEntityContainer _entityContainer;

        protected BaseFactory(IAssets assets, IEntityContainer entityContainer)
        {
            _assets = assets;
            _entityContainer = entityContainer;
        }

        public async UniTask<T> InstantiateAsRegistered<T>(Vector3 at, Quaternion rotation, Transform parent = null) where T : Object, IFactoryEntity
        {
            GameObject instantiatedObject = await _assets.Instantiate<GameObject>(typeof(T).Name, parent);
            T component = instantiatedObject.GetComponent<T>();
            _entityContainer.RegisterEntity(component);
            return component;
        }
        
        public async UniTask<T> InstantiateAsRegistered<T>(Transform parent = null) where T : Object, IFactoryEntity
        {
            GameObject instantiatedObject = await _assets.Instantiate<GameObject>(typeof(T).Name, parent);
            T component = instantiatedObject.GetComponent<T>();
            _entityContainer.RegisterEntity(component);
            return component;
        }

        public async UniTask<T> Instantiate<T>(Vector3 at, Vector3 rotation, Transform parent = null) where T : Object
        {
            GameObject instantiatedObject = await _assets.Instantiate<GameObject>(typeof(T).Name, parent);
            instantiatedObject.transform.position = at;
            instantiatedObject.transform.rotation = Quaternion.Euler(rotation);
            return instantiatedObject.GetComponent<T>();
        }

        public async UniTask<T> Instantiate<T>(Transform parent = null) where T : Object
        {
            GameObject instantiatedObject = await _assets.Instantiate<GameObject>(typeof(T).Name, parent);
            return instantiatedObject.GetComponent<T>();
        }
    }
}