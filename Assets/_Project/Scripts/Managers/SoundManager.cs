using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    private AudioSource audioSource;
    private AudioSource bgmSource;

    private Dictionary<string, AudioClip> soundDictionary = new();

    [SerializeField] private string resourcePath = "Sounds"; // Thư mục trong Assets/Resources/Sounds
    [SerializeField] private float bgmFadeDuration = 1.5f;

    #region Unity Methods

    public void OnAwake()
    {
        audioSource = GetComponent<AudioSource>();

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        LoadAllSounds();
    }

    public void OnStart()
    {

    }

    public void OnUpdate()
    {

    }

    #endregion

    /// <summary>
    /// Load toàn bộ AudioClip từ thư mục Resources/Sounds vào Dictionary
    /// </summary>
    private void LoadAllSounds()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>(resourcePath);

        foreach (AudioClip clip in clips)
        {
            if (!soundDictionary.ContainsKey(clip.name))
            {
                soundDictionary.Add(clip.name, clip);
            }
            else
            {
                Debug.LogWarning($"[SoundManager] ⚠️ Trùng tên clip: {clip.name}, clip này bị bỏ qua.");
            }
        }

        Debug.Log($"[SoundManager] ✅ Loaded {soundDictionary.Count} sounds from '{resourcePath}'");

        // Log ra toàn bộ key–value trong dictionary
        foreach (var kvp in soundDictionary)
        {
            string clipInfo = kvp.Value != null
                ? $"(length: {kvp.Value.length:F2}s, samples: {kvp.Value.samples}, freq: {kvp.Value.frequency})"
                : "(null)";
            Debug.Log($"[SoundManager] 🎵 Key: '{kvp.Key}' → Clip: '{kvp.Value?.name}' {clipInfo}");
        }
    }


    /// <summary>
    /// Phát âm thanh theo tên (đã được load trong Resources/Sounds)
    /// </summary>
    public void PlaySound(string soundName, float volume = 1f, float pitch = 1f)
    {
        if (soundDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(clip, volume);
            audioSource.pitch = 1f;
        }
        else
        {
            Debug.LogWarning($"[SoundManager] ❌ Không tìm thấy âm thanh '{soundName}' trong '{resourcePath}'");
        }
    }

    /// <summary>
    /// Kiểm tra có tồn tại âm thanh này không
    /// </summary>
    public bool HasSound(string soundName)
    {
        return soundDictionary.ContainsKey(soundName);
    }

    /// <summary>
    /// Lấy danh sách tất cả các sound đã load
    /// </summary>
    public IEnumerable<string> GetAllSoundNames()
    {
        return soundDictionary.Keys;
    }

    // --- Play background music (loop) ---
    public void PlayBGM(string bgmName, float volume = 1f, bool fade = true)
    {
        if (!soundDictionary.TryGetValue(bgmName, out AudioClip clip))
        {
            Debug.LogWarning($"[SoundManager] ❌ Không tìm thấy BGM '{bgmName}'");
            return;
        }

        // Nếu đang phát cùng clip → bỏ qua
        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        StopAllCoroutines();
        StartCoroutine(SwitchBGM(clip, volume, fade));
    }

    private IEnumerator SwitchBGM(AudioClip newClip, float targetVolume, bool fade)
    {
        if (fade && bgmSource.isPlaying)
        {
            // Fade out nhạc cũ
            float startVol = bgmSource.volume;
            for (float t = 0; t < bgmFadeDuration; t += Time.deltaTime)
            {
                bgmSource.volume = Mathf.Lerp(startVol, 0, t / bgmFadeDuration);
                yield return null;
            }
        }

        bgmSource.clip = newClip;
        bgmSource.Play();

        if (fade)
        {
            // Fade in nhạc mới
            for (float t = 0; t < bgmFadeDuration; t += Time.deltaTime)
            {
                bgmSource.volume = Mathf.Lerp(0, targetVolume, t / bgmFadeDuration);
                yield return null;
            }
        }

        bgmSource.volume = targetVolume;
    }

    // --- Stop background music ---
    public void StopBGM(bool fade = true)
    {
        if (!bgmSource.isPlaying) return;
        StopAllCoroutines();
        StartCoroutine(StopBGMRoutine(fade));
    }

    private IEnumerator StopBGMRoutine(bool fade)
    {
        if (fade)
        {
            float startVol = bgmSource.volume;
            for (float t = 0; t < bgmFadeDuration; t += Time.deltaTime)
            {
                bgmSource.volume = Mathf.Lerp(startVol, 0, t / bgmFadeDuration);
                yield return null;
            }
        }

        bgmSource.Stop();
        bgmSource.clip = null;
    }

    public void MuteSound(bool isMute)
    {
        audioSource.mute = isMute;
    }

    public void MuteBGM(bool isMute)
    {
        bgmSource.mute = isMute;
    }
}
