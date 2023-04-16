// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System;
using TripolisInc.EcsCore.GameComponent;

namespace TripolisInc.EcsCore.Interfaces
{
    public interface IEcsEntityComponent : IMonoBehavior, IDisposable
    {
        int EntityId { get; }
        void Bind(EcsWorldComponent world, int entityId);
    }
}