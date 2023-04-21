using System;
using NUnit.Framework;
using Scenes.Scripts.Animations;

namespace Tests.Animations
{
    [TestFixture]
    public class TestFlash
    {
        [Test]
        public void 通常動作テスト()
        {
            var dummy = new DummyDisplayObject();
            var flash = new Flash()
            {
                EffectImageSet = dummy,
                Duration = 20,
            };

            for (int i = 0; i < 10; i++)
            {
                flash.Execute();
            }

            Assert.Less(Math.Abs(dummy.Alpha) - 1.0, 0.01);

            for (int i = 0; i < 11; i++)
            {
                flash.Execute();
            }

            Assert.Less(Math.Abs(dummy.Alpha), 0.01);
        }

        [Test]
        public void Delayテスト()
        {
            var dummy = new DummyDisplayObject();
            dummy.Alpha = 0;

            var flash = new Flash()
            {
                EffectImageSet = dummy,
                Duration = 20,
                Delay = 10,
            };

            for (int i = 0; i < 10; i++)
            {
                flash.Execute();
            }

            Assert.Less(Math.Abs(dummy.Alpha), 0.01);

            for (int i = 0; i < 10; i++)
            {
                flash.Execute();
            }

            Assert.Less(Math.Abs(dummy.Alpha) - 1.0, 0.01);

            for (int i = 0; i < 11; i++)
            {
                flash.Execute();
            }

            Assert.Less(Math.Abs(dummy.Alpha), 0.01);
        }

        [Test]
        public void Intervalテスト()
        {
            var dummy = new DummyDisplayObject();
            dummy.Alpha = 0;

            var flash = new Flash()
            {
                EffectImageSet = dummy,
                Duration = 10,
                Interval = 10,
                RepeatCount = 3,
            };

            for (int i = 0; i < 5; i++)
            {
                flash.Execute();
                System.Diagnostics.Debug.WriteLine($"TestFlash (99) : {dummy.Alpha}");
            }

            Assert.Less(Math.Abs(dummy.Alpha) - 1.0, 0.01);

            for (int i = 0; i < 5; i++)
            {
                flash.Execute();
                System.Diagnostics.Debug.WriteLine($"TestFlash (99) : {dummy.Alpha}");
            }

            for (int i = 0; i < 10; i++)
            {
                flash.Execute();
                System.Diagnostics.Debug.WriteLine($"TestFlash (99) : {dummy.Alpha}");
                Assert.Less(Math.Abs(dummy.Alpha), 0.01);
            }

            Assert.Less(Math.Abs(dummy.Alpha) - 1.0, 0.01);


            for (int i = 0; i < 5; i++)
            {
                flash.Execute();
            }

            Assert.Less(Math.Abs(dummy.Alpha) - 1.0, 0.01);

            for (int i = 0; i < 5; i++)
            {
                flash.Execute();
            }

            Assert.Less(Math.Abs(dummy.Alpha), 0.01);
        }
    }
}