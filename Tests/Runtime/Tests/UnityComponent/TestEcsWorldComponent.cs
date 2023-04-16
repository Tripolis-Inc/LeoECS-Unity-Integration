// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Leopotam.EcsLite;
using NUnit.Framework;
using Tests.Runtime.Ecs.Systems;
using Tests.Runtime.Engine;
using Tests.Runtime.Helpers;
using TripolisInc.EcsCore.GameComponent;
using TripolisInc.EcsCore.Service;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime.Tests.UnityComponent
{
    [TestFixture(Category = "Unity Component")]
    public class TestEcsWorldComponent
    {
        private static readonly string[] CASE_ENTITY_NAMES = new[] { null, "SimpleName", "OtherName" };
        
        private GameObject _container;

        [UnitySetUp]
        public IEnumerator UnitySetup()
        {
            _container = new GameObject("Test Container");
            yield return null;
        }

        [UnityTest]
        public IEnumerator CreationWorldComponentTest([ValueSource(nameof(InitializationCases))] InitializationCase initializationCase)
        {
            var worldComponent = CreateSpyWorldContainer(initializationCase);
            yield return new WaitForEndOfFrame();
            
            Assert.That(worldComponent, Is.Not.Null);
            
            var ecsWorld = worldComponent.GetEcsWorld();
            Assert.That(ecsWorld, Is.Not.Null);
            Assert.That(worldComponent.Systems, Is.Not.Null);
            Assert.IsTrue(ecsWorld.IsAlive());

            var worldSystems = worldComponent.Systems as EcsSystems;
            Assert.That(worldSystems, Is.Not.Null);
            
            Assert.IsTrue(worldComponent.IsSharedDataSetted);
            Assert.AreSame(worldComponent.SharedData, initializationCase.sharedData);
            Assert.AreSame(worldSystems.GetShared<object>(), initializationCase.sharedData);

            var systemsCount = worldSystems.GetAllSystems().Count;
            Assert.AreEqual(systemsCount, initializationCase.systems.Length);
            Assert.AreEqual(systemsCount, worldComponent.AfterPopulateSystemsCount);

            foreach (var spySystem in worldSystems.GetAllSystems().OfType<SpySystem>())
            {
                Assert.IsTrue(spySystem.IsInited);
                Assert.IsTrue(spySystem.IsPreInited);
                Assert.IsFalse(spySystem.IsDestroyed);
                Assert.IsFalse(spySystem.IsPostDestroyed);
            }

            var container = EcsWorldsContainer.Instance;
            Assert.AreSame(container.GetWorld(ecsWorld), worldComponent);
        }

        [UnityTest]
        public IEnumerator DestroyingWorldComponentTest([ValueSource(nameof(InitializationCases))] InitializationCase initializationCase)
        {
            var component = CreateSpyWorldContainer(initializationCase);
            yield return new WaitForEndOfFrame();

            var ecsWorld = component.GetEcsWorld();
            var systems = component.Systems;
            
            Component.DestroyImmediate(component);
            yield return new WaitForEndOfFrame();
            
            Assert.IsFalse(ecsWorld.IsAlive());
            Assert.AreEqual(systems.GetAllSystems().Count, 0);
            foreach (var spySystem in initializationCase.systems.OfType<SpySystem>())
            {
                Assert.IsTrue(spySystem.IsDestroyed);
                Assert.IsTrue(spySystem.IsPostDestroyed);
            }
        }

        [UnityTest]
        public IEnumerator EcsObjectCreationTest([ValueSource(nameof(InitializationCases))] InitializationCase initializationCase)
        {
            var component = CreateSpyWorldContainer(initializationCase);
            yield return new WaitForEndOfFrame();
            
            var ecsWorld = component.GetEcsWorld();

            TestEntityCreation(ecsWorld, CASE_ENTITY_NAMES,
                (string name) => component.CreateEntity<EcsEntityComponent>(name, _container.transform));

            var prefabCases = new[] { Resources.Load<GameObject>(TestConstants.TestPrefabPath), new GameObject("Test") };
            Assert.Throws(typeof(ArgumentNullException), () => component.CreateEntityFromPrefab<EcsEntityComponent>(null));
            TestEntityCreation(ecsWorld, prefabCases,
                prefab => component.CreateEntityFromPrefab<EcsEntityComponent>(prefab, _container.transform));
        }

        [UnityTest]
        public IEnumerator BindEntityTest(
            [ValueSource(nameof(InitializationCases))] InitializationCase initializationCase)
        {
            var component = CreateSpyWorldContainer(initializationCase);
            yield return new WaitForEndOfFrame();

            var ecsWorld = component.GetEcsWorld();
            int[] entities = null;
            var entitiesCount = ecsWorld.GetAllEntities(ref entities);
            
            component.BindEntity(null);
            LogAssert.Expect(LogType.Error, "Component cannot be bind, because he isn't alive.");
            Assert.AreEqual(entitiesCount, ecsWorld.GetAllEntities(ref entities));

            var spyComponent = new SpyEntityComponent();
            spyComponent.SetHasInstanceValue(false);
            component.BindEntity(spyComponent);
            LogAssert.Expect(LogType.Error, "Component cannot be bind, because he isn't alive.");
            Assert.IsTrue(spyComponent.HasCheckInstance);
            Assert.AreEqual(entitiesCount, ecsWorld.GetAllEntities(ref entities));
            
            spyComponent.Reset();
            spyComponent.SetHasInstanceValue(true);
            component.BindEntity(spyComponent);
            Assert.AreEqual(spyComponent.BindCounter, 1);
            Assert.AreSame(spyComponent.WorldComponent, component);
            Assert.AreEqual(entitiesCount + 1, ecsWorld.GetAllEntities(ref entities));
            Assert.IsTrue(entities.Contains(spyComponent.EntityId));
            
            spyComponent.Dispose();
        }
        
        [UnityTearDown]
        public IEnumerator UnityTearDown()
        {
            if (_container != null)
                GameObject.Destroy(_container);

            yield return null;
        }

        private void TestEntityCreation<T>(EcsWorld ecsWorld, T[] cases, Func<T, int> callback)
        {
            int[] entites = null;
            var createdEntities = new List<int>();
            var entitiesCount = ecsWorld.GetAllEntities(ref entites);
            foreach (var caseObj in cases)
            {
                createdEntities.Add(callback(caseObj));
                entitiesCount++;
            }

            var afterCount = ecsWorld.GetAllEntities(ref entites);
            Assert.AreEqual(afterCount, entitiesCount);
            var entitiesHash = new HashSet<int>(entites);
            foreach (var entity in createdEntities)
                Assert.IsTrue(entitiesHash.Contains(entity));
        }
        
        private SpyWorldComponent CreateSpyWorldContainer(InitializationCase data)
        {
            var component = _container.AddComponent<SpyWorldComponent>();
            component.PopulatedSystems = data.systems;
            component.SharedData = data.sharedData;

            return component;
        }

        private static IEnumerable InitializationCases()
        {
            yield return new InitializationCase() { sharedData = null, systems = Array.Empty<IEcsSystem>() };
            yield return new InitializationCase() { sharedData = null, systems = new[] { new SpySystem() } };
            yield return new InitializationCase()
            {
                sharedData = new StubTestSystem(),
                systems = new IEcsSystem[] { new SpySystem(), new SpySystem(), new StubTestSystem() }
            };
            yield return new InitializationCase() { sharedData = 10, systems = Array.Empty<IEcsSystem>() };
        }

        public struct InitializationCase
        {
            public object sharedData;
            [NotNull]
            public IEcsSystem[] systems;
        }
    }
}