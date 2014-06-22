using UnityEngine;
using System.Collections;

public class SpecialEffectsManager : MonoBehaviour {
	static public SpecialEffectsManager instance;
	
	public ShakeTransform normal_damage_shake;
	public ShakeTransform critical_damage_shake;
	public ShakeTransform moster_transformation_shake;
	public ShakeTransform moster_death_shake;

	void Awake(){
		instance = this;
	}
	public IEnumerator Coroutine_StartTweenAlpha(SpriteRenderer sprite, float dest_alpha, float time)
	{
		float start_alpha = sprite.color.a;
		for (float current_time = 0f; current_time < time; current_time += Time.deltaTime)
		{
			ChangeAlphaValue(sprite, Mathf.Lerp(start_alpha, dest_alpha, current_time / time));
			yield return new WaitForSeconds(0.001f);
		}
		ChangeAlphaValue(sprite, dest_alpha);
	}
	public IEnumerator Coroutine_StartTweenAlpha(SpriteRenderer sprite, float start_alpha, float dest_alpha, float time)
	{
		for (float current_time = 0f; current_time < time; current_time += Time.deltaTime)
		{
			ChangeAlphaValue(sprite, Mathf.Lerp(start_alpha, dest_alpha, current_time / time));
			yield return new WaitForSeconds(0.001f);
		}
		ChangeAlphaValue(sprite, dest_alpha);
	}
	void ChangeAlphaValue (SpriteRenderer sprite, float alpha_value)
	{
		Color col = sprite.color;
		col.a = alpha_value;
		sprite.color = col;
	}
}