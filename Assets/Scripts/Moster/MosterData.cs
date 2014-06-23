using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MosterData : MonoBehaviour {
	public string moster_name;
	public List<MosterData> possible_evolution_list;
	public float time_before_life_decrease = 0f;
	public int life_bonus = 10;
	public int shield_modifier = 10;
	public int[] element_affinity_modifiers;
	public List<Skill> skills;
	public Transform visuals_transform;
	public Animator moster_animator;
	public NpcMosterBattle moster_battle {get; set;}
	public NpcMosterExploration moster_exploration {get; set;}
	public MosterEvolution moster_evolution {get; set;}
	public Transform exploration_hitboxes_transform;

	void Awake()
	{
		moster_battle = GetComponentInChildren<NpcMosterBattle>();
		moster_exploration = GetComponentInChildren<NpcMosterExploration>();
		moster_evolution = GetComponentInChildren<MosterEvolution>();

		moster_battle.moster_data = this;
		moster_battle.visuals_transform.localScale = visuals_transform.localScale;
//		moster_battle.main_renderer.color = Color.red;
		moster_battle.sprite_animator.runtimeAnimatorController = GetAnimatorController();//yoda

		moster_exploration.moster_data = this;
		moster_exploration.visuals_transform.localScale = visuals_transform.localScale;
//		moster_exploration.main_renderer.color = Color.red;
		moster_exploration.sprite_animator.runtimeAnimatorController = GetAnimatorController();

		moster_evolution.moster_data = this;
	}

	public RuntimeAnimatorController GetAnimatorController()
	{
		return moster_animator.runtimeAnimatorController;
	}
}
