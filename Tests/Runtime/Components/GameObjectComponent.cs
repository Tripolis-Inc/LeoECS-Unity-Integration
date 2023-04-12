using System;
using TripolisInc.EcsCore.Attributes;
using UnityEngine;

namespace TripolisInc.EcsCore.Test
{
    [Serializable]
    [EcsComponent(nameof(GameObjectComponent))]
    public struct GameObjectComponent
    {
        public GameObject gameObject;
    }
}