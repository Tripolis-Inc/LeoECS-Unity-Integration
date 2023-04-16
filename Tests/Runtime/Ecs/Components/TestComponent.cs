using System;
using TripolisInc.EcsCore.Attributes;

namespace Tests.Runtime.Ecs.Components
{
    [Serializable]
    [EcsComponent("Test Component")]
    public struct TestComponent
    {
        public string test;
    }
}