// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System;
using Leopotam.EcsLite;
using TripolisInc.EcsCore.Component;
using TripolisInc.EcsCore.GameComponent;
using TripolisInc.EcsCore.Service;
using UnityEngine;

namespace TripolisInc.EcsCore.Misc
{
    public static class EcsWorldExtension
    {
        public static readonly int DontCreatedEntity = -1;
        
        public static bool IsGameObjectEntity(this EcsWorld world, int entity) =>
            world.GetPool<UnityComponent<EcsEntityComponent>>().Has(entity);

        public static int CreateEntityFromPrefab(this EcsWorld world, GameObject prefab, Transform parent = null)
        {
            var worldComponent = EcsWorldsContainer.Instance.GetWorld(world);
            if (worldComponent == null)
                return DontCreatedEntity;

            return worldComponent.CreateEntityFromPrefab(prefab, parent);
        }

        public static int CreateGameObjectEntity(this EcsWorld world, string name, Transform parent = null)
        {
            var worldComponent = EcsWorldsContainer.Instance.GetWorld(world);
            if (worldComponent == null)
                return DontCreatedEntity;

            return worldComponent.CreateEntity(name, parent);
        }

        public static IEcsPool GetOrCreatePoolByType(this EcsWorld world, Type type)
        {
            var pool = world.GetPoolByType(type);
            if (pool != null)
                return pool;

            var methodInfo = typeof(EcsWorld).GetMethod("GetPool");
            if (methodInfo == null)
            {
                Debug.LogError("Cannot find method");
                return null;
            }

            return methodInfo.MakeGenericMethod(type).Invoke(world, new object[0]) as IEcsPool;
        }
    }
}