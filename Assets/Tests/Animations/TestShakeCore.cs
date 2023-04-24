using NUnit.Framework;
using Scenes.Scripts.Animations;

namespace Tests.Animations
{
    public class TestShakeCore
    {
        [Test]
        public void 生成テスト()
        {
            var _ = new ShakeCore();
        }

        [Test]
        public void 普通に揺らす()
        {
            var dummy = new DummyDisplayObject();
            var core = new ShakeCore
            {
                Target = dummy,
                Duration = 8,
                Strength = 20
            };

            core.Execute();
            Assert.Greater(dummy.X, 10);
            Assert.Greater(dummy.Y, 10);
            System.Diagnostics.Debug.WriteLine($"TestShakeCore (41) : x={dummy.X},y={dummy.Y}");

            core.Execute();
            Assert.Less(dummy.X, -10);
            Assert.Less(dummy.Y, -10);
            System.Diagnostics.Debug.WriteLine($"TestShakeCore (41) : x={dummy.X},y={dummy.Y}");

            core.Execute();
            Assert.Greater(dummy.X, 10);
            Assert.Greater(dummy.Y, 10);
            System.Diagnostics.Debug.WriteLine($"TestShakeCore (41) : x={dummy.X},y={dummy.Y}");

            core.Execute();
            Assert.Less(dummy.X, -10);
            Assert.Less(dummy.Y, -10);
            System.Diagnostics.Debug.WriteLine($"TestShakeCore (41) : x={dummy.X},y={dummy.Y}");

            core.Execute();
            Assert.Greater(dummy.X, 5);
            Assert.Greater(dummy.Y, 5);
            System.Diagnostics.Debug.WriteLine($"TestShakeCore (41) : x={dummy.X},y={dummy.Y}");

            core.Execute();
            Assert.Less(dummy.X, -5);
            Assert.Less(dummy.Y, -5);
            System.Diagnostics.Debug.WriteLine($"TestShakeCore (41) : x={dummy.X},y={dummy.Y}");

            core.Execute();
            Assert.Greater(dummy.X, 0);
            Assert.Greater(dummy.Y, 0);
            System.Diagnostics.Debug.WriteLine($"TestShakeCore (41) : x={dummy.X},y={dummy.Y}");

            core.Execute();
            Assert.AreEqual(dummy.X, 0);
            Assert.AreEqual(dummy.Y, 0);
            System.Diagnostics.Debug.WriteLine($"TestShakeCore (41) : x={dummy.X},y={dummy.Y}");
        }

        [Test]
        public void 縦揺れ()
        {
            var dummy = new DummyDisplayObject();
            var core = new ShakeCore
            {
                Target = dummy,
                Duration = 6,
                Strength = 20
            };
        }
    }
}