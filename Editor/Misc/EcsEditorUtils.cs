// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TripolisInc.EcsCore.Attributes;
using UnityEngine;

namespace TripolisInc.EcsCore.Editor
{
    public static class EcsEditorUtils
    {
        private static readonly HashSet<Type> AllComponents;
        private static readonly EcsComponentEditorInfo[] EcsComponentsInfo;
        private static readonly MD5 MD5Hasher;

        static EcsEditorUtils()
        {
            AllComponents = new HashSet<Type>();
            
            var componentAttribute = typeof(EcsComponentAttribute);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsDefined(componentAttribute, false))
                        AllComponents.Add(type);
                }
            }

            EcsComponentsInfo = AllComponents.Where(x => x.IsSerializable)
                .Select(x => new EcsComponentEditorInfo(x, x.GetAttribute<EcsComponentAttribute>())).ToArray();
            
            MD5Hasher = MD5.Create();
        }

        public static EcsComponentEditorInfo[] GetEcsEditorComponents() => EcsComponentsInfo;

        public static Color GetComponentColor(Type type)
        {
            var hash = MD5Hasher.ComputeHash(Encoding.UTF8.GetBytes(type.FullName));
            var typeHash = Math.Abs(BitConverter.ToInt32(hash, 0));
            var hue = (typeHash % 360) / 360f;
            var limiter = (typeHash & ((1 << (typeHash % 7)) - 1)) + 17;
            var sat = Mathf.Min(Mathf.Max(0.3f , (typeHash % limiter) / (float)limiter), 0.7f);
            var color = Color.HSVToRGB(hue, sat, 1f);
            color.a = 0.4f;
            return color;
        }

        public readonly struct EcsComponentEditorInfo
        {
            public readonly Type type;
            public readonly EcsComponentAttribute attribute;

            public EcsComponentEditorInfo(Type type, EcsComponentAttribute attribute)
            {
                this.type = type;
                this.attribute = attribute;
            }
        }
    }
}