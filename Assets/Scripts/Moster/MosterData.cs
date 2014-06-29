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
	public Animator moster_animator_dark;
	public NpcMosterBattle moster_battle {get; set;}
	public NpcMosterExploration moster_exploration {get; set;}
	public MosterEvolution moster_evolution {get; set;}
	public Transform exploration_hitboxes_transform;
	public Transform dark_transform;
	public ObjectActivator dark_object_activator;

	void Awake()
	{
		moster_battle = GetComponentInChildren<NpcMosterBattle>();
		moster_exploration = GetComponentInChildren<NpcMosterExploration>();
		moster_evolution = GetComponentInChildren<MosterEvolution>();

		moster_battle.moster_data = this;
		moster_exploration.moster_data = this;
		moster_evolution.moster_data = this;

	}
	void Start()
	{
		MosterStateUpdate();
	}
	public void MosterStateUpdate(){
		if (MostersManager.instance.IsEliminatedDark(this)) // si version dark eliminé
		{
			moster_exploration.gameObject.SetActive(false);
		}
		else if (MostersManager.instance.IsEliminated(this)) //si juste la version normale éliminé
		{
			moster_battle.sprite_animator.runtimeAnimatorController = GetDarkAnimatorController();			
			moster_exploration.sprite_animator.runtimeAnimatorController = GetDarkAnimatorController();
			moster_exploration.transform.position = dark_transform.position;
			moster_exploration.transform.localScale = dark_transform.localScale;
			moster_exploration.transform.localRotation = dark_transform.localRotation;
			dark_object_activator.Change();
			moster_battle.phase_increment_bonus_ratio *= 1.5f;
			moster_battle.life *= 2;
			moster_battle.attack_total_time *= 0.9f;
			moster_battle.attack_min_gap_time /= 2;
			moster_battle.after_burst_attack_time = 0f;
		}
		else //si la version normale a jamais été eliminé
		{
			moster_battle.sprite_animator.runtimeAnimatorController = GetAnimatorController();			
			moster_exploration.sprite_animator.runtimeAnimatorController = GetAnimatorController();
		}
		moster_battle.visuals_transform.localScale = visuals_transform.localScale;
		moster_exploration.visuals_transform.localScale = visuals_transform.localScale;

	}
	public RuntimeAnimatorController GetAnimatorController()
	{
		return moster_animator.runtimeAnimatorController;
	}
	public RuntimeAnimatorController GetDarkAnimatorController()
	{
		return moster_animator_dark.runtimeAnimatorController;
	}
}
