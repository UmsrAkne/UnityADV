using System.Xml.Linq;
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

            chain.AddAnimationTag(XElement.Parse("<anime name=\"alphaChanger\" amount=\"0.1\" />"));
            chain.AddAnimationTag(XElement.Parse("<anime name=\"slide\" speed=\"5\" distance=\"200\" />"));

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

        [Test]
        public void ループ動作テスト()
        {
            var dummyDisplayObject = new DummyDisplayObject
            {
                Y = 0
            };

            var chain = new AnimationChain
            {
                Target = dummyDisplayObject,
                RepeatCount = 1,
            };

            chain.AddAnimationTag(XElement.Parse("<anime name=\"slide\" speed=\"5\" distance=\"100\" />"));
            chain.AddAnimationTag(XElement.Parse("<anime name=\"slide\" speed=\"5\" distance=\"200\" />"));

            for (int i = 0; i < 300; i++)
            {
                chain.Execute();
            }

            Assert.Greater(dummyDisplayObject.Y, 600);
            Assert.IsFalse(chain.IsWorking);
        }

        [Test]
        public void ループ動作テスト２回()
        {
            var dummyDisplayObject = new DummyDisplayObject
            {
                Y = 0
            };

            var chain = new AnimationChain
            {
                Target = dummyDisplayObject,
                RepeatCount = 2,
            };

            chain.AddAnimationTag(XElement.Parse("<anime name=\"slide\" speed=\"5\" distance=\"100\" />"));
            chain.AddAnimationTag(XElement.Parse("<anime name=\"slide\" speed=\"5\" distance=\"200\" />"));

            for (int i = 0; i < 350; i++)
            {
                chain.Execute();
                System.Diagnostics.Debug.WriteLine($"TestAnimationChain (94) : {dummyDisplayObject.Y}");
            }

            Assert.Greater(dummyDisplayObject.Y, 900);
            Assert.IsFalse(chain.IsWorking);
        }

        [Test]
        public void 遅延実行テスト()
        {
            var dummyDisplayObject = new DummyDisplayObject();
            dummyDisplayObject.Alpha = 0;

            var chain = new AnimationChain
            {
                Target = dummyDisplayObject,
                Delay = 10,
            };

            chain.AddAnimationTag(XElement.Parse("<anime name=\"alphaChanger\" amount=\"0.1\" />"));
            chain.AddAnimationTag(XElement.Parse("<anime name=\"slide\" speed=\"5\" distance=\"200\" />"));

            // 初期状態
            Assert.AreEqual(dummyDisplayObject.Alpha, 0);
            Assert.AreEqual(0, dummyDisplayObject.Y);

            for (int i = 0; i < 10; i++)
            {
                // Delay = 10 となっているので、最初の１０回分は動作中だが値は変化しない
                chain.Execute();
                Assert.AreEqual(dummyDisplayObject.Alpha, 0);
                Assert.AreEqual(0, dummyDisplayObject.Y);
                Assert.IsTrue(chain.IsWorking);
            }

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