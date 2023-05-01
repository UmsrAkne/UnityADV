using System;
using NUnit.Framework;
using Scenes.Scripts.Animations;

namespace Tests.Animations
{
    [TestFixture]
    public class TestScaleChange
    {
        [Test]
        public void 生成テスト()
        {
            var _ = new ScaleChange();
        }

        [Test]
        public void 通常動作テスト()
        {
            var dummy = new DummyDisplayObject();
            var scaleChanger = new ScaleChange
            {
                Target = dummy,
                To = 1.5,
                Duration = 5,
            };

            Assert.IsTrue(scaleChanger.IsWorking);
            Assert.Less(Math.Abs(dummy.Scale) - 1.0,  0.01);

            scaleChanger.Execute();
            Assert.Less(Math.Abs(dummy.Scale) - 1.1,  0.01);

            scaleChanger.Execute();
            Assert.Less(Math.Abs(dummy.Scale) - 1.2,  0.01);

            scaleChanger.Execute();
            Assert.Less(Math.Abs(dummy.Scale) - 1.3,  0.01);

            scaleChanger.Execute();
            Assert.Less(Math.Abs(dummy.Scale) - 1.4,  0.01);

            scaleChanger.Execute();
            Assert.Less(Math.Abs(dummy.Scale) - 1.5,  0.01);

            Assert.IsFalse(scaleChanger.IsWorking);

            // 停止後に実行しても動作しないことを確認する
            scaleChanger.Execute();
            scaleChanger.Execute();
            scaleChanger.Execute();
            Assert.Less(Math.Abs(dummy.Scale) - 1.5,  0.01);
            Assert.IsFalse(scaleChanger.IsWorking);
        }

        [Test]
        public void 遅延実行通常動作()
        {
            var dummy = new DummyDisplayObject();
            var scaleChanger = new ScaleChange
            {
                Target = dummy,
                To = 1.3,
                Duration = 3,
                Delay = 3,
            };

            Assert.IsTrue(scaleChanger.IsWorking);
            Assert.Less(Math.Abs(dummy.Scale) - 1.0,  0.01);

            scaleChanger.Execute();
            scaleChanger.Execute();
            scaleChanger.Execute();
            Assert.Less(Math.Abs(dummy.Scale) - 1.0,  0.01);

            scaleChanger.Execute();
            Assert.Less(Math.Abs(dummy.Scale) - 1.1,  0.01);

            scaleChanger.Execute();
            Assert.Less(Math.Abs(dummy.Scale) - 1.2,  0.01);

            scaleChanger.Execute();
            Assert.Less(Math.Abs(dummy.Scale) - 1.3,  0.01);

            Assert.IsFalse(scaleChanger.IsWorking);

            // 停止後に実行しても動作しないことを確認する
            scaleChanger.Execute();
            scaleChanger.Execute();
            scaleChanger.Execute();
            Assert.Less(Math.Abs(dummy.Scale) - 1.3,  0.01);
            Assert.IsFalse(scaleChanger.IsWorking);
        }
    }
}