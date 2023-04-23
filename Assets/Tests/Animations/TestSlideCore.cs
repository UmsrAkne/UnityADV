using System;
using NUnit.Framework;
using Scenes.Scripts.Animations;
using UnityEngine;

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

            LoopExecute(slideCore, 40);
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

            for (int i = 0; i < 120; i++)
            {
                slideCore.Execute();
                System.Diagnostics.Debug.WriteLine($"TestSlideCore (54) : {displayObject.Y}");
            }
            
            Assert.Less(Math.Abs(displayObject.Y - 1000.0), 2.0);
        }

        [Test]
        public void 通常動作長距離移動低速度のテスト()
        {
            var displayObject = new DummyDisplayObject();
            var slideCore = new SlideCore
            {
                Speed = 1,
                Target = displayObject,
                Distance = 500,
            };

            slideCore.Start();

            for (int i = 0; i < 850; i++)
            {
                slideCore.Execute();
                System.Diagnostics.Debug.WriteLine($"TestSlideCore (75) : {displayObject.Y}");
            }

            Assert.Less(Math.Abs(displayObject.Y - 500.0), 1.0);
        }

        [Test]
        public void GetSinXTest()
        {
            var core = new SlideCore();
            Assert.AreEqual(core.GetCustomSinX(0), 0);
            Assert.Less(Math.Abs(core.GetCustomSinX(30) - 1.0), 0.01);
            Assert.Less(Math.Abs(core.GetCustomSinX(90) - 2.0), 0.01);
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