// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace TripolisInc.Test.Runtime.Helpers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class LoadSceneAttribute : Attribute, IOuterUnityTestAction
    {
        public string path;
        private LoadSceneMode _mode;
        private Scene _currentScene;
        private Scene _originalScene;

        public LoadSceneAttribute(string scenePath, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            path = scenePath;
            _mode = loadMode;
        }

        public IEnumerator BeforeTest(ITest test)
        {
            Debug.Assert(path.EndsWith(".unity"));

            _originalScene = SceneManager.GetActiveScene();
#if UNITY_EDITOR
            _currentScene = EditorSceneManager.LoadSceneInPlayMode(path, new LoadSceneParameters(_mode));
#else
            var sceneName = TestConstants.Scenes.GetName(path);
            SceneManager.LoadScene(sceneName,_mode);
            _currentScene = SceneManager.GetSceneByName(sceneName);
#endif
            yield return null;
        }

        public IEnumerator AfterTest(ITest test)
        {
            if (_mode == LoadSceneMode.Additive && _currentScene.isLoaded)
            {
                var operation = SceneManager.UnloadSceneAsync(_currentScene);
                while (!operation.isDone)
                    yield return null;
                yield break;
            }

            if (_originalScene.IsValid() && !_originalScene.isLoaded)
            {
                SceneManager.LoadScene(_originalScene.name);
                yield break;
            }

            SceneManager.LoadScene(0, LoadSceneMode.Single);
            yield return null;
        }
    }
}