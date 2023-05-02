using System;
using NUnit.Framework;
using Scenes.Scripts.Animations;

namespace Tests.Animations
{
    public class TestSlideCoreB
    {
        [Test]
        public void GetPositionRatioテスト()
        {
            var dummy = new DummyDisplayObject();
            var slideCore = new SlideCoreB
            {
                Duration = 40,
                Distance = 100,
                Target = dummy
            };

            for (int i = 0; i < 50; i++)
            {
                slideCore.Execute();
            }

            Assert.Less(Math.Abs(dummy.X - 100.0), 0.5);
            Assert.AreEqual(0, dummy.Y);
        }
    }
}