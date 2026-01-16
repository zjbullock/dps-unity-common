using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public List<AudioSource> audioSources;

    [Range(0f, 1f)]
    public float baseVolume;


    public int bgmEaseOut = 2;


    private Coroutine easeOut;

    void Awake() {

    }

    void Start() {
        audioSources = new List<AudioSource>(GetComponents<AudioSource>());

        baseVolume = audioSources[0].volume;
    }


    void FixedUpdate() {

    }

    void SetPauseMenu() {
        
    }

    public void PlayAudio(SoundEffectEnums audioLocation) {

        // if(easeOut != null) {
        //     StopCoroutine(easeOut);
        //     easeOut = null;
        // }
        string audioName = audioLocation + "";
        audioName = GeneralUtilsStatic.EnumStringCleaner(audioName);
        AudioClip audioClip = Resources.Load(Constants.Audio.SFX + audioName) as AudioClip;
        foreach(AudioSource audioSource in this.audioSources) {
            if(audioSource != null && audioSource.clip == null) {
                audioSource.PlayOneShot(audioClip, baseVolume);
                return;
            }
        }
        // if(audioSources != null && audioClip != null) {
        //     Debug.Log("Playing: " + audioLocation);
        //     audioSources[1].PlayOneShot(audioClip, baseVolume);
        // }
    }

    public void PlayAudio(AudioClip audioClip) {

        // if(easeOut != null) {
        //     StopCoroutine(easeOut);
        //     easeOut = null;
        // }
        foreach(AudioSource audioSource in this.audioSources) {
            if(audioSource != null) {
                audioSource.PlayOneShot(audioClip, baseVolume);
                return;
            }
        }
        // if(audioSources != null && audioClip != null) {
        //     Debug.Log("Playing: " + audioLocation);
        //     audioSources[1].PlayOneShot(audioClip, baseVolume);
        // }
    }

    #nullable enable
    public AudioClip? GetPrimaryAudioClip() {
        return audioSources[0].clip;
    }

    public void SetAudioClipPrimarySource(AudioClip audioClip) {
        if(easeOut != null) {
            StopCoroutine(easeOut);
            easeOut = null;
        }
        if(audioSources != null && audioClip != null) {
            Debug.Log("Primary Audio Source: " + audioClip);
            audioSources[0].volume = baseVolume / 4;
            audioSources[0].priority = 1;
            audioSources[0].clip = audioClip;
            audioSources[0].loop = true;
            audioSources[0].Play();
        }
    }

    public void StopAudioClip(int audioSource = 0) {
        audioSources[audioSource].Stop();
        audioSources[audioSource].clip = null;
        audioSources[audioSource].volume = baseVolume;
    }
    

    public void StopAllAudio(int audioSource = 0) {
        easeOut = StartCoroutine(EaseOutAudioAndStop(audioSource));
    }

    private IEnumerator EaseOutAudioAndStop(int audioSource = 0) {
        while(audioSources[audioSource].volume > 0.01f) {
            audioSources[audioSource].volume -= Time.deltaTime / bgmEaseOut;
            yield return null;
        }

        audioSources[audioSource].volume = 0;
        audioSources[audioSource].Stop();
        audioSources[audioSource].volume = baseVolume;
    }

}
