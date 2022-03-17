using System.Collections;
using System.Collections.Generic;
using UnityEngine.TestRunner;
using UnityEditor.TestRunner;
using NUnit.Framework;
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

        [Test]
        public void TestExecute()
        {
            var res = new Resource();
            res.Scenarios = new List<Scenario>()
            {
                new Scenario(){Text = "abcdef" },
                new Scenario(){Text = "ghijkl" },
                new Scenario(){Text = "mnopqr" },
            };

            var writer = new TextWriter();
            writer.setResource(res);

            // ここからテスト

            writer.execute();
            Assert.AreEqual(writer.CurrentText, string.Empty);

            writer.executeEveryFrame();
            Assert.AreEqual(writer.CurrentText, "a");

            for (var i = 0; i < 10; i++)
            {
                writer.executeEveryFrame();
            }

            Assert.AreEqual(writer.CurrentText, "abcdef", "11frame 経過時の状態。テキストの全てが過不足なく入力済みか");

            writer.execute();
            Assert.AreEqual(writer.CurrentText, string.Empty);

            writer.executeEveryFrame();
            writer.executeEveryFrame();
            writer.execute();

            Assert.AreEqual(writer.CurrentText, "ghijkl", "2フレーム経過でテキスト描画を切り上げ。テキストが全て入力されているか");

            writer.executeEveryFrame();
            writer.executeEveryFrame();
            writer.executeEveryFrame();
            Assert.AreEqual(writer.CurrentText, "ghjikl", "executeEveryFrame() を余分に実行しても問題ないか？");
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
