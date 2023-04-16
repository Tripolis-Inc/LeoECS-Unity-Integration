// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.Runtime.UnityComponents;
using TripolisInc.EcsCore.GameComponent;
using TripolisInc.EcsCore.Misc;
using TripolisInc.Test.EcsCore.Component;
using TripolisInc.Test.Runtime.Helpers;
using UnityEngine;
using UnityEngine.TestTools;

namespace TripolisInc.Test.Runtime.UnityComponent
{
    [TestFixture(Category = "Unity Component")]
    public class TestEcsEntityComponent
    {
        private GameObject _container;
        private SpyWorldComponent _worldComponent;
        private EcsEntityComponent _entityComponent;

        [UnitySetUp]
        public IEnumerator TestSetup()
        {
            _container = new GameObject("Test Container");
            yield return new WaitForEndOfFrame();
        }

        [UnityTest]
        public IEnumerator TestEmptyWorldInitialization()
        {
            yield return InstantiateEntityComponent();
            LogAssert.Expect(LogType.Error, "Cannot find ecs world.");
            Assert.IsFalse(_entityComponent.IsAlive());
        }

        [UnityTest]
        public IEnumerator TestEntityInitialization()
        {
            yield return InstantiateWorldComponent();
            Assert.That(_worldComponent, Is.Not.Null);

            yield return InstantiateEntityComponent();
            Assert.That(_entityComponent, Is.Not.Null);
            Assert.IsTrue(_entityComponent.IsInited);

            var ecsWorld = _worldComponent.GetEcsWorld();
            int[] entities = null;
            ecsWorld.GetAllEntities(ref entities);
            
            Assert.IsTrue(entities.Contains(_entityComponent.EntityId));
            var pool = ecsWorld.GetPool<TripolisInc.EcsCore.Component.UnityComponent<EcsEntityComponent>>();
            Assert.IsTrue(pool.Has(_entityComponent.EntityId));
        }

        [UnityTest]
        public IEnumerator TestEntityDestroy()
        {
            yield return InstantiateWorldComponent();
            Assert.That(_worldComponent, Is.Not.Null);

            yield return InstantiateEntityComponent();
            Assert.That(_entityComponent, Is.Not.Null);

            int[] entities = null;
            var ecsWorld = _worldComponent.GetEcsWorld();
            var entityId = _entityComponent.EntityId;

            { // Entity game object destroy
                GameObject.Destroy(_entityComponent);
                yield return new WaitForEndOfFrame();
                
                Assert.IsFalse(_entityComponent.IsAlive());
                ecsWorld.GetAllEntities(ref entities);
                Assert.IsFalse(entities.Contains(entityId));
            }

            yield return InstantiateEntityComponent();
            Assert.That(_entityComponent, Is.Not.Null);
            entityId = _entityComponent.EntityId;

            { // Entity destroy through dispose
                _entityComponent.Dispose();
                yield return new WaitForEndOfFrame();
                
                Assert.IsFalse(_entityComponent.IsAlive());
                ecsWorld.GetAllEntities(ref entities);
                Assert.IsFalse(entities.Contains(entityId));
            }
        }

        [UnityTest]
        public IEnumerator TestEcsComponentsInitialization()
        {
            yield return InstantiateWorldComponent();
            Assert.That(_worldComponent, Is.Not.Null);
            var ecsWorld = _worldComponent.GetEcsWorld();

            var entityObject = GameObject.Instantiate(Resources.Load<GameObject>(TestConstants.TestPrefabPath));
            Assert.That(entityObject, Is.Not.Null);
            entityObject.transform.SetParent(_container.transform);
            yield return new WaitForEndOfFrame();
            _entityComponent = entityObject.GetComponent<EcsEntityComponent>();
            Assert.That(_entityComponent, Is.Not.Null);
            Assert.IsTrue(_entityComponent.IsInited);
            
            var pool = ecsWorld.GetPool<TripolisInc.EcsCore.Component.UnityComponent<EcsEntityComponent>>();
            Assert.IsTrue(pool.Has(_entityComponent.EntityId));

            var testComponentPool = ecsWorld.GetPool<TestComponent>();
            Assert.IsTrue(testComponentPool.Has(_entityComponent.EntityId));

            var unityColliderPool = ecsWorld.GetPool<TripolisInc.EcsCore.Component.UnityComponent<BoxCollider>>();
            Assert.IsTrue(unityColliderPool.Has(_entityComponent.EntityId));
        }
        
        [UnityTearDown]
        public IEnumerator TestTearDown()
        {
            if (_container != null )
                GameObject.Destroy(_container);

            _container = null;
            _worldComponent = null;
            yield return null;
        }

        private IEnumerator InstantiateWorldComponent()
        {
            _worldComponent = _container.AddComponent<SpyWorldComponent>();
            yield return new WaitForEndOfFrame();
        }

        private IEnumerator InstantiateEntityComponent()
        {
            var entityObject = new GameObject("Entity Object");
            entityObject.transform.SetParent(_container.transform);
            yield return new WaitForEndOfFrame();
            
            _entityComponent = entityObject.AddComponent<EcsEntityComponent>();
            yield return new WaitForEndOfFrame();
        }
    }
}