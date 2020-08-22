﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlaylist : MonoBehaviour
{
    public static MusicPlaylist Instance;

    [SerializeField] AudioClip[] audioClips;
    [SerializeField] AudioClip bossFightMusic;
    [SerializeField] AudioClip afterBossMusic;

    int currTrackIndex = 0;
    AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!audioSource.isPlaying)
        {
            currTrackIndex = (currTrackIndex + 1) % audioClips.Length;
            audioSource.clip = audioClips[currTrackIndex];
            audioSource.Play();
            GameData.Instance.saveData.currSongIndex = currTrackIndex;
            GameData.Instance.Save();
        }
    }
    public IEnumerator FadeOutAtBossFight(float fadeTime)
    {
        yield return StartCoroutine(FadeOut(fadeTime));

        audioSource.clip = bossFightMusic;
        audioSource.Play();
    }
    public IEnumerator FadeOutAfterBoss(float fadeTime)
    {
        yield return StartCoroutine(FadeOut(fadeTime));

        audioSource.clip = afterBossMusic;
        audioSource.Play();
    }

    IEnumerator FadeOut(float fadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public int GetCurrentSongIndex()
    {
        return currTrackIndex;
    }

    public void PlaySongWithIndex(int index)
    {
        currTrackIndex = index;
        audioSource.Stop();
        audioSource.clip = audioClips[currTrackIndex];
        audioSource.Play();
    }
}