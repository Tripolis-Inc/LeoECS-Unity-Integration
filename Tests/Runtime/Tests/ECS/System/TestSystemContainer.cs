// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using Leopotam.EcsLite;
using NUnit.Framework;
using Tests.Runtime.Ecs.Systems;
using TripolisInc.EcsCore.System;

namespace Tests.Runtime.Tests.ECS.System
{
    [TestFixture(Category = "ECS System")]
    public class TestSystemContainer
    {
        private EcsWorld _world;
        private EcsSystems _systems;
        private SystemsContainer _container;

        [SetUp]
        public void SetUp()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _container = new SystemsContainer();
            
            _container.AddSystem(new SpySystem());
            _container.AddSystem(new SpySystem());
        }

        [Test]
        public void TestSystemsContainer()
        {
            _systems.Add(new SpySystem());
            _container.Populate(_systems);
            _systems.Add(new StubTestSystem());
            
            _systems.Init();
            
            var allSystems = _systems.GetAllSystems();
            Assert.AreEqual(allSystems.Count, 4);
            
            Assert.AreSame(allSystems[0].GetType(), typeof(SpySystem));
            Assert.AreSame(allSystems[1].GetType(), typeof(SpySystem));
            Assert.AreSame(allSystems[2].GetType(), typeof(SpySystem));
            Assert.AreSame(allSystems[3].GetType(), typeof(StubTestSystem));
            
            Assert.IsTrue(((SpySystem) allSystems[1]).IsPreInited);
            Assert.IsTrue(((SpySystem) allSystems[2]).IsPreInited);
            
            Assert.IsTrue(((SpySystem) allSystems[1]).IsInited);
            Assert.IsTrue(((SpySystem) allSystems[2]).IsInited);
        }
    }
}