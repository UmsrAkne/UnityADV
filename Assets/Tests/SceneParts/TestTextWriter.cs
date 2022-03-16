using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestTextWriter
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestTextWriterSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        public void TestExecute()
        {
            // var res = new Resource();
            // res.Scenarios = new List<Scenario>()
            // {
            //     new Scenario(){Text = "abcdef" },
            //     new Scenario(){Text = "ghijkl" },
            //     new Scenario(){Text = "mnopqr" },
            //     new Scenario(){Text = "stuvwx" }
            // };
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestTextWriterWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
