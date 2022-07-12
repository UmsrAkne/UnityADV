namespace Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using NUnit.Framework;
    using SceneContents;
    using SceneParts;
    using UnityEngine.TestTools;

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
                new Scenario() { Text = "abcdef" },
                new Scenario() { Text = "ghijkl" },
                new Scenario() { Text = "mnopqr" }
            };

            var writer = new TextWriter();
            writer.SetResource(res);

            /// ここからテスト

            writer.Execute();
            Assert.AreEqual(writer.CurrentText, string.Empty);

            writer.ExecuteEveryFrame();
            Assert.AreEqual(writer.CurrentText, "a");

            for (var i = 0; i < 10; i++)
            {
                writer.ExecuteEveryFrame();
            }

            Assert.AreEqual(writer.CurrentText, "abcdef", "11frame 経過時の状態。テキストの全てが過不足なく入力済みか");

            writer.Execute();
            Assert.AreEqual(writer.CurrentText, string.Empty);

            writer.ExecuteEveryFrame();
            writer.ExecuteEveryFrame();
            writer.Execute();

            Assert.AreEqual(writer.CurrentText, "ghijkl", "2フレーム経過でテキスト描画を切り上げ。テキストが全て入力されているか");

            writer.ExecuteEveryFrame();
            writer.ExecuteEveryFrame();
            writer.ExecuteEveryFrame();
            Assert.AreEqual(writer.CurrentText, "ghijkl", "executeEveryFrame() を余分に実行しても問題ないか？");
        }

        [Test]
        public void シナリオのカウンターのテスト()
        {
            var res = new Resource();
            res.Scenarios = new List<Scenario>()
            {
                new Scenario() { Text = "abcdef" },
                new Scenario() { Text = "ghijkl" },
                new Scenario() { Text = "mnopqr" }
            };

            var writer = new TextWriter();
            writer.SetResource(res);

            Assert.AreEqual(writer.ScenarioIndex, 0);
            writer.Execute();

            Assert.AreEqual(writer.ScenarioIndex, 0);
            writer.Execute();
            writer.Execute();

            Assert.AreEqual(writer.ScenarioIndex, 1);

            writer.Execute();
            writer.Execute();
            Assert.AreEqual(writer.ScenarioIndex, 2);
        }

        [Test]
        public void 特定のインデックスまでジャンプするテスト()
        {
            var res = new Resource();
            res.Scenarios = new List<Scenario>()
            {
                new Scenario() { Text = "abcdef" },
                new Scenario() { Text = "ghijkl" },
                new Scenario() { Text = "mnopqr" },
                new Scenario() { Text = "stuvwx" }
            };

            var writer = new TextWriter();
            writer.SetResource(res);

            /// ここからテスト

            writer.SetScenarioIndex(2);
            Assert.AreEqual(writer.CurrentText, string.Empty);

            writer.ExecuteEveryFrame();
            Assert.AreEqual(writer.CurrentText, "m");

            for (var i = 0; i < 10; i++)
            {
                writer.ExecuteEveryFrame();
            }

            Assert.AreEqual(writer.CurrentText, "mnopqr", "10frame 経過時の状態。テキストの全てが過不足なく入力済みか");

            writer.Execute();
            Assert.AreEqual(writer.CurrentText, string.Empty);

            writer.ExecuteEveryFrame();
            writer.ExecuteEveryFrame();
            writer.Execute();

            Assert.AreEqual(writer.CurrentText, "stuvwx", "2フレーム経過でテキスト描画を切り上げ。テキストが全て入力されているか");
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
