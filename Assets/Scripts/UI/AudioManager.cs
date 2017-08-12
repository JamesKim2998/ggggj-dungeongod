using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static float BGMvolume = 0.5f, SFXvolume = 1f;
    public AudioSource BGM;
    public List<AudioClip> SFXs;

    private void Awake()
    {
        BGM = Camera.main.GetComponent<AudioSource>();
        BGM.volume = BGMvolume;
        BGM.Play();
    }

    public void setBGM(AudioClip audio)
    {
        BGM.clip = audio;
        BGM.Play();
    }

    public static IEnumerator playSFX(AudioSource source, AudioClip audio)
    {
        source.clip = audio;
        source.volume = SFXvolume;
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
        Destroy(source);
    }
}
