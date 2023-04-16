// using System.Runtime.CompilerServices;
// using Leopotam.EcsLite;
// using TripolisInc.EcsCore.Component;
// using UnityEngine;
//
// namespace TripolisInc.Test.EcsCore
// {
//     public class SecondTestSystem : IEcsInitSystem, IEcsRunSystem
//     {
//         private EcsWorld _world;
//         private EcsFilter _filter;
//         private EcsPool<TestSecondComponent> _pool;
//
//         public void Init(IEcsSystems systems)
//         {
//             _world = systems.GetWorld();
//             _filter = _world.Filter<TestSecondComponent>().End();
//             _pool = _world.GetPool<TestSecondComponent>();
//         }
//
//         public void Run(IEcsSystems systems)
//         {
//             var transformPool = _world.GetPool<UnityComponent<Transform>>();
//             foreach (var entity in _filter)
//             {
//                 if (!transformPool.Has(entity))
//                 {
//                     Debug.LogError($"Transform component not found for entity {entity}");
//                     continue;
//                 }
//
//                 ref var component = ref transformPool.Get(entity);
//                 if (component.unityComponent == null)
//                 {
//                     Debug.LogError($"Transform reference is not valid. Entity - {entity}");
//                 }
//             }
//         }
//     }
// }