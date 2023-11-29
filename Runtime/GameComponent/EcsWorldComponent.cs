// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using Leopotam.EcsLite;

namespace TripolisInc.EcsCore.GameComponent
{
    public abstract class EcsWorldComponent : BaseEcsWorldComponent
    {
        private IEcsSystems _systems;

        protected virtual void Start()
        {   
            base.Start();
            
            _systems = new EcsSystems(World, GetSharedData());
            PopulateSystems(_systems);
            _systems.Init();
        }

        protected abstract void PopulateSystems(IEcsSystems systems);
        protected abstract object GetSharedData();
        
        private void Update()
        {
            _systems?.Run();
        }

        protected override void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }
            
            base.OnDestroy();
        }
    }
}
