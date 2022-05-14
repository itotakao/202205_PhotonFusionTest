namespace Audio
{
    public interface ISoundPlayer
    {
        AudioMixerGroupType GroupType { get; }
        float Volume { get; }
        float FadeInSec { get; }
        float FadeOutSec { get; }
    }
}

namespace Audio
{
    public sealed class MasterPlayer : ISoundPlayer
    {
        public AudioMixerGroupType GroupType => AudioMixerGroupType.Master;

        public float Volume => 1.0f;

        public float FadeInSec => 0.0f;

        public float FadeOutSec => 0.1f;
    }
}

namespace Audio
{
    public sealed class BGMPlayer : ISoundPlayer
    {
        public AudioMixerGroupType GroupType => AudioMixerGroupType.BGM;

        public float Volume => 1.0f;

        public float FadeInSec => 0.0f;

        public float FadeOutSec => 0.1f;
    }
}

namespace Audio
{
    public sealed class AmbientPlayer : ISoundPlayer
    {
        public AudioMixerGroupType GroupType => AudioMixerGroupType.Ambient;

        public float Volume => 1.0f;

        public float FadeInSec => 0.0f;

        public float FadeOutSec => 0.1f;
    }
}

namespace Audio
{
    public sealed class SEPlayer : ISoundPlayer
    {
        public AudioMixerGroupType GroupType => AudioMixerGroupType.SE;

        public float Volume => 1.0f;

        public float FadeInSec => 0.0f;

        public float FadeOutSec => 0.0f;
    }
}

namespace Audio
{
    public sealed class VoicePlayer : ISoundPlayer
    {
        public AudioMixerGroupType GroupType => AudioMixerGroupType.Voice;

        public float Volume => 1.0f;

        public float FadeInSec => 0.0f;

        public float FadeOutSec => 0.0f;
    }
}