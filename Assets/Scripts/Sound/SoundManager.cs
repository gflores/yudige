using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	static public SoundManager instance;
	public AudioSource evolution_music;
	public AudioSource evolution_sfx;
	public AudioSource death_music;
	public AudioSource death_sfx;
	public AudioSource enemy_death_music;
	public AudioSource normal_damage_sound;
	public AudioSource strong_damage_sound;
	public AudioSource weak_damage_sound;
	public AudioSource attack_start_sound;
	public AudioSource change_element_sound;
	public AudioSource remove_element_sound;
	public AudioSource battle_start_sound;

	public AudioSource current_music_played {get; set;}

	void Awake(){
		instance = this;
	}

	public void ForcePlayMusic(AudioSource music){
		PlayMusic(music);
	}
	void PlayMusic(AudioSource music, float volume = 1f)
	{
		if (current_music_played != null)
			current_music_played.Stop();
		current_music_played = music;
		music.volume = volume;
		music.Play();
	}
	public void PlayIfDifferent(AudioSource music, float volume = 1f){
		if (current_music_played != music)
			PlayMusic(music, volume);
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
	public IEnumerator LaunchVolumeFade(AudioSource music, float time, float from = 0f, float to = 1f)
	{
		float delta_volume = to - from;
		
		music.volume = from;
		if (time > 0)
		{
			for (float curr_t = 0; curr_t < time; curr_t += Time.deltaTime)
			{
				music.volume += delta_volume * (Time.deltaTime / time);
				yield return new WaitForSeconds(0.001f);
			}
		}
		music.volume = to;
	}

}
