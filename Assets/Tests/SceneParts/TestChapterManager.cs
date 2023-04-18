namespace Tests
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using Scenes.Scripts.SceneContents;
    using Scenes.Scripts.SceneParts;

    public class TestChapterManager
    {
        [Test]
        public void 生成テスト()
        {
            var chapterManager = new ChapterManager();
        }

        [Test]
        public void GetNextChapterIndexTest()
        {
            var chapterManager = new ChapterManager();
            var scenarios = new List<Scenario>()
            {
                new Scenario() { Index = 1, },
                new Scenario() { Index = 2, },
                new Scenario() { Index = 3, ChapterName = "testChapter" },
                new Scenario() { Index = 4, },
                new Scenario() { Index = 5, ChapterName = "testChapter2" },
                new Scenario() { Index = 6, },
            };

            chapterManager.SetResource(new Resource() { Scenarios = scenarios });

            chapterManager.SetScenario(scenarios[0]);
            var firstIndex = chapterManager.GetNextChapterIndex();
            Assert.AreEqual(2, firstIndex, "最初のチャプターのインデックスは 2");
            Assert.AreEqual("testChapter", scenarios[firstIndex].ChapterName);

            chapterManager.SetScenario(scenarios[firstIndex]);
            var secondIndex = chapterManager.GetNextChapterIndex();
            Assert.AreEqual(4, secondIndex, "２つ目のチャプターのインデックスは 4");
            Assert.AreEqual("testChapter2", scenarios[secondIndex].ChapterName);

            chapterManager.SetScenario(scenarios[secondIndex]);
            Assert.AreEqual(5, chapterManager.GetNextChapterIndex(), "次のチャプターは存在しないため、 secondIndex(4) + 1 が取得できる");
        }

        [Test]
        public void GetNextChapterIndexTestチャプターなし()
        {
            var chapterManager = new ChapterManager();
            var scenarios = new List<Scenario>()
            {
                new Scenario() { Index = 1, },
                new Scenario() { Index = 2, },
                new Scenario() { Index = 3, },
                new Scenario() { Index = 4, },
            };

            chapterManager.SetResource(new Resource() { Scenarios = scenarios });

            chapterManager.SetScenario(scenarios[0]);
            Assert.AreEqual(1, chapterManager.GetNextChapterIndex());
        }
    }
}