// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using Leopotam.EcsLite;
using TripolisInc.EcsCore.GameComponent;

namespace Tests.Runtime.Engine
{
    public class StubWorldComponent : EcsWorldComponent
    {
        protected override void PopulateSystems(IEcsSystems systems)
        {
        }

        protected override object GetSharedData()
        {
            return null;
        }
    }
}