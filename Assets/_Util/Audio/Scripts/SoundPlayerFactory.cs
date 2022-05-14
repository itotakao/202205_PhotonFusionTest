using System.ComponentModel;

namespace Audio
{
    public static class SoundPlayerFactory
    {
        public static ISoundPlayer Create(AudioMixerGroupType type)
        {
            switch (type)
            {
                case AudioMixerGroupType.Master:
                    return new MasterPlayer();
                case AudioMixerGroupType.BGM:
                    return new BGMPlayer();
                case AudioMixerGroupType.Ambient:
                    return new AmbientPlayer();
                case AudioMixerGroupType.SE:
                    return new SEPlayer();
                case AudioMixerGroupType.Voice:
                    return new VoicePlayer();
            }

            throw new InvalidEnumArgumentException();
        }
    }
}