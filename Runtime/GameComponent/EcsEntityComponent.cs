// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using TripolisInc.EcsCore.Component;
using TripolisInc.EcsCore.Misc;
using UnityEngine;

namespace TripolisInc.EcsCore.GameComponent
{
    public class EcsEntityComponent : MonoBehaviour
    {
#if UNITY_EDITOR
        public const string ECS_COMPONENTS_PROP_NAME = nameof(ecsComponents);
#endif
        
        [SerializeField] 
        private EcsWorldComponent world;
        [SerializeField]
        private UnityEngine.Component[] unityComponents;
        [SerializeField]
        private List<ComponentContainer> ecsComponents;

        private int _entityId;
        private bool _isInited = false;

        private void Start()
        {
            if (_isInited)
                return;

            world = GetComponentInParent<EcsWorldComponent>(true);
            if (world == null)
            {
                Debug.LogError("Cannot find ecs world.");
                Destroy(this);
            }

            world.BindEntity(this);
        }

        public void Init(EcsWorldComponent world, int entityId)
        {
            if (_isInited)
                return;
            
            this.world = world;
            _entityId = entityId;
            InitComponents();
            
            _isInited = true;
        }

        private void InitComponents()
        {
            var ecsWorld = world.GetEcsWorld();
            var entityPool = ecsWorld.GetPool<UnityComponent<EcsEntityComponent>>();
            ref var c = ref entityPool.Add(_entityId);
            c.unityComponent = this;

            var genericType = typeof(UnityComponent<>);
            foreach (var unityComponent in unityComponents.DistinctBy(x => x.GetType()))
            {
                var componentType = genericType.MakeGenericType(unityComponent.GetType());
                var instanceComp = Activator.CreateInstance(componentType);
                var field = componentType.GetField(UnityComponent<UnityEngine.Component>.ComponentFieldName);
                field.SetValue(instanceComp, unityComponent);
                
                var pool = ecsWorld.GetOrCreatePoolByType(componentType);
                AddComponent(pool, instanceComp);
            }

            foreach (var componentContainer in ecsComponents)
            {
                var componentType = componentContainer.component.GetType();
                var pool = ecsWorld.GetOrCreatePoolByType(componentType);
                AddComponent(pool, componentContainer.component);
            }

            void AddComponent(IEcsPool pool, object instanceComp)
            {
                if (pool.Has(_entityId))
                    pool.SetRaw(_entityId, instanceComp);
                else 
                    pool.AddRaw(_entityId, instanceComp);
            }
        }

        private void OnDestroy()
        {
            if (world == null)
                return;
            
            var ecsWorld = world.GetEcsWorld();
            if (ecsWorld == null)
                return;
            
            ecsWorld.DelEntity(_entityId);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (world != null)
                return;

            world = GetComponentInParent<EcsWorldComponent>(true);
        }
        
        public void AddEcsComponent(Type componentType)
        {
            var componentInstance = Activator.CreateInstance(componentType);
            var container = new ComponentContainer() { component = componentInstance };
            ecsComponents.Add(container);
        }

        public void DeleteEcsComponent(Type componentType)
        {
            var index = ecsComponents.FindIndex(x => x.component.GetType() == componentType);
            if (index < 0)
                return;
            
            ecsComponents.RemoveAt(index);
        }
        
        public ComponentContainer GetEcsComponent(int index) => ecsComponents[index]; 
#endif

        [Serializable]
        public struct ComponentContainer
        {
#if UNITY_EDITOR
            public const string COMPONENT_FIELD_NAME = nameof(component);
#endif
            
            [SerializeReference]
            public object component;
        }
    }
}