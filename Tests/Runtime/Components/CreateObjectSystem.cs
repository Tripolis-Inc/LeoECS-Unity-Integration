using Leopotam.EcsLite;
using TripolisInc.EcsCore.GameComponent;
using TripolisInc.EcsCore.Misc;
using UnityEngine;

namespace TripolisInc.Test.EcsCore
{
    public class CreateObjectSystem : IEcsInitSystem
    {
        private int _entity;
        
        public void Init(IEcsSystems systems)
        {
            var prefab = Resources.Load<GameObject>("EntityPrefab");
            var world = systems.GetWorld();
            _entity = world.CreateEntityFromPrefab<EcsEntityComponent>(prefab);

            if (_entity == -1)
                Debug.LogError($"Cannot create entity from {nameof(CreateObjectSystem)}");
        }
    }
}