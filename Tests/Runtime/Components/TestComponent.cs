using System;
using TripolisInc.EcsCore.Attributes;

namespace TripolisInc.Test.EcsCore.Component
{
    [Serializable]
    [EcsComponent("Test Component")]
    public struct TestComponent
    {
        public string test;
    }
}