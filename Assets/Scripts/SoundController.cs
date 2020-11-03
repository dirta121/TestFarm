using UnityEngine;
namespace TestFarm
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundController : Singleton<SoundController>
    {
        private AudioSource _audioSource;
        public AudioClip[] uiClickClips;
        public AudioClip[] coinsClips;
        public AudioClip[] musicClips;
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            PlayMusic();
        }
        public void PlayMusic()
        {
            _audioSource.loop = true;
            _audioSource.clip = GetRandomClip(musicClips);
            _audioSource.Play();
        }
        public void PlayUIClick()
        {
            _audioSource.PlayOneShot(GetRandomClip(uiClickClips));
        }
        public void PlayCoinsClip()
        {
            _audioSource.PlayOneShot(GetRandomClip(coinsClips));
        }
        public void PlayCurrentClip(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }
        public void PlayCurrentClip(AudioClip[] clips)
        {
            _audioSource.PlayOneShot(GetRandomClip(clips));
        }
        private AudioClip GetRandomClip(AudioClip[] clips)
        {
            return clips[UnityEngine.Random.Range(0, clips.Length)];
        }
    }
}