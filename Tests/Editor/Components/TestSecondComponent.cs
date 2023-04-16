// using System;
// using System.Collections.Generic;
// using TripolisInc.EcsCore.Attributes;
// using UnityEngine;
// using Vector3 = System.Numerics.Vector3;
//
// namespace TripolisInc.Test.EcsCore
// {
//     [Serializable]
//     [EcsComponent("Test Second", "Test/Second Test")]
//     public struct TestSecondComponent
//     {
//         public string testStr;
//         public int testInt;
//         public float testFloat;
//         public Vector3 testVector;
//         public GameObject testGO;
//         public Collider[] testArray;
//         public List<Transform> testList;
//         public NestedStruct testSerializedObj;
//
//         [Header("Test Header")]
//         [SerializeField]
//         private string testPrivate;
//
//         [HideInInspector]
//         public int testHidden;
//
//         [Serializable]
//         public struct NestedStruct
//         {
//             public string value1;
//             public int value2;
//         }
//     }
// }