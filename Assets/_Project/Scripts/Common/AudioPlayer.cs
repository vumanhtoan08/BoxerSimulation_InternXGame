using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public bool playOnAwake = true;
    public bool playOneShot = false;
    public bool isLoop = false;
    [Range(0f, 1f)]
    public float volume = 1f;
    public AudioClip audioClip;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!playOnAwake) return;
        Play();
        PlayOneShot(volume);
    }
    public void Play()
    {
        if (AudioManager.MusicSetting != 1 || playOneShot) return;
        audioSource.clip = audioClip;
        audioSource.loop = isLoop;
        audioSource.volume = volume;
        audioSource.Play();
    }
    public void PlayOneShot(float _volume)
    {
        if (AudioManager.SoundSetting != 1 || !playOneShot) return;
        audioSource.PlayOneShot(audioClip, _volume);
    }
    public void Stop()
    {
        audioSource.Stop();
    }
    public void AdjustVolume(float volume)
    {
        this.volume = volume;
        audioSource.volume = volume;
    }
}
