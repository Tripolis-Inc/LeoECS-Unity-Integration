// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System.Linq;

namespace TripolisInc.Test.Runtime.Helpers
{
    public static class TestConstants
    {
        public const string PrefabPath = 
            "Packages/com.tripolis.inc.ecspackage/Tests/Runtime/TestResources/EntityPrefab.prefab";

        public static class Scenes
        {
            public const string EmptyScene = 
                "Packages/com.tripolis.inc.ecspackage/Tests/Runtime/TestScenes/TestEmptyScene.unity"; 
            
            public const string ECS_TEST =
                "Packages/com.tripolis.inc.ecspackage/Tests/Runtime/TestScenes/TestEcsScene.unity";

            public static string GetName(string scenePath) => scenePath.Split("/").Last().Replace(".unity", "");
        }
    }
}