using UnityEngine;
using System.Collections;

public class SoundRepetition : MonoBehaviour {
	public AudioSource main_sound;
	public AudioSource[] alternative_sounds;
	public float repetition_delay = 0.5f;
	public int random_range_min = 2;
	public int random_range_max = 4;
	
	bool is_active = false;
	
	public void MakeLaunchSoundRepetition()
	{
		if (is_active == false)
		{
			is_active = true;
			StartCoroutine("LaunchSoundRepetition");
		}
	}
	IEnumerator LaunchSoundRepetition()
	{
		while (true)
		{
			int sound_count = Random.Range(random_range_min, random_range_max);
			
			for (int i = 0; i != sound_count; ++i)
			{
				SoundManager.instance.PlayIndependant(main_sound);
				yield return new WaitForSeconds(repetition_delay);
			}
			if (alternative_sounds.Length != 0)
			{
				SoundManager.instance.PlayIndependant(alternative_sounds[Random.Range(0, alternative_sounds.Length)]);
				yield return new WaitForSeconds(repetition_delay);
			}
		}
	}
	
	public void StopMakeSoundRepetition()
	{
		if (is_active == true)
		{
			is_active = false;
			StopCoroutine("LaunchSoundRepetition");		
		}
	}
}
