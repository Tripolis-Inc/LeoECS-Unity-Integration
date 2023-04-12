// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections;
using NUnit.Framework;
using TripolisInc.Test.Runtime.Helpers;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace TripolisInc.Test.Runtime
{
    [TestFixture]
    public class TestSceneLoadTest
    {
        [Test]
        [LoadScene(TestConstants.Scenes.EmptyScene)]
        public void VerifyLoadEmptyScene() => Test(TestConstants.Scenes.EmptyScene);

        [Test]
        [LoadScene(TestConstants.Scenes.ECS_TEST)]
        public void VerifyLoadEcsScene() => Test(TestConstants.Scenes.ECS_TEST);
        
        [Test] 
        [LoadScene(TestConstants.Scenes.EmptyScene), LoadScene(TestConstants.Scenes.ECS_TEST, LoadSceneMode.Additive)]
        public void VerifyLoadMultilyScenes()
        {
            var scenes = new[] { TestConstants.Scenes.EmptyScene, TestConstants.Scenes.ECS_TEST };
            MultiplyTest(scenes);
        }

        [UnityTest] 
        [LoadScene(TestConstants.Scenes.EmptyScene)]
        public IEnumerator VerifyLoadUnityEmptyScene()
        {
            Test(TestConstants.Scenes.EmptyScene);
            yield return null;
        }

        [UnityTest] 
        [LoadScene(TestConstants.Scenes.ECS_TEST)]
        public IEnumerator VerifyLoadUnityEcsScene()
        {
            Test(TestConstants.Scenes.ECS_TEST);
            yield return null;
        }
        
        [UnityTest] 
        [LoadScene(TestConstants.Scenes.EmptyScene), LoadScene(TestConstants.Scenes.ECS_TEST, LoadSceneMode.Additive)]
        public IEnumerator VerifyLoadUnityMultilyScenes()
        {
            var scenes = new[] { TestConstants.Scenes.EmptyScene, TestConstants.Scenes.ECS_TEST };
            MultiplyTest(scenes);

            yield return null;
        }
        
        private void Test(string scenePath)
        {
            var currentScene = SceneManager.GetActiveScene();
            Assert.AreEqual(currentScene.path, scenePath);
        }

        private void MultiplyTest(string[] scenePaths)
        {
            for (int i = 0; i < scenePaths.Length; i++)
            {
                var sceneByIndex = SceneManager.GetSceneAt(i);
                Assert.AreEqual(sceneByIndex.path, scenePaths[i]);
            }
        }
    }
}