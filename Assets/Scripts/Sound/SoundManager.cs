using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	static public SoundManager instance;

	public AudioSource current_music_played {get; set;}

	void Awake(){
		instance = this;
	}

	public void ForcePlayMusic(AudioSource music){
		PlayMusic(music);
	}
	void PlayMusic(AudioSource music)
	{
		if (current_music_played != null)
			current_music_played.Stop();
		current_music_played = music;
		music.Play();
	}
	public void PlayIfDifferent(AudioSource music){
		if (current_music_played != music)
			PlayMusic(music);
	}

	public void PauseAndMute()
	{
		AudioListener.pause = true;
	}
	public void UnpauseAndUnmute()
	{
		AudioListener.pause = false;
	}
	public void PlayIndependant(AudioSource sound)
	{
		sound.PlayOneShot(sound.clip, sound.volume);
		if (sound.pitch < 0)
			sound.time = sound.clip.length;
	}

}
