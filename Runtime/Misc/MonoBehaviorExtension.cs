// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using TripolisInc.EcsCore.Interfaces;

namespace TripolisInc.EcsCore.Misc
{
    public static class MonoBehaviorExtension
    {
        public static bool IsAlive(this IMonoBehavior obj) => obj != null && obj.HasInstance();
    }
}