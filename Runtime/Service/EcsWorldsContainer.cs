// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using Leopotam.EcsLite;
using TripolisInc.EcsCore.GameComponent;
using UnityEngine;

namespace TripolisInc.EcsCore.Service
{
    public sealed class EcsWorldsContainer
    {
        public static EcsWorldsContainer Instance { get; } = new EcsWorldsContainer();

        private Dictionary<EcsWorld, EcsWorldComponent> _worldComponents = new Dictionary<EcsWorld, EcsWorldComponent>();
        
        private EcsWorldsContainer() {}

        public bool AddWorld(EcsWorld world, EcsWorldComponent component)
        {
            if (_worldComponents.ContainsKey(world))
            {
                Debug.LogWarning($"World - {world} already added.");
                return false;
            }

            _worldComponents[world] = component;
            return true;
        }

        public void RemoveWorld(EcsWorld world) => _worldComponents.Remove(world);

        public EcsWorldComponent GetWorld(EcsWorld world)
        {
            if (_worldComponents.TryGetValue(world, out var component))
                return component;

            Debug.LogError("World not found.");
            return default;
        }
    }
}