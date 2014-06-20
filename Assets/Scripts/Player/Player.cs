using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	public static Player instance;

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
	float _time_passed_before_decrease_life = 0f;

	void Awake () {
		instance = this;
		base_element_affinities = new int[(int)Element.Count];
	}

	void FixedUpdate()
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
		PlayerExploration.instance.DisableControls();
		CameraManager.instance.SetColorToBehindPlane(Color.white);
		yield return StartCoroutine(CameraManager.instance.COROUTINE_ExplorationPlaneBehindToOpaque(2f));

		//apply change
		MostersManager.instance.AddToEvolved(current_moster);
		time_lived = 0f;
		ApplyEvolutionChanges(new_moster);
		//return to exploration normal state
		yield return StartCoroutine(CameraManager.instance.COROUTINE_ExplorationPlaneBehindToTransparent(2f));
		PlayerExploration.instance.EnableControls();
	}
	public void ApplyEvolutionChanges(MosterData new_moster)
	{
		current_life += new_moster.life_bonus;
		current_moster = new_moster;
		PlayerExploration.instance.UpdateMosterExploration();
	}

	public bool CheckDeath()
	{
		if (current_life <= 0)
		{
			current_life = 0;
			OnDeath();
			if (StateManager.instance.current_states.Contains(StateManager.State.BATTLE))
			    BattleManager.instance.OnPlayerDeath();
			return true;
		}
		return false;
	}

	public void OnDeath()
	{
		Debug.LogWarning("DEAD");
		StartCoroutine(Coroutine_DeathScene());
	}
	IEnumerator Coroutine_DeathScene()
	{
		PlayerExploration.instance.DisableControls();
		CameraManager.instance.SetColorToBehindPlane(Color.black);
		yield return StartCoroutine(CameraManager.instance.COROUTINE_ExplorationPlaneBehindToOpaque(2f));

		ApplyDeathChange();
		yield return StartCoroutine(CameraManager.instance.COROUTINE_ExplorationPlaneBehindToTransparent(2f));
		PlayerExploration.instance.EnableControls();
	}
	void ApplyDeathChange()
	{
		base_shield = 0;
		current_life = 0;
		for (int i = 0; i != base_element_affinities.Length; ++i)
			base_element_affinities[i] = 0;
		MostersManager.instance.AddToEvolved(current_moster);
		current_karma = MostersManager.instance.eliminated_mosters_list.Count + MostersManager.instance.evolved_mosters_list.Count;
		ApplyEvolutionChanges(GameManager.instance.baby_moster);

		GameManager.instance.rebirth_screen.SetCameraToThis();
		GameManager.instance.rebirth_screen.ApplyBackgroundMusic();
		PlayerExploration.instance.transform.position = GameManager.instance.rebirth_spawn_point.transform.position;
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

}