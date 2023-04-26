using System;
using System.Collections.Generic;
using System.Linq;
using Scenes.Scripts.SceneContents;

namespace Scenes.Scripts.SceneParts
{
    public class BgvPlayer : IScenarioSceneParts
    {
        public bool NeedExecuteEveryFrame => true;
        private List<ISound> voices;
        private ISound playingVoice;
        private Dictionary<string,ISound> bgVoicesByName;
        private bool mute = true;
        private double volume = 1.0;

        private VoicePlayer VoicePlayer { get; }

        private double Volume
        {
            get => volume;
            set
            {
                if (value >= 0 && value <= 1.0)
                {
                    if (playingVoice != null)
                    {
                        playingVoice.Volume = value;
                    }

                    volume = value;
                }
            }
        }

        private bool Playing => voices != null && voices.Any();

        public BgvPlayer(VoicePlayer voicePlayer)
        {
            VoicePlayer = voicePlayer;
            VoicePlayer.SoundComplete += VoicePlayerCompleteEventHandler;
            VoicePlayer.SoundStart += VoicePlayerStartEventHandler;
        }

        public void Execute()
        {
        }

        public void ExecuteEveryFrame()
        {
            if (!Playing)
            {
                return;
            }

            if (!playingVoice.IsPlaying)
            {
                // 再生中の音声が終了したら、リスト上で次の音声があれば再生、なければシャッフルして先頭から再生する。
                var currentIndex = voices.IndexOf(playingVoice);
                if (currentIndex == voices.Count -1)
                {
                    voices = voices.OrderBy(_ => Guid.NewGuid()).ToList();
                    Play(voices.First());
                }
                else
                {
                    Play(voices[currentIndex]);
                }
            }

            if (mute)
            {
                return;
            }

            Volume += 0.02;
        }

        public void SetScenario(Scenario scenario)
        {
            var bgvOrder = scenario.BGVOrders.FirstOrDefault(vo => vo.Channel == VoicePlayer.Channel);
            if (bgvOrder != null)
            {
                playingVoice?.Stop();
                voices = bgvOrder.FileNames.Select(n => bgVoicesByName[n]).OrderBy(_ => Guid.NewGuid()).ToList();
                Play(voices.FirstOrDefault());
            }

            scenario.StopOrders.ForEach(order =>
            {
                if (order.Target == StoppableSceneParts.backgroundVoice || order.Target == StoppableSceneParts.bgv)
                {
                    if (order.Channel == VoicePlayer.Channel)
                    {
                        mute = true;
                        Volume = 0;
                        voices = new List<ISound>();
                        playingVoice?.Stop();
                    }
                }
            });
        }

        public void SetResource(Resource resource)
        {
            bgVoicesByName = resource.BGVoicesByName;
        }

        private void VoicePlayerStartEventHandler(object sender, EventArgs e)
        {
            mute = true;
            Volume = 0;
        }

        private void VoicePlayerCompleteEventHandler(object sender, EventArgs e)
        {
            mute = false;
        }

        private void Play(ISound v)
        {
            playingVoice = v;
            v.Play();
        }

        public void SetUI(UI ui)
        {
        }
    }
}