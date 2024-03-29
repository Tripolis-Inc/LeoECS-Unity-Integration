// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System;
using Leopotam.EcsLite;
using TripolisInc.EcsCore.GameComponent;
using TripolisInc.EcsCore.Interfaces;
using TripolisInc.EcsCore.Service;
using UnityEngine;

namespace TripolisInc.EcsCore.Misc
{
    public static class EcsWorldExtension
    {
        public static int CreateEntityFromPrefab<T>(this EcsWorld world, GameObject prefab, Transform parent = null)  where T : EcsMonoBehavior, IEcsEntityComponent
        {
            var worldComponent = EcsWorldsContainer.Instance.GetWorld(world);
            if (worldComponent == null)
                throw new ArgumentException($"World component not found or destroyed for {world}");

            return worldComponent.CreateEntityFromPrefab<T>(prefab, parent);
        }

        public static int CreateGameObjectEntity<T>(this EcsWorld world, string name, Transform parent = null)  where T : EcsMonoBehavior, IEcsEntityComponent
        {
            var worldComponent = EcsWorldsContainer.Instance.GetWorld(world);
            if (worldComponent == null)
                throw new ArgumentException($"World component not found or destroyed for {world}");

            return worldComponent.CreateEntity<T>(name, parent);
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