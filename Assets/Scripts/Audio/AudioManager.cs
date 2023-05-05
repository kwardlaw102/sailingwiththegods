using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	[SerializeField] private Sound[] sounds = null;
	//[SerializeField] private static AudioManager instance = null;
	[SerializeField] private AudioMixerGroup mixerGroup = null;
	[SerializeField] private AudioMixer mixer = null;

	private const float MUTE_VOLUME_DB = -80f;
	private const float NORMAL_VOLUME_DB = 0f;

	void Awake() {
		Globals.Register(this);

		///////////////////////////////////////////////////////////////////////////
		///To make sure that the game doesn't create two audio managers by mistake.
		/*if(instance == null) {
			instance = this;
		}
		else {
			Destroy(gameObject);
			return;
		}
		/////////////////////////////////////////////////////////////////////////////////

		/*This funtion is here so that when the game loads to a new scene,
		 * the audio doesn't skip, repeat, or stop. The audio for, say music, is continuous.*/
		//DontDestroyOnLoad(gameObject);


		//This foreach loop is for adding all the variables to the audio source
		foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.outputAudioMixerGroup = mixerGroup; //this is to output the audio to the mixer

			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
			s.source.playOnAwake = s.onAwake;
		}
	}


	//This function plays a sound
	public void PlaySound(string name) {
		Sound s = Array.Find(sounds, sound => sound.name == name);

		s.source.Play();
	}


	//this function is to stop a sound from playing
	public void StopSound(string name) {
		Sound s = Array.Find(sounds, sound => sound.name == name);
		s.source.Stop();

		if (s == null) {
			Debug.LogWarning("Sound:  " + name + " not found!");
			return;
		}
	}

	public void MuteMainGameAudio() {
		Debug.Log("Mute main game");
		mixer.SetFloat("musicVolume", MUTE_VOLUME_DB);
		mixer.SetFloat("backgroundVolume", MUTE_VOLUME_DB);
	}

	public void UnmuteMainGameAudio() {
		Debug.Log("Unmute main game");
		mixer.SetFloat("musicVolume", NORMAL_VOLUME_DB);
		mixer.SetFloat("backgroundVolume", NORMAL_VOLUME_DB);
	}
}
