using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	public static Player instance;
	public SequentialEventValidate death_scene_action_sequence;
	public MosterData current_moster {get; set;}

	public int life_bonus_per_karma;
	public int shield_bonus_per_karma;
	public int element_affinity_bonus_per_karma;

	public int base_shield {get; set;}
	public int[] base_element_affinities{get;set;}

	public int current_life {get; set;}
	public int current_karma {get; set;}
	public float time_lived {get; set;}
	public bool is_living_time_passing {get; set;}

//	public bool can_evolve {get; set;}
	float _time_passed_before_decrease_life = 0f;

	void Awake () {
		instance = this;
		base_element_affinities = new int[(int)Element.Count];
	}

	void Update()
	{
		if (is_living_time_passing == true)
		{
			time_lived += Time.deltaTime;
			_time_passed_before_decrease_life += Time.deltaTime;
			if (current_moster.time_before_life_decrease != 0f && _time_passed_before_decrease_life >= current_moster.time_before_life_decrease)
			{
				_time_passed_before_decrease_life = 0f;
				current_life -= 1;
				CheckDeath();
			}
		}
	}

	public int GetEffectiveMaxShield()
	{
		return base_shield + current_moster.shield_modifier;
	}

	public int GetEffectiveElementAffinity(Element element)
	{
		return base_element_affinities[(int)element] + current_moster.element_affinity_modifiers[(int)element];
	}

	public void EvolveTo(MosterData new_moster)
	{
		StartCoroutine(Coroutine_EvolutionScene(new_moster));
	}
	public IEnumerator Coroutine_EvolutionScene(MosterData new_moster)
	{
		Debug.LogWarning("EVOLVING TO: " + new_moster.moster_name);
		StateManager.instance.current_states.Add(StateManager.State.SCRIPTED_EVENT);
		StateManager.instance.UpdateFromStates();
		CameraManager.instance.SetColorToBehindPlane(Color.black);
//		TweenAlpha.Begin(CameraManager.instance.exploration_plane_behind_player_animation., 2f, 1f);
		Debug.LogWarning("waiting");
		StartCoroutine(SoundManager.instance.LaunchVolumeFade(
			SoundManager.instance.current_music_played,
			2f,
			SoundManager.instance.current_music_played.volume,
			0f
		));

		yield return StartCoroutine(SpecialEffectsManager.instance.Coroutine_StartTweenAlpha(
			CameraManager.instance.exploration_plane_behind_player_animation.GetComponent<SpriteRenderer>(),
			0f, 0.85f, 2f));
		SoundManager.instance.PlayIfDifferent(SoundManager.instance.evolution_music);
		yield return new WaitForSeconds(1f);

		//yield return StartCoroutine(CameraManager.instance.COROUTINE_ExplorationPlaneBehindToOpaque(2f));
		SoundManager.instance.PlayIndependant(SoundManager.instance.evolution_sfx);

		SpecialEffectsManager.instance.moster_transformation_shake.time_ratio = 1 / 4f;
		StartCoroutine(SpecialEffectsManager.instance.moster_transformation_shake.LaunchShake(PlayerExploration.instance.visuals_transform));
		PlayerExploration.instance.generic_animator.SetBool("IsSick", true);
		yield return new WaitForSeconds(2f);
		CameraManager.instance.SetColorToFadePlane(Color.white);
		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToOpaque(2f));
		PlayerExploration.instance.generic_animator.SetBool("IsSick", false);
		//apply change
		MostersManager.instance.AddToEvolved(current_moster);
		time_lived = 0f;
		ApplyEvolutionChanges(new_moster);
		//return to exploration normal state
