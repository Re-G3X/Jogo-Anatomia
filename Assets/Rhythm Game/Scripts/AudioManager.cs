using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneMusicData
{
    public string sceneName;
    public AudioClip music;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;

    public SceneMusicData[] sceneMusics;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneMusic(scene.name);
    }

    void PlaySceneMusic(string sceneName)
    {
        StopAllCoroutines();

        foreach (var data in sceneMusics)
        {
            if (data.sceneName == sceneName)
            {
                StartCoroutine(FadeInMusic(data.music, 1f));
                return;
            }
        }

        StartCoroutine(FadeOut(1f));
    }

    public IEnumerator FadeInMusic(AudioClip newClip, float duration)
    {
        if (musicSource.clip == newClip && musicSource.isPlaying) yield break;

        yield return StartCoroutine(FadeOut(duration));

        musicSource.clip = newClip;
        musicSource.Play();

        float targetVolume = 0.15f;
        musicSource.volume = 0;

        while (musicSource.volume < targetVolume)
        {
            musicSource.volume += Time.deltaTime / duration;
            yield return null;
        }

        musicSource.volume = targetVolume;
    }

    public IEnumerator FadeOut(float duration)
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume;
    }
}