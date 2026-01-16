using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour {

    public static BGMController instance;
    // Start is called before the first frame update

    public AudioSource bgmAudioSource;

    [SerializeField]
    private bool startedLoop = false;

    [SerializeField]
    private bool canPlay = false;

    private float baseVolume;

    [SerializeField]
    private int defaultEaseOut = 3;
    private int bgmEaseOut;
    private Coroutine easeOut;

    [SerializeField]
    private AudioClip introTrack;

    [SerializeField]
    private AudioClip loopedTrack;

    void Awake() {
        this.SetInstance();
        this.bgmEaseOut = this.defaultEaseOut;
        this.baseVolume = this.bgmAudioSource.volume;
    }

    private void SetInstance() {
        if(instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }


    void LateUpdate() {
        this.ManageTracks();
    }

    private void ManageTracks() {
        if (this.bgmAudioSource == null || !this.canPlay) {
            return;
        }

        if (!this.bgmAudioSource.isPlaying && !this.startedLoop) {
            this.bgmAudioSource.clip = this.loopedTrack;
            this.bgmAudioSource.Play();
            this.bgmAudioSource.loop = true;
            this.startedLoop = true;
        }
    }

    void SetPauseMenu() {
        
    }
    #nullable enable

    private void ResetAudioClips() {
        if(this.bgmAudioSource == null) {
            return;
        }
        this.loopedTrack = null;
        this.introTrack = null;
        this.canPlay = false;
        this.startedLoop = false;
        this.bgmAudioSource.loop = false;
        Debug.Log("resetting audioclips");
    }

    #nullable enable
    public void SetAudioClip(AudioClip primary, AudioClip? intro = null) {
        if(easeOut != null) {
            StopCoroutine(easeOut);
            easeOut = null;
        }
        
        this.ResetAudioClips();

        // if(this.loopedTrack == primary) {
        //     return;
        // }

        if (bgmAudioSource == null) {
            Debug.LogError("No BGM Audio source");
            return;
        }

        this.ResetVolume();
        this.loopedTrack = primary;
        this.introTrack = intro;
        try {
            if (this.introTrack != null) {
                this.bgmAudioSource.clip = this.introTrack;
                this.bgmAudioSource.Play();
                return;
            }

            if (this.loopedTrack != null) {
                this.bgmAudioSource.clip = this.loopedTrack;
                this.bgmAudioSource.Play();
                return;
            }
        } finally {
            this.canPlay = true;
        }
    }

    #nullable disable

    public float GetVolume() {
        return bgmAudioSource.volume;
    }

    public void SetVolume(float newVolume) {
        bgmAudioSource.volume = newVolume;
    }

    public void ResetVolume() {
        bgmAudioSource.volume = baseVolume;
    }

    #nullable enable
    public void StopAudio(System.Action? callback, int? easeOutOverride = null) {
        if (easeOutOverride != null) {
            this.bgmEaseOut = (int) easeOutOverride!;
        }
        easeOut = StartCoroutine(EaseOutAudioAndStop(callback));
    }

    private IEnumerator EaseOutAudioAndStop(System.Action? callback = null) {
        while(bgmAudioSource.volume > 0.01f) {
            bgmAudioSource.volume -= Time.deltaTime / bgmEaseOut;
            yield return null;
        }

        this.StopAudio();
        if (callback != null) {
            callback();
        }
    }

    public void StopAudio() {
        bgmAudioSource.volume = 0;
        bgmAudioSource.Stop();
        bgmAudioSource.clip = null;
        this.bgmEaseOut = this.defaultEaseOut;
    }

    #nullable disable
}
