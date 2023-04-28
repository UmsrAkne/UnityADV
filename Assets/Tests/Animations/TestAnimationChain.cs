using System;
using System.Collections.Generic;
using System.Xml.Linq;
using NUnit.Framework;
using Scenes.Scripts.Animations;
using Tests.Loaders;

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

        [Test]
        public void インターバル動作テスト()
        {
            var dummyDisplayObject = new DummyDisplayObject
            {
                Y = 0
            };

            var chain = new AnimationChain
            {
                Target = dummyDisplayObject,
                RepeatCount = 1,
                Delay = 10,
                Interval = 5,
            };

            chain.AddAnimationTag(XElement.Parse("<anime name=\"slide\" speed=\"5\" distance=\"20\" />"));
            chain.AddAnimationTag(XElement.Parse("<anime name=\"slide\" speed=\"5\" distance=\"30\" />"));

            for (int i = 0; i < 10; i++)
            {
                // Delay = 10 となっているので、最初の１０回分は動作中だが値は変化しない
                chain.Execute();
                Assert.AreEqual(0, dummyDisplayObject.Y);
                Assert.IsTrue(chain.IsWorking);
            }

            for (int i = 0; i < 15; i++)
            {
                chain.Execute();
                System.Diagnostics.Debug.WriteLine($"TestAnimationChain (178) : {dummyDisplayObject.Y}");
            }

            for (int i = 0; i < 5; i++)
            {
                // インターバルが挟まる　この区間では値は変化しない
                chain.Execute();
                System.Diagnostics.Debug.WriteLine($"TestAnimationChain (178) : {dummyDisplayObject.Y}");
                Assert.Greater(1.0, Math.Abs(dummyDisplayObject.Y) - 50.0);
                Assert.IsTrue(chain.IsWorking);
            }

            for (int i = 0; i < 30; i++)
            {
                chain.Execute(); // 最後まで実行
            }

            Assert.Greater(dummyDisplayObject.Y, 100);
            Assert.IsFalse(chain.IsWorking);
        }

        [Test]
        public void ダミー実行のテスト()
        {
            var generator = new DummyAnimationGenerator();

            var aa = new DummyAnimation() { Duration = 2, };
            var ab = new DummyAnimation() { Duration = 2, };
            var dummy = new DummyDisplayObject();

            generator.Animations.Add(aa);
            generator.Animations.Add(ab);

            var animationChain = new AnimationChain(generator)
            {
                Target = dummy
            };

            // DummyAnimationGenerator を DI したことにより、ここで入力した値に関わらず、 aa, ab, が使用される
            // アニメーション名が shake なのは、無効なアニメーションを入力できないため
            animationChain.AddAnimationTag(XElement.Parse("<anime name=\"shake\" />"));
            animationChain.AddAnimationTag(XElement.Parse("<anime name=\"shake\" />"));

            Assert.IsTrue(animationChain.IsWorking);
            Assert.IsTrue(aa.IsWorking);
            Assert.IsTrue(ab.IsWorking);

            animationChain.Execute();
            Assert.AreEqual(1, aa.ProcessCounter);

            animationChain.Execute();
            Assert.AreEqual(2, aa.ProcessCounter);
            Assert.IsFalse(aa.IsWorking);

            animationChain.Execute();
            Assert.AreEqual(1, ab.ProcessCounter);

            animationChain.Execute();
            Assert.AreEqual(2, ab.ProcessCounter);
            Assert.IsFalse(ab.IsWorking);
        }

        [Test]
        public void 同時実行のテスト()
        {
            var generator = new DummyAnimationGenerator();

            var aa = new DummyAnimation() { Duration = 2, GroupName = "a"};
            var ab = new DummyAnimation() { Duration = 2, GroupName = "a"};
            var ac = new DummyAnimation() { Duration = 2, };
            var dummy = new DummyDisplayObject();

            generator.Animations.Add(aa);
            generator.Animations.Add(ab);
            generator.Animations.Add(ac);

            var animationChain = new AnimationChain(generator)
            {
                Target = dummy
            };

            // DummyAnimationGenerator を DI したことにより、ここで入力した値に関わらず、 aa, ab, ac が使用される
            // アニメーション名が shake なのは、無効なアニメーションを入力できないため
            animationChain.AddAnimationTag(XElement.Parse("<anime name=\"shake\" />"));
            animationChain.AddAnimationTag(XElement.Parse("<anime name=\"shake\" />"));
            animationChain.AddAnimationTag(XElement.Parse("<anime name=\"shake\" />"));

            Assert.IsTrue(animationChain.IsWorking);
            Assert.IsTrue(aa.IsWorking);
            Assert.IsTrue(ab.IsWorking);

            // aa, ab は同じグループ名が指定されているので、同時に実行される。
            animationChain.Execute();
            Assert.AreEqual(1, aa.ProcessCounter);
            Assert.AreEqual(1, ab.ProcessCounter);

            animationChain.Execute();
            Assert.AreEqual(2, aa.ProcessCounter);
            Assert.AreEqual(2, ab.ProcessCounter);

            Assert.IsFalse(aa.IsWorking);
            Assert.IsFalse(ab.IsWorking);

            animationChain.Execute();
            Assert.AreEqual(1, ac.ProcessCounter);

            animationChain.Execute();
            Assert.AreEqual(2, ac.ProcessCounter);
            Assert.IsFalse(ac.IsWorking);

            animationChain.Execute();
            Assert.IsFalse(animationChain.IsWorking);
        }

        [Test]
        public void 同時実行ループのテスト()
        {
            var generator = new DummyAnimationGenerator();

            var aa = new DummyAnimation() { Duration = 2, GroupName = "a"};
            var ab = new DummyAnimation() { Duration = 2, GroupName = "a"};
            var ac = new DummyAnimation() { Duration = 2, };

            var ad = new DummyAnimation() { Duration = 2, GroupName = "a"};
            var ae = new DummyAnimation() { Duration = 2, GroupName = "a"};
            var af = new DummyAnimation() { Duration = 2, };

            var dummy = new DummyDisplayObject();

            generator.Animations.Add(aa);
            generator.Animations.Add(ab);
            generator.Animations.Add(ac);

            generator.Animations.Add(ad);
            generator.Animations.Add(ae);
            generator.Animations.Add(af);

            var animationChain = new AnimationChain(generator)
            {
                Target = dummy,
                RepeatCount = 1,
            };

            // DummyAnimationGenerator を DI したことにより、ここで入力した値に関わらず、 aa - af が使用される
            // アニメーション名が shake なのは、無効なアニメーションを入力できないため
            animationChain.AddAnimationTag(XElement.Parse("<anime name=\"shake\" />"));
            animationChain.AddAnimationTag(XElement.Parse("<anime name=\"shake\" />"));
            animationChain.AddAnimationTag(XElement.Parse("<anime name=\"shake\" />"));

            // 実行一周目
            // グループになっているアニメーションが２つあるでの、一周分の Duration は 4
            animationChain.Execute();
            animationChain.Execute();
            animationChain.Execute();
            animationChain.Execute();

            CollectionAssert.AreEqual(
                new List<bool>() { aa.IsWorking, ab.IsWorking, ac.IsWorking, ad.IsWorking, ae.IsWorking, af.IsWorking, },
                new List<bool>() { false, false, false, true, true, true }
            );

            // 実行２周目
            animationChain.Execute();
            animationChain.Execute();
            animationChain.Execute();
            animationChain.Execute();

            CollectionAssert.AreEqual(
                new List<bool>() { aa.IsWorking, ab.IsWorking, ac.IsWorking, ad.IsWorking, ae.IsWorking, af.IsWorking, },
                new List<bool>() { false, false, false, false, false, false },
                "8回分実行した時点で6個分のアニメーションは全て終了しているはず"
            );

            animationChain.Execute();
            Assert.IsFalse(animationChain.IsWorking);
        }
    }
}