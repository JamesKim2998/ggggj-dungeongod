using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static float BGMvolume = 0.5f, SFXvolume = 0.5f;
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

	/*
    static IEnumerator playSFX(AudioSource source, AudioClip audio)
    {
        source.clip = audio;
        source.volume = SFXvolume;
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
        Destroy(source);
    }
	*/

    static IEnumerator playSFX(AudioSource source, float delay = 0)
    {
		yield return delay;
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
        Destroy(source.gameObject);
    }

	public static void playSFX(MonoBehaviour target, AudioClip clip, float delay = 0, float volume = 1)
	{
		var tempGO = new GameObject("Audio: " + clip.name);
		var coroutineRunner = tempGO.AddComponent<ObjectTag>();
		tempGO.transform.position = target.transform.position;
		var audioSource = tempGO.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.spatialBlend = 0.9f;
		audioSource.maxDistance = 8;
		audioSource.volume = volume * SFXvolume;
		coroutineRunner.StartCoroutine(playSFX(audioSource, delay));
	}

	public static void playSFX(Vector3 position, AudioClip clip, float delay = 0, float volume = 1)
	{
		var tempGO = new GameObject("Audio: " + clip.name);
		var coroutineRunner = tempGO.AddComponent<ObjectTag>();
		tempGO.transform.position = position;
		var audioSource = tempGO.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.spatialBlend = 0.9f;
		audioSource.maxDistance = 8;
		audioSource.volume = volume * SFXvolume;
		coroutineRunner.StartCoroutine(playSFX(audioSource, delay));
	}
}