//		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToTransparent(3f));
		CameraManager.instance.SetColorToBehindPlane(Color.white);
		StartCoroutine(SoundManager.instance.LaunchVolumeFade(
			SoundManager.instance.current_music_played,
			1f,
			SoundManager.instance.current_music_played.volume,
			0f
		));
		yield return StartCoroutine(CameraManager.instance.COROUTINE_ExplorationPlaneBehindToTransparent(1f));
		GameManager.instance.current_screen.MakeGoTo();
		StateManager.instance.current_states.Remove(StateManager.State.SCRIPTED_EVENT);
		StateManager.instance.UpdateFromStates();
	}
	public void ApplyEvolutionChanges(MosterData new_moster)
	{
		current_life += new_moster.life_bonus;
		current_moster = new_moster;
		RefreshMoster();
	}

	public bool CheckDeath()
	{
		if (current_life <= 0)
		{
			current_life = 0;
			LaunchDeath();
			return true;
		}
		return false;
	}

	public void LaunchDeath()
	{
		Debug.LogWarning("DEAD");
		SoundManager.instance.PlayIfDifferent(SoundManager.instance.death_music);
		StartCoroutine(Coroutine_LaunchDeathRoutine());
	}
	IEnumerator Coroutine_LaunchDeathRoutine()
	{
		StateManager.instance.current_states.Add(StateManager.State.SCRIPTED_EVENT);
		StateManager.instance.UpdateFromStates();

		if (StateManager.instance.current_states.Contains(StateManager.State.BATTLE))
		{
			yield return StartCoroutine(BattleManager.instance.Coroutine_PlayerBattleDeathRoutine());
			BattleManager.instance.InteruptBattleCoroutines();
			CameraManager.instance.battle_gui_camera.enabled = false;
			yield return StartCoroutine(Coroutine_DeathFadeScene(
				CameraManager.instance.battle_plane_behind_player_animation,
				PlayerBattle.instance.main_renderer,
				PlayerBattle.instance.visuals_transform
				));
		}
		else if (StateManager.instance.current_states.Contains(StateManager.State.EXPLORATION))
		{
			yield return StartCoroutine(Coroutine_DeathFadeScene(
				CameraManager.instance.exploration_plane_behind_player_animation,
				PlayerExploration.instance.main_renderer,
				PlayerExploration.instance.visuals_transform
			));
		}
		StateManager.instance.current_states.Remove(StateManager.State.SCRIPTED_EVENT);
		StateManager.instance.UpdateFromStates();
	}
	IEnumerator Coroutine_DeathFadeScene(Animation behind_plane_animation, SpriteRenderer player_main_renderer, Transform visuals_transform)
	{
		float behind_plane_alpha_value = 0.85f;

		behind_plane_animation.GetComponent<SpriteRenderer>().color = Color.black;
//		CameraManager.instance.SetColorToBehindPlane(Color.black);//too
		//		TweenAlpha.Begin(CameraManager.instance.exploration_plane_behind_player_animation., 2f, 1f);
		Debug.LogWarning("waiting");
		yield return StartCoroutine(SpecialEffectsManager.instance.Coroutine_StartTweenAlpha(
			behind_plane_animation.GetComponent<SpriteRenderer>(),
			0f, behind_plane_alpha_value, 2f));
		
		//toto
		//yield return StartCoroutine(CameraManager.instance.COROUTINE_ExplorationPlaneBehindToOpaque(2f));
		yield return new WaitForSeconds(1f);
		SoundManager.instance.PlayIndependant(SoundManager.instance.death_sfx);
		SpecialEffectsManager.instance.moster_transformation_shake.time_ratio = 1 / 4f;
		StartCoroutine(SpecialEffectsManager.instance.moster_transformation_shake.LaunchShake(visuals_transform));
		PlayerExploration.instance.generic_animator.SetBool("IsSick", true);
		yield return StartCoroutine(SpecialEffectsManager.instance.Coroutine_StartTweenAlpha(
			player_main_renderer,
			1f, 0f, 2f));

		yield return new WaitForSeconds(2f);

		CameraManager.instance.SetColorToFadePlane(Color.black);
		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToOpaque(2f));
//		yield return StartCoroutine(CameraManager.instance.Coroutine_LaunchAnimation(
//			behind_plane_animation,
//			"ToOpaque", 2f
//		));

		yield return new WaitForSeconds(2f);
		//		yield return StartCoroutine(death_scene_action_sequence.Coroutine_LaunchStartSequence());

		PlayerExploration.instance.generic_animator.SetBool("IsSick", false);
		//apply change

		ApplyDeathChange();
		CameraManager.instance.SetColorToBehindPlane(new Color(0, 0, 0, behind_plane_alpha_value));
		//return to exploration normal state
		//		yield return new WaitForSeconds(0.5f);
		PlayerExploration.instance.main_renderer.color = Color.white;
		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToTransparent(3f));
		yield return StartCoroutine(CameraManager.instance.Coroutine_LaunchAnimation(
			CameraManager.instance.exploration_plane_behind_player_animation,
			"ToTransparent", 1f
		));
	}
	void ApplyDeathChange()
	{
		 if (StateManager.instance.current_states.Contains(StateManager.State.BATTLE))
			BattleManager.instance.EndBattle();
		StateManager.instance.current_states.Remove(StateManager.State.BATTLE);
		StateManager.instance.current_states.Add(StateManager.State.EXPLORATION);
		StateManager.instance.UpdateFromStates();
		base_shield = 0;
		current_life = 0;
		for (int i = 0; i != base_element_affinities.Length; ++i)
			base_element_affinities[i] = 0;
		MostersManager.instance.AddToEvolved(current_moster);
		current_karma = MostersManager.instance.eliminated_mosters_list.Count + MostersManager.instance.evolved_mosters_list.Count;
		ApplyEvolutionChanges(GameManager.instance.baby_moster);

		GameManager.instance.GetSpawnScreen().MakeGoTo();
		PlayerExploration.instance.transform.position = GameManager.instance.GetSpawnPoint().transform.position;
		PlayerExploration.instance.RotatePlayer(Vector2.up);
		
	}
	public void UseKarmaForLife()
	{
		if (current_karma <= 0)
		{
			Debug.LogWarning("Pas assez de karma.");
			return ;
		}
		current_karma -= 1;
		current_life += life_bonus_per_karma;
	}
	public void UseKarmaForShield()
	{
		if (current_karma <= 0)
		{
			Debug.LogWarning("Pas assez de karma.");
			return ;
		}
		current_karma -= 1;
		base_shield += shield_bonus_per_karma;
	}
	public void UseKarmaForElementAffinity(Element element)
	{
		if (current_karma <= 0)
		{
			Debug.LogWarning("Pas assez de karma.");
			return ;
		}
		current_karma -= 1;
		base_element_affinities[(int) element] += element_affinity_bonus_per_karma;
	}
	public void RefreshMoster()
	{
		PlayerExploration.instance.UpdateMosterExploration();
		PlayerBattle.instance.UpdateMosterBattle();
	}
}