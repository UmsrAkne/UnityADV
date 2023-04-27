using NUnit.Framework;
using Scenes.Scripts.Animations;

namespace Tests.Animations
{
    public class TestSlide
    {
        [Test]
        public void 生成テスト()
        {
            var _ = new Slide();
        }

        [Test]
        public void 通常動作テスト()
        {
            var dummy = new DummyDisplayObject();
            var slide = new Slide
            {
                Target = dummy,
                Speed = 5,
                Distance = 50
            };

            Assert.AreEqual(0,dummy.Y);
            Assert.IsTrue(slide.IsWorking);

            for (int i = 0; i < 20; i++)
            {
                slide.Execute();
            }

            Assert.Greater(dummy.Y, 50.0);
            Assert.IsFalse(slide.IsWorking);
        }

        [Test]
        public void 遅延実行テスト()
        {
            var dummy = new DummyDisplayObject();
            var slide = new Slide
            {
                Target = dummy,
                Speed = 5,
                Distance = 50,
                Delay = 10
            };

            Assert.AreEqual(0,dummy.Y);
            Assert.IsTrue(slide.IsWorking);

            for (int i = 0; i < 10; i++)
            {
                slide.Execute();
                Assert.AreEqual(0, dummy.Y);
                Assert.IsTrue(slide.IsWorking);
            }

            for (int i = 0; i < 20; i++)
            {
                slide.Execute();
            }

            Assert.Greater(dummy.Y, 50.0);
            Assert.IsFalse(slide.IsWorking);
        }
    }
}