namespace Tests
{
    using System.Collections.Generic;
    using Assets.Tests.SceneContents;
    using NUnit.Framework;
    using SceneContents;
    using SceneParts;

    public class TestVoicePlayer
    {
        [Test]
        public void ボイス再生のテストパターン１()
        {
            //// 再生 -> 再生終了 -> 再生 -> 再生終了

            int completeCounter = 0;

            var voicePlayer = new VoicePlayer();
            voicePlayer.SoundComplete += (sender, e) => completeCounter++;

            var scenario1 = new Scenario();
            scenario1.VoiceOrders.Add(new VoiceOrder() { FileName = "sound1" });

            var scenario2 = new Scenario();
            scenario2.VoiceOrders.Add(new VoiceOrder() { FileName = "sound2" });

            var sound1 = new DummySound() { Duration = 3 };
            var sound2 = new DummySound() { Duration = 3 };

            var resource = new Resource();
            resource.VoicesByName = new Dictionary<string, ISound>()
            {
                { "sound1", sound1 },
                { "sound2", sound2 }
            };

            voicePlayer.SetResource(resource);

            voicePlayer.SetScenario(scenario1); // 最初のシナリオを投入
            voicePlayer.Execute();

            Assert.IsTrue(sound1.IsPlaying);
            Assert.IsFalse(sound2.IsPlaying);

            sound1.Forward(1); // 1秒送る
            voicePlayer.ExecuteEveryFrame();
            Assert.IsTrue(sound1.IsPlaying);

            sound1.Forward(2); // 2秒送る
            voicePlayer.ExecuteEveryFrame();
            Assert.IsFalse(sound1.IsPlaying);
            Assert.AreEqual(completeCounter, 1);

            voicePlayer.SetScenario(scenario2); // 文章送り 
            voicePlayer.Execute();

            Assert.IsFalse(sound1.IsPlaying);
            Assert.IsTrue(sound2.IsPlaying);

            sound2.Forward(1);
            voicePlayer.ExecuteEveryFrame();

            Assert.IsFalse(sound1.IsPlaying);
            Assert.IsTrue(sound2.IsPlaying);

            sound2.Forward(2);
            voicePlayer.ExecuteEveryFrame();

            Assert.IsFalse(sound1.IsPlaying);
            Assert.IsFalse(sound2.IsPlaying);
            Assert.AreEqual(completeCounter, 2);
        }

        [Test]
        public void ボイス再生のテストパターン２()
        {
            //// 再生 -> 途中まで再生 -> 次を再生 -> 再生終了

            int completeCounter = 0;

            var voicePlayer = new VoicePlayer();
            voicePlayer.SoundComplete += (sender, e) => completeCounter++;

            var scenario1 = new Scenario();
            scenario1.VoiceOrders.Add(new VoiceOrder() { FileName = "sound1" });

            var scenario2 = new Scenario();
            scenario2.VoiceOrders.Add(new VoiceOrder() { FileName = "sound2" });

            var sound1 = new DummySound() { Duration = 3 };
            var sound2 = new DummySound() { Duration = 3 };

            var resource = new Resource();
            resource.VoicesByName = new Dictionary<string, ISound>()
            {
                { "sound1", sound1 },
                { "sound2", sound2 }
            };

            voicePlayer.SetResource(resource);

            voicePlayer.SetScenario(scenario1); // 最初のシナリオを投入
            voicePlayer.Execute();

            Assert.IsTrue(sound1.IsPlaying);
            Assert.IsFalse(sound2.IsPlaying);

            sound1.Forward(1); // 1秒送る
            voicePlayer.ExecuteEveryFrame();
            Assert.IsTrue(sound1.IsPlaying);

            voicePlayer.SetScenario(scenario2); // 現在サウンドが再生中だが、次を投入
            voicePlayer.Execute();

            Assert.IsFalse(sound1.IsPlaying, "sound2 が再生状態、sound1 は停止しているはず");
            Assert.IsTrue(sound2.IsPlaying);

            sound2.Forward(1);
            voicePlayer.ExecuteEveryFrame();

            Assert.IsFalse(sound1.IsPlaying);
            Assert.IsTrue(sound2.IsPlaying);

            sound2.Forward(2);
            voicePlayer.ExecuteEveryFrame();

            Assert.IsFalse(sound1.IsPlaying);
            Assert.IsFalse(sound2.IsPlaying, "sound2 が再生終了");
            Assert.AreEqual(completeCounter, 1, "再生終了イベントは一回しか飛んでないはず");
        }
    }
}
