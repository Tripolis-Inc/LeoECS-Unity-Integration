// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System;
using UnityEditor;

namespace TripolisInc.EcsCore.Editor
{
    public static class EditorUtils
    {
        public static SerializedObjectChange ChangeSerializeObject(SerializedObject target) =>
            new SerializedObjectChange(target);

        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            return (T) Attribute.GetCustomAttribute(type, typeof(T));
        }

        public struct SerializedObjectChange : IDisposable
        {
            private SerializedObject targetObject;

            public SerializedObjectChange(SerializedObject target)
            {
                targetObject = target;
                targetObject.Update();
            }
            
            public void Dispose()
            {
                targetObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(targetObject.targetObject);
                targetObject = null;
            }
        }
    }
}