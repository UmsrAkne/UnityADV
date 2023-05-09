using NUnit.Framework;
using Scenes.Scripts.Animations;

namespace Tests.Animations
{
    public class TestAnimationCounter
    {
        [Test]
        public void 無条件カウントアップ()
        {
            var counter = new AnimationCounter();

            Assert.Zero(counter.ExecutionCounter);
            Assert.Zero(counter.TotalExecutionCounter);
            Assert.IsTrue(counter.IsWorking);
            Assert.IsFalse(counter.CanProcess, "countUp() 実行前の状態ではまだ判定がなされていないので false");

            counter.CountUp();
            Assert.AreEqual(1, counter.ExecutionCounter);
            Assert.AreEqual(1, counter.TotalExecutionCounter);
            Assert.IsTrue(counter.IsWorking);
            Assert.IsTrue(counter.CanProcess);

            counter.CountUp();
            Assert.AreEqual(2, counter.ExecutionCounter);
            Assert.AreEqual(2, counter.TotalExecutionCounter);
            Assert.IsTrue(counter.IsWorking);
            Assert.IsTrue(counter.CanProcess);
        }

        [Test]
        public void Durationありのカウントアップ()
        {
            var counter = new AnimationCounter
            {
                Duration = 3
            };

            counter.CountUp();
            counter.CountUp();
            counter.CountUp();

            Assert.AreEqual(0, counter.ExecutionCounter, "Duration の回数分実行されたのでリセットされているはず");
            Assert.AreEqual(3, counter.TotalExecutionCounter, "Duration の影響を受けずカウントされている");
            Assert.IsFalse(counter.IsWorking, "Duration の回数分実行されたので停止する");
            Assert.IsFalse(counter.CanProcess, "Duration の回数分実行されたので処理も停止");
        }

        [Test]
        public void Repeatありのカウントアップ()
        {
            var counter = new AnimationCounter
            {
                Duration = 3,
                RepeatCount = 1,
            };

            counter.CountUp();
            counter.CountUp();
            counter.CountUp();

            Assert.AreEqual(0, counter.ExecutionCounter, "Duration の回数分実行されたのでリセットされているはず");
            Assert.AreEqual(3, counter.TotalExecutionCounter, "Duration の影響を受けずカウントされている");
            Assert.IsTrue(counter.IsWorking, "2週目に突入するのでまだ動作中");
            Assert.IsTrue(counter.CanProcess, "2週目に突入するのでまだ動作中");

            counter.CountUp();
            counter.CountUp();
            counter.CountUp();

            Assert.AreEqual(0, counter.ExecutionCounter, "Duration の回数分実行されたのでリセットされているはず");
            Assert.AreEqual(6, counter.TotalExecutionCounter, "Duration の影響を受けずカウントされている");
            Assert.IsFalse(counter.IsWorking, "2週目のカウントアップ終了後に停止する");
            Assert.IsFalse(counter.CanProcess, "2週目のカウントアップ終了後に停止する");
        }

        [Test]
        public void Delayありのカウントアップ()
        {
            var counter = new AnimationCounter
            {
                Delay = 2,
                Duration = 2,
            };

            // Delay の回数分実行する。
            counter.CountUp();
            Assert.IsFalse(counter.CanProcess);
            Assert.Zero(counter.ExecutionCounter, "Delay で処理が止まっている間はカウントは増えない");

            counter.CountUp();
            Assert.IsFalse(counter.CanProcess);
            Assert.Zero(counter.ExecutionCounter, "Delay で処理が止まっている間はカウントは増えない");

            Assert.Zero(counter.Delay, "Delay を全て消化");

            counter.CountUp();
            Assert.IsTrue(counter.CanProcess, "Delay を消費したあとは、処理実行可能になる。");

            counter.CountUp();
            Assert.IsFalse(counter.CanProcess, "Duration の回数分実行したので停止");
        }

        [Test]
        public void Intervalありのカウントアップ()
        {
            var counter = new AnimationCounter
            {
                Delay = 2,
                Interval = 3,
                Duration = 2,
                RepeatCount = 1,
            };

            counter.CountUp();
            counter.CountUp();
            Assert.Zero(counter.Delay);

            counter.CountUp();
            Assert.IsTrue(counter.CanProcess);

            counter.CountUp();
            Assert.IsTrue(counter.CanProcess);

            counter.CountUp();
            Assert.Zero(counter.ExecutionCounter, "Interval 消化中");
            Assert.IsFalse(counter.CanProcess);

            counter.CountUp();
            Assert.Zero(counter.ExecutionCounter, "Interval 消化中");
            Assert.IsFalse(counter.CanProcess);

            counter.CountUp();
            Assert.Zero(counter.ExecutionCounter, "Interval 消化中");
            Assert.IsFalse(counter.CanProcess);

            counter.CountUp();
            Assert.AreEqual(1, counter.ExecutionCounter);
            Assert.IsTrue(counter.CanProcess);

            counter.CountUp();
            Assert.Zero(counter.ExecutionCounter, "Duration の回数分に達したのでリセット");
            Assert.IsFalse(counter.CanProcess);
        }
    }
}