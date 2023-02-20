// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using Leopotam.EcsLite;
using TripolisInc.EcsCore.Misc;
using TripolisInc.EcsCore.Service;
using UnityEngine;

namespace TripolisInc.EcsCore.GameComponent
{
    public abstract class EcsWorldComponent : MonoBehaviour
    {
        private EcsWorld _world;
        private IEcsSystems _systems;

        protected virtual void Start()
        {
            _world = new EcsWorld();
            EcsWorldsContainer.Instance.AddWorld(_world, this);
            
            _systems = new EcsSystems(_world, GetSharedData());
            PopulateSystems(_systems);
            _systems.Init();
        }

        protected abstract void PopulateSystems(IEcsSystems systems);
        protected abstract object GetSharedData();
        
        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }

            if (_world != null)
            {
                EcsWorldsContainer.Instance.RemoveWorld(_world);
                _world.Destroy();
                _world = null;
            }
        }

        public EcsWorld GetEcsWorld()
        {
            if (_world == null)
            {
                Debug.LogWarning("World is destroyed or not created.");
                return null;
            }

            return _world;
        }

        public int CreateEntityFromPrefab(GameObject prefab, Transform parent = null)
        {
            if (parent == null)
                parent = transform;
            
            var entity = _world.NewEntity();
            var go = Instantiate(prefab, parent);
            var entityComponent = go.GetOrAddComponent<EcsEntityComponent>();
            entityComponent.Init(this, entity);
            return entity;
        }

        public int CreateEntity(string name = null, Transform parent = null)
        {
            if (parent == null)
                parent = transform;

            if (string.IsNullOrEmpty(name))
                name = "EntityObject";

            var entity = _world.NewEntity();
            var go = new GameObject(name, typeof(EcsEntityComponent));
            go.transform.SetParent(parent);
            
            var entityComponent = go.GetComponent<EcsEntityComponent>();
            entityComponent.Init(this, entity);
            return entity;
        }

        public void BindEntity(EcsEntityComponent component)
        {
            if (component == null)
                return;

            var entity = _world.NewEntity();
            component.Init(this, entity);
        }
    }
}
