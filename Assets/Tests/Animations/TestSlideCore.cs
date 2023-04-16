using System;
using NUnit.Framework;
using Scenes.Scripts.Animations;

namespace Tests.Animations
{
    [TestFixture]
    public class TestSlideCore
    {
        [Test]
        public void 通常動作のテスト()
        {
            var displayObject = new DummyDisplayObject();
            var slideCore = new SlideCore
            {
                Speed = 1,
                Target = displayObject,
                Distance = 20,
            };

            slideCore.Start();

            LoopExecute(slideCore, 10);
            Assert.Greater(displayObject.Y, 1);

            LoopExecute(slideCore, 25);
            Assert.GreaterOrEqual(displayObject.Y, 20);

            Assert.Less(Math.Abs(displayObject.Y - 20.0), 1.0);
        }

        [Test]
        public void 通常動作長距離移動のテスト()
        {
            var displayObject = new DummyDisplayObject();
            var slideCore = new SlideCore
            {
                Speed = 10,
                Target = displayObject,
                Distance = 1000,
            };

            slideCore.Start();

            LoopExecute(slideCore, 30);
            Assert.Greater(displayObject.Y, 20.0);

            LoopExecute(slideCore, 200);
            Assert.Less(Math.Abs(displayObject.Y - 1000.0), 2.0);
        }

        private void LoopExecute(SlideCore core, int count)
        {
            for (int i = 0; i < count; i++)
            {
                core.Execute();
            }
        }
    }
}