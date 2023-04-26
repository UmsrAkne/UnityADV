using NUnit.Framework;
using Scenes.Scripts.SceneContents;

namespace Tests.SceneContents
{
    [TestFixture]
    public class TestStopOrder
    {
        [Test]
        public void 生成テスト()
        {
            var _ = new StopOrder();
        }

        [Test]
        public void IsAnimationStopOrderTest()
        {
            Assert.IsFalse(new StopOrder().IsAnimationStopOrder(), "デフォルト状態");

            Assert.IsFalse(new StopOrder() { Name = string.Empty, Target = StoppableSceneParts.anime }
                    .IsAnimationStopOrder(),"空文字");

            Assert.IsFalse(new StopOrder() { Name = "se", Target = StoppableSceneParts.anime }
                    .IsAnimationStopOrder(), "アニメーション以外の文字");

            // 以下アニメーションの停止命令として判定される文字列
            Assert.IsTrue(new StopOrder() { Name = "alphaChanger", Target = StoppableSceneParts.anime }
                .IsAnimationStopOrder());

            Assert.IsTrue(new StopOrder() { Name = "shake", Target = StoppableSceneParts.anime }
                .IsAnimationStopOrder());

            Assert.IsTrue(new StopOrder() { Name = "slide", Target = StoppableSceneParts.anime }
                .IsAnimationStopOrder());

            Assert.IsTrue(new StopOrder() { Name = "flash", Target = StoppableSceneParts.anime }
                .IsAnimationStopOrder());

            Assert.IsTrue(new StopOrder() { Name = "bound", Target = StoppableSceneParts.anime }
                .IsAnimationStopOrder());

            Assert.IsTrue(new StopOrder() { Name = "animationChain", Target = StoppableSceneParts.anime }
                .IsAnimationStopOrder());

            Assert.IsTrue(new StopOrder() { Name = "chain", Target = StoppableSceneParts.anime }
                .IsAnimationStopOrder());

            Assert.IsTrue(new StopOrder() { Name = "scaleChange", Target = StoppableSceneParts.anime }
                .IsAnimationStopOrder());

            Assert.IsTrue(new StopOrder() { Name = "draw", Target = StoppableSceneParts.anime }.IsAnimationStopOrder());

            //　判定は大文字小文字を無視して行われるため、一部の文字が大文字に変わったりしても結果は変化しない
            Assert.IsTrue(new StopOrder() { Name = "AlphaChanger", Target = StoppableSceneParts.anime }
                .IsAnimationStopOrder());

            Assert.IsTrue(new StopOrder() { Name = "Shake", Target = StoppableSceneParts.anime }
                .IsAnimationStopOrder());

            Assert.IsTrue(new StopOrder() { Name = "Slide", Target = StoppableSceneParts.anime }
                .IsAnimationStopOrder());

            Assert.IsTrue(new StopOrder() { Name = "Flash", Target = StoppableSceneParts.anime }
                .IsAnimationStopOrder());

            Assert.IsTrue(new StopOrder() { Name = "Bound", Target = StoppableSceneParts.anime }
                .IsAnimationStopOrder());

            Assert.IsTrue(new StopOrder() { Name = "AnimationChain", Target = StoppableSceneParts.anime }
                .IsAnimationStopOrder());

            Assert.IsTrue(new StopOrder() { Name = "Chain", Target = StoppableSceneParts.anime }
                .IsAnimationStopOrder());

            Assert.IsTrue(new StopOrder() { Name = "ScaleChange", Target = StoppableSceneParts.anime }
                .IsAnimationStopOrder());

            Assert.IsTrue(new StopOrder() { Name = "Draw", Target = StoppableSceneParts.anime }.IsAnimationStopOrder());
        }
    }
}