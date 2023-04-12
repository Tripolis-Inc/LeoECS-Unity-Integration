using Leopotam.EcsLite;
using TripolisInc.EcsCore.GameComponent;

namespace TripolisInc.Test.EcsCore
{
    public class TestWorldComponent : EcsWorldComponent
    {
        protected override void PopulateSystems(IEcsSystems systems)
        {
            systems.Add(new TestSystem());
            systems.Add(new SecondTestSystem());
            systems.Add(new CreateObjectSystem());
        }

        protected override object GetSharedData() => null;
    }
}