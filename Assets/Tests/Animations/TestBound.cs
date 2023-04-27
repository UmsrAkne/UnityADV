using NUnit.Framework;
using Scenes.Scripts.Animations;

namespace Tests.Animations
{
    [TestFixture]
    public class TestBound
    {
        [Test]
        public void 通常実行テスト()
        {
            var dummy = new DummyDisplayObject() { X = 0 };
            var bound = new Bound
            {
                Strength = 10,
                Duration = 4,
                Target = dummy,
            };

            Assert.IsTrue(bound.IsWorking);
            Assert.Zero(dummy.X);

            bound.Execute();
            Assert.AreEqual(10, dummy.X);

            bound.Execute();
            Assert.AreEqual(20, dummy.X);

            bound.Execute();
            Assert.AreEqual(10, dummy.X);

            bound.Execute();
            Assert.Zero(dummy.X);

            bound.Execute();
            Assert.Zero(dummy.X);
            Assert.IsFalse(bound.IsWorking);
        }

        [Test]
        public void 遅延実行テスト()
        {
            var dummy = new DummyDisplayObject() { X = 0 };
            var bound = new Bound
            {
                Strength = 10,
                Duration = 4,
                Target = dummy,
                Delay = 3,
            };

            Assert.IsTrue(bound.IsWorking);
            Assert.Zero(dummy.X);

            bound.Execute();
            bound.Execute();
            bound.Execute();
            Assert.Zero(dummy.X);

            bound.Execute();
            Assert.AreEqual(10, dummy.X);

            bound.Execute();
            Assert.AreEqual(20, dummy.X);

            bound.Execute();
            Assert.AreEqual(10, dummy.X);

            bound.Execute();
            Assert.Zero(dummy.X);

            bound.Execute();
            Assert.Zero(dummy.X);
            Assert.IsFalse(bound.IsWorking);
        }

        [Test]
        public void インターバル実行テスト()
        {
            var dummy = new DummyDisplayObject() { X = 0 };
            var bound = new Bound
            {
                Strength = 10,
                Duration = 4,
                Target = dummy,
                Delay = 3,
                Interval = 3,
                RepeatCount = 1,
            };

            Assert.IsTrue(bound.IsWorking);
            Assert.Zero(dummy.X);

            // Delay 区間
            bound.Execute();
            bound.Execute();
            bound.Execute();
            Assert.Zero(dummy.X);

            // 実際に動作する区間
            bound.Execute();
            Assert.AreEqual(10, dummy.X);

            bound.Execute();
            Assert.AreEqual(20, dummy.X);

            bound.Execute();
            Assert.AreEqual(10, dummy.X);

            bound.Execute();
            Assert.Zero(dummy.X);

            // Interval 区間
            bound.Execute();
            bound.Execute();
            bound.Execute();
            Assert.Zero(dummy.X);

            // ２回目の実行区間
            bound.Execute();
            System.Diagnostics.Debug.WriteLine($"TestBound (117) : {dummy.X}");
            Assert.NotZero(dummy.X);

            bound.Execute();
            System.Diagnostics.Debug.WriteLine($"TestBound (121) : {dummy.X}");
            Assert.NotZero(dummy.X);

            bound.Execute();
            Assert.NotZero(dummy.X);

            // 動作終了
            bound.Execute();
            Assert.Zero(dummy.X);
            Assert.IsFalse(bound.IsWorking);
        }
    }
}