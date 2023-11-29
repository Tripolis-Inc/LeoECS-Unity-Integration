// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System;
using Leopotam.EcsLite;
using TripolisInc.EcsCore.Interfaces;
using TripolisInc.EcsCore.Misc;
using TripolisInc.EcsCore.Service;
using UnityEngine;

namespace TripolisInc.EcsCore.GameComponent
{
    public abstract class BaseEcsWorldComponent : EcsMonoBehavior
    {
        protected EcsWorld World = new EcsWorld();

        protected virtual void Start()
        {
            EcsWorldsContainer.Instance.AddWorld(World, this);
        }

        protected virtual void OnDestroy()
        {
            if (World != null)
            {
                EcsWorldsContainer.Instance.RemoveWorld(World);
                World.Destroy();
                World = null;
            }
        }
        
        public EcsWorld GetEcsWorld()
        {
            if (World == null)
            {
                Debug.LogWarning("World is destroyed or not created.");
                return null;
            }

            return World;
        }

        public int CreateEntityFromPrefab<T>(GameObject prefab, Transform parent = null) where T : EcsMonoBehavior, IEcsEntityComponent
        {
            if (prefab == null)
                throw new ArgumentNullException(nameof(prefab));
            
            if (parent == null)
                parent = transform;
            
            var entity = World.NewEntity();
            var go = Instantiate(prefab, parent);
            var entityComponent = go.GetOrAddComponent<T>();
            entityComponent.Bind(this, entity);
            return entity;
        }

        public int CreateEntity<T>(string name = null, Transform parent = null) where T : EcsMonoBehavior, IEcsEntityComponent
        {
            if (parent == null)
                parent = transform;

            if (string.IsNullOrEmpty(name))
                name = "EntityObject";

            var entity = World.NewEntity();
            var go = new GameObject(name, typeof(T));
            go.transform.SetParent(parent);
            
            var entityComponent = go.GetComponent<T>();
            entityComponent.Bind(this, entity);
            return entity;
        }

        public void BindEntity(IEcsEntityComponent component)
        {
            if (!component.IsAlive())
            {
                Debug.LogError("Component cannot be bind, because he isn't alive.");
                return;
            }

            var entity = World.NewEntity();
            component.Bind(this, entity);
        }
    }
}