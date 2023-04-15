// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using Leopotam.EcsLite;
using NUnit.Framework;
using Tests.Runtime.UnityComponents;
using TripolisInc.EcsCore.GameComponent;
using TripolisInc.EcsCore.Service;
using UnityEngine;
using UnityEngine.TestTools;

namespace TripolisInc.Test.Runtime
{
    [TestFixture(Category = "Service")]
    public class TestEcsWorldsContainer
    {
        private EcsWorld _ecsWorld;
        private GameObject _worldContainer;
        private EcsWorldComponent _component;

        [OneTimeSetUp]
        public void SetupBeforeTest()
        {
            _ecsWorld = new EcsWorld();
            _worldContainer = new GameObject("WorldContainer");
            _component = _worldContainer.AddComponent<StubWorldComponent>();
        }

        [Test, Order(1)]
        public void CheckSetupCompleted()
        {
            Assert.That(_worldContainer, Is.Not.Null);
            Assert.That(_component, Is.Not.Null);
            Assert.That(_ecsWorld, Is.Not.Null);
        }
        
        [Test, Order(2)]
        public void WorldsContainerExistingTest()
        {
            var container = EcsWorldsContainer.Instance;
            Assert.That(container, Is.Not.Null);
        }

        [Test, Order(3)]
        public void GetWorldTest()
        {
            var container = EcsWorldsContainer.Instance;
            Assert.That(container.GetWorld(null), Is.Null);
            LogAssert.Expect(LogType.Error, "World not found.");

            var testWorld = new EcsWorld();
            Assert.That(container.GetWorld(testWorld), Is.Null);
            LogAssert.Expect(LogType.Error, "World not found.");
            testWorld.Destroy();
        }

        [Test, Order(4)]
        public void AddWorldComponentTest()
        {
            var container = EcsWorldsContainer.Instance;

            Assert.False(container.AddWorld(null, null));
            LogAssert.Expect(LogType.Error, "World cannot be null");
            
            Assert.False(container.AddWorld(null, _component));
            LogAssert.Expect(LogType.Error, "World cannot be null");
            
            Assert.False(container.AddWorld(_ecsWorld, null));
            LogAssert.Expect(LogType.Error, "World component cannot be null.");
            
            Assert.IsTrue(container.AddWorld(_ecsWorld, _component));
            
            Assert.IsFalse(container.AddWorld(_ecsWorld, _component));
            LogAssert.Expect(LogType.Warning, $"World - {_ecsWorld} already added.");
            
            Assert.AreSame(container.GetWorld(_ecsWorld), _component);
        }

        [Test, Order(5)]
        public void GetAddedWorldComponentTest()
        {
            var container = EcsWorldsContainer.Instance;
            var component = container.GetWorld(_ecsWorld);
            
            Assert.That(component, Is.Not.Null);
            Assert.AreSame(_component, component);

            var sameComponent = container.GetWorld(_ecsWorld);
            Assert.AreSame(sameComponent, component);
        }

        [Test, Order(6)]
        public void RemoveWorldComponentTest()
        {
            var container = EcsWorldsContainer.Instance;
            
            var beforeDelete = container.GetWorld(_ecsWorld);
            Assert.That(beforeDelete, Is.Not.Null);

            container.RemoveWorld(_ecsWorld);
            var component = container.GetWorld(_ecsWorld);
            LogAssert.Expect(LogType.Error, "World not found.");
            Assert.IsNull(component);
            Assert.AreNotSame(component, beforeDelete);

            Assert.DoesNotThrow(() => { container.RemoveWorld(_ecsWorld); });
            
            Assert.DoesNotThrow(() => { container.RemoveWorld(null); });
            LogAssert.Expect(LogType.Error, "World cannot be null.");
            
            Assert.DoesNotThrow(() =>
            {
                var world = new EcsWorld();
                container.RemoveWorld(world);
                world.Destroy();
            });
        }

        [OneTimeTearDown]
        public void TearDownAfterTest()
        {
            if (_worldContainer != null)
                GameObject.Destroy(_worldContainer);

            if (_ecsWorld != null)
                _ecsWorld.Destroy();
            
            _ecsWorld = null;
            _worldContainer = null;
            _component = null;
        }
    }
}