using Leopotam.EcsLite;
using TripolisInc.EcsCore.Component;
using TripolisInc.EcsCore.GameComponent;
using TripolisInc.Test.EcsCore.Component;
using UnityEngine;

namespace TripolisInc.Test.EcsCore
{
    public class TestSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _testCompFilter;
        private EcsPool<TestComponent> _pool;
        private EcsWorld _world;
        
        public void Init(IEcsSystems systems)
        {
            Debug.Log("Init Test System");
            _world = systems.GetWorld();
            _testCompFilter = _world.Filter<TestComponent>().End();
            _pool = _world.GetPool<TestComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var secondPool = _world.GetPool<UnityComponent<EcsEntityComponent>>();
            foreach (var entity in _testCompFilter)
            {
                ref var testComponent = ref _pool.Get(entity);
                Debug.Log($"Test Component: Value - {testComponent.test} / Entity - {entity}");
                if (secondPool.Has(entity))
                {
                    ref var entityComp = ref secondPool.Get(entity);
                    Debug.Log(
                        $"Entity Component: Entity - {entity} : GOName - {entityComp.unityComponent.gameObject.name}");
                }
            }
        }
    }
}