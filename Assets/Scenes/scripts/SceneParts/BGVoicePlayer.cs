namespace SceneParts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using SceneContents;

    public class BGVoicePlayer : IScenarioSceneParts
    {
        // 一度音声の再生を開始したら、100ms は ExecuteEveryFrame() の処理をしない。
        // この待機時間を設けないと、AudioSource.IsPlaying の仕様により、音声が多重再生される。
        private readonly int playWaitTime = 100;

        private BGVOrder currentOrder;
        private ISound playingVoice;
        private List<ISound> playingVoiceList;
        private bool playRequest;
        private int channel;
        private Dictionary<string, ISound> bgVoicesByName;
        private double volume;
        private int playingVoiceIndex;
        private Stopwatch playElapsedTimer = new Stopwatch();
        private bool increasingVolume;
        private VoicePlayer voicePlayer;
        private StopOrder stopOrder;

        public BGVoicePlayer(VoicePlayer voicePlayer)
        {
            channel = voicePlayer.Channel;
            this.voicePlayer = voicePlayer;
        }

        public bool NeedExecuteEveryFrame => true;

        public double Volume
        {
            get => volume;
            private set
            {
                if (playingVoice != null)
                {
                    playingVoice.Volume = value;
                }

                volume = value;
            }
        }

        public void Execute()
        {
            if (stopOrder != null)
            {
                playingVoice.Stop();
                playingVoice = null;
                playingVoiceList = null;
                playingVoiceIndex = 0;
                Volume = 0;
                stopOrder = null;
            }

            if (!playRequest)
            {
                return;
            }

            playingVoiceList = currentOrder.FileNames.Select(name => bgVoicesByName[name]).OrderBy(_ => Guid.NewGuid()).ToList();
            playingVoice = playingVoiceList.First();
            playElapsedTimer.Restart();
        }

        public void ExecuteEveryFrame()
        {
            if (increasingVolume && volume < 1.0)
            {
                Volume += 0.02;
            }

            if (playingVoice == null || playingVoice.IsPlaying || playElapsedTimer.ElapsedMilliseconds <= playWaitTime)
            {
                return;
            }

            playingVoiceIndex++;

            if (playingVoiceList.Count <= playingVoiceIndex)
            {
                playingVoiceIndex = 0;

                if (playingVoiceList.Count >= 3)
                {
                    var exceptLast = playingVoiceList.GetRange(0, playingVoiceList.Count - 1);
                    exceptLast = exceptLast.OrderBy(_ => Guid.NewGuid()).ToList();
                    exceptLast.Insert(new Random().Next(1, exceptLast.Count - 1), playingVoiceList.Last());

                    playingVoiceList = exceptLast;
                }
            }

            playingVoice = playingVoiceList[playingVoiceIndex];
            playingVoice.Volume = Volume;
            playingVoice.Play();
            playElapsedTimer.Restart();
        }

        public void SetResource(Resource resource)
        {
            bgVoicesByName = resource.BGVoicesByName;
        }

        public void SetScenario(Scenario scenario)
        {
            scenario.BGVOrders.ForEach(order =>
            {
                if (order.Channel == channel)
                {
                    currentOrder = order;
                    playRequest = true;
                    voicePlayer.SoundComplete += VoiceCompleteEventHandler;
                }
            });

            scenario.StopOrders.ForEach(order =>
            {
                if (order.Target == StoppableSceneParts.backgroundVoice || order.Target == StoppableSceneParts.bgv)
                {
                    if (order.Channel == channel)
                    {
                        stopOrder = order;
                    }
                }
            });

            if (scenario.VoiceOrders.Any(o => o.Channel == channel))
            {
                // 同一チャンネルの音声再生の指示がある場合は、このプレイヤーの音量は 0 に変更
                Volume = 0;
                increasingVolume = false;
            }
        }

        public void SetUI(UI ui)
        {
        }

        private void VoiceCompleteEventHandler(object sender, EventArgs e)
        {
            if (playRequest)
            {
                increasingVolume = true;
            }

            voicePlayer.SoundComplete -= VoiceCompleteEventHandler;
            playRequest = false;
        }
    }
}
