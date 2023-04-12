// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TripolisInc.Test.Runtime;
using TripolisInc.Test.Runtime.Helpers;
using UnityEditor;
using UnityEditor.TestTools;

[assembly:TestPlayerBuildModifier(typeof(BuildModifier))]

namespace TripolisInc.Test.Runtime
{
    public class BuildModifier : ITestPlayerBuildModifier
    {
        private static EditorBuildSettingsScene[] sceneLists;

        public BuildPlayerOptions ModifyOptions(BuildPlayerOptions playerOptions)
        {
            var attributeList = new List<LoadSceneAttribute>();
            var currentAssembly = Assembly.GetExecutingAssembly();
            foreach (var type in currentAssembly.GetTypes())
            {
                foreach (var methodInfo in type.GetMethods())
                    attributeList.AddRange(methodInfo.GetCustomAttributes<LoadSceneAttribute>());
            }

            var scenePathsList = new HashSet<string>();
            scenePathsList.UnionWith(attributeList.Select(x => x.path));

            playerOptions.scenes = playerOptions.scenes.Union(scenePathsList).ToArray();
            return playerOptions;
        }
    }
}

#endif