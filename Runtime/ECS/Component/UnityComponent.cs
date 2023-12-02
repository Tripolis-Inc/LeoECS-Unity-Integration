// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

namespace TripolisInc.EcsCore.Component
{
    public struct UnityComponent<T> where T : UnityEngine.Component
    {
        public const string ComponentFieldName = nameof(unityComponent);

        public T unityComponent;
    }
}