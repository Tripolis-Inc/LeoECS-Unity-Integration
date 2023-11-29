// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using TripolisInc.EcsCore.GameComponent;
using TripolisInc.EcsCore.Interfaces;

namespace Tests.Runtime.Engine
{
    public class SpyEntityComponent : IEcsEntityComponent
    {
        public int EntityId { get; private set; }
        public bool HasCheckInstance;
        public BaseEcsWorldComponent WorldComponent;
        public int BindCounter;

        private bool _hasInstance;

        public SpyEntityComponent() => Reset();
        
        public void SetHasInstanceValue(bool value) => _hasInstance = value;
        
        public bool HasInstance()
        {
            HasCheckInstance = true;
            return _hasInstance;
        }

        public void Bind(BaseEcsWorldComponent world, int entityId)
        {
            EntityId = entityId;
            WorldComponent = world;
            BindCounter++;
        }

        public void Dispose()
        {
            if (WorldComponent == null)
                return;
            
            var ecsWorld = WorldComponent.GetEcsWorld();
            if (ecsWorld == null)
                return;
            
            ecsWorld.DelEntity(EntityId);
        }
        
        public void Reset()
        {
            EntityId = 0;
            WorldComponent = null;
            HasCheckInstance = false;
            _hasInstance = true;
            BindCounter = 0;
        }
    }
}