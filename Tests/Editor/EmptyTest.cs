// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.Editor
{
    public class EmptyTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void FirstEmptyTestSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator FirstEmptyTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
