using NUnit.Framework;
using Scenes.Scripts.Animations;

namespace Tests.Animations
{
    public class TestAnimationChain
    {
        [Test]
        public void 生成テスト()
        {
            var _ = new AnimationChain();
        }

        [Test]
        public void 通常動作テスト()
        {
            var dummyDisplayObject = new DummyDisplayObject();
            dummyDisplayObject.Alpha = 0;

            var chain = new AnimationChain
            {
                Target = dummyDisplayObject
            };
;
            chain.AddAnimation(new AlphaChanger()
            {
                Amount = 0.1,
            });

            chain.AddAnimation(new Slide()
            {
                Speed = 5,
                Distance = 200,
            });

            for (int i = 0; i < 10; i++)
            {
                chain.Execute();
            }

            Assert.Greater(dummyDisplayObject.Alpha, 1.0, "AlphaChanger を 10回実行したので Alpha > 1.0 となっている。");
            Assert.AreEqual(0, dummyDisplayObject.Y, "Slide は動作待ち状態なので、 Y は変化していない");
            Assert.IsTrue(chain.IsWorking, "AlphaChanger は終了したが、 Slide は残っているので IsWorking == true");

            for (int i = 0; i < 100; i++)
            {
                chain.Execute();
            }

            Assert.Greater(dummyDisplayObject.Alpha, 1.0, "AlphaChanger は停止しているので、一度目のチェックと同じ値");
            Assert.Greater(dummyDisplayObject.Y, 200);
            Assert.IsFalse(chain.IsWorking);
        }
    }
}