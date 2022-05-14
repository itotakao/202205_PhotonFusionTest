using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public enum AudioMixerGroupType
    {
        Master,
        BGM,
        Ambient,
        SE,
        Voice,
    }

    [DefaultExecutionOrder(-100)]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Current { get; private set; }

        public static readonly float MasterVolumeRate = 1;
        public static readonly float BGMVolumeRate = 0.2f;
        public static readonly float AmbientVolumeRate = 0.3f;
        public static readonly float SEVolumeRate = 1;
        public static readonly float VoiceVolumeRate = 1;

        public SoundManager BGM;
        public SoundManager Ambient;
        public SoundManager SE;
        public SoundManager Voice;

        [SerializeField]
        private AudioMixer audioMixer;
        private readonly Dictionary<AudioMixerGroupType, AudioMixerGroup> mixerGroupDict = new Dictionary<AudioMixerGroupType, AudioMixerGroup>();

        public AudioMixerGroup GetAudioMixerGroup(AudioMixerGroupType mixerGroupType) { return mixerGroupDict[mixerGroupType]; }

        private void Awake()
        {
            Current = this;
        }

        private void OnValidate()
        {
            SoundManager[] sounds = GetComponentsInChildren<SoundManager>();

            foreach (SoundManager s in sounds)
            {
                if (s.name.Contains("BGM"))
                {
                    BGM = s;
                }
                else if (s.name.Contains("Ambient"))
                {
                    Ambient = s;
                }
                else if (s.name.Contains("SE"))
                {
                    SE = s;
                }
                else if (s.name.Contains("Voice"))
                {
                    Voice = s;
                }
            }
        }

        private void Start()
        {
            InitializeAudioMixer();
        }

        private void InitializeAudioMixer()
        {
            foreach (AudioMixerGroupType mixerGroupType in Enum.GetValues(typeof(AudioMixerGroupType)))
            {
                string mixerGroupTypeName = mixerGroupType.ToString();

                AudioMixerGroup audioMixerGroup = audioMixer.FindMatchingGroups(mixerGroupTypeName)[0];
                mixerGroupDict[mixerGroupType] = audioMixerGroup;

                //SettingFloat volume = (SettingFloat)typeof(Settings.Audio.MixerVolume).GetField(mixerGroupTypeName).GetValue(null);
                var volume = (float)typeof(AudioManager).GetField(mixerGroupTypeName + "VolumeRate").GetValue(null);
                float db = Mathf.Clamp(Mathf.Log10(volume) * 20.0f, -80.0f, 20.0f);
                audioMixer.SetFloat(mixerGroupTypeName + ".Volume", db); // ミキサー側でこの名前でパラメータをエクスポーズしておくこと

                Debug.Log(string.Format("{0} のボリュームを {1:F3} dB ({2:F3}) に設定しました", mixerGroupType, db, volume));
            }
        }
    }
}
