using Leopotam.EcsLite;

namespace Tests.Runtime.Ecs.Systems
{
    public class StubTestSystem : IEcsInitSystem, IEcsRunSystem
    {
        public void Init(IEcsSystems systems)
        {
        }

        public void Run(IEcsSystems systems)
        {
        }
    }
}