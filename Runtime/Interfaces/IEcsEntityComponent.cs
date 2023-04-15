// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System;
using Leopotam.EcsLite;
using TripolisInc.EcsCore.GameComponent;

namespace TripolisInc.EcsCore.Interfaces
{
    public interface IEcsEntityComponent : IMonoBehavior, IDisposable
    {
        void Bind(EcsWorldComponent world, int entityId);
    }
}