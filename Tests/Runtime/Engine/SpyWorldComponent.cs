// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System;
using Leopotam.EcsLite;
using TripolisInc.EcsCore.GameComponent;

namespace Tests.Runtime.Engine
{
    public class SpyWorldComponent : EcsWorldComponent
    {
        [NonSerialized]
        public int BeforePopulateSystemsCount;
        [NonSerialized]
        public int AfterPopulateSystemsCount;
        [NonSerialized]
        public bool IsSharedDataSetted;

        public IEcsSystem[] PopulatedSystems;
        public IEcsSystems Systems;
        public object SharedData;
        
        protected override void PopulateSystems(IEcsSystems systems)
        {
            Systems = systems;
            BeforePopulateSystemsCount = systems.GetAllSystems().Count;
            AfterPopulateSystemsCount = BeforePopulateSystemsCount;
            if (PopulatedSystems == null)
                return;

            foreach (var addedSystem in PopulatedSystems)
                systems.Add(addedSystem);

            AfterPopulateSystemsCount = systems.GetAllSystems().Count;
        }

        protected override object GetSharedData()
        {
            IsSharedDataSetted = true;
            return SharedData;
        } 
    }
}