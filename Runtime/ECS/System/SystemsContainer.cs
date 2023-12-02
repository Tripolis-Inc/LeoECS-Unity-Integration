// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using Leopotam.EcsLite;

namespace TripolisInc.EcsCore.System
{
    public class SystemsContainer
    {
        private List<IEcsSystem> _systems = new List<IEcsSystem>(128);

        public void AddSystem(IEcsSystem system) => _systems.Add(system);

        public void Populate(IEcsSystems systems)
        {
            foreach (var system in _systems)
            {
                systems.Add(system);
            }
        }
    }
}