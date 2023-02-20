// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TripolisInc.EcsCore.Misc
{
    public static class EcsPackageExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
        {
            if (go == null)
                return null;

            var component = go.GetComponent<T>();
            if (component != null)
                return component;

            return go.AddComponent<T>();
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> enumerable,
            Func<TSource, TKey> selector) => enumerable.GroupBy(selector, x => x).Select(x => x.First());
    }
}