using System.Collections;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource Source;

        private float fadeVelocity;
        private float maxVolume;

        [SerializeField]
        private AudioMixerGroupType audioType;
        private ISoundPlayer soundPlayerType;

        private void Awake()
        {
            soundPlayerType = SoundPlayerFactory.Create(audioType);
        }

        private void OnValidate()
        {
            Source = GetComponent<AudioSource>();
            if (gameObject.name.Contains(AudioMixerGroupType.BGM.ToString()))
            {
                audioType = AudioMixerGroupType.BGM;
            }

            if (gameObject.name.Contains(AudioMixerGroupType.Ambient.ToString()))
            {
                audioType = AudioMixerGroupType.Ambient;
            }

            if (gameObject.name.Contains(AudioMixerGroupType.SE.ToString()))
            {
                audioType = AudioMixerGroupType.SE;
            }

            if (gameObject.name.Contains(AudioMixerGroupType.Voice.ToString()))
            {
                audioType = AudioMixerGroupType.Voice;
            }
        }

        private void Start()
        {
            Source.outputAudioMixerGroup = AudioManager.Current.GetAudioMixerGroup(soundPlayerType.GroupType);
        }

        public bool IsPlaying(string clipName)
        {
            return Source.clip.name == clipName && Source.isPlaying && Source.volume > 0.0f && fadeVelocity >= 0.0f;
        }

        public void Play(string clipName)
        {
            transform.position = Vector3.zero;

            StartCoroutine(PlayAsync(clipName));
        }

        public void Play(string clipName, Vector3 toPos)
        {
            transform.position = toPos;

            StartCoroutine(PlayAsync(clipName));
        }

        public IEnumerator PlayAsync(string clipName, Vector3 toPos)
        {
            transform.position = toPos;

            yield return StartCoroutine(PlayAsync(clipName));
        }

        public IEnumerator PlayAsync(string clipName)
        {
            ResourceRequest req = Resources.LoadAsync<AudioClip>(soundPlayerType.GroupType.ToString() + "/" + clipName);
            Debug.Log(soundPlayerType.GroupType.ToString() + "/" + clipName);
            yield return req;
            Source.Stop();
            Play((AudioClip)req.asset);
        }

        public void Play(AudioClip clip)
        {
            if (Source.clip == clip && Source.isPlaying && Source.volume > 0.0f && fadeVelocity >= 0.0f) { return; }

            maxVolume = soundPlayerType.Volume;
            fadeVelocity = 1.0f / soundPlayerType.FadeInSec;

            Source.Stop();

            Source.clip = clip;
            Source.volume = soundPlayerType.FadeInSec == 0.0f ? maxVolume : 0.0f;

            Source.Play();
        }

        public void Stop()
        {
            fadeVelocity = -1.0f / soundPlayerType.FadeOutSec;
        }

        private void Update()
        {
            Source.volume = Mathf.Clamp(Source.volume + Time.deltaTime * fadeVelocity, 0.0f, maxVolume);
        }
    }
}