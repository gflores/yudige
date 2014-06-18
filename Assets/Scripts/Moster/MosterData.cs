using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MosterData : MonoBehaviour {
	public string moster_name;
	public List<MosterData> possible_evolution_list;
	public Transform visuals_transform;
	public SpriteRenderer _sprite_renderer;
	public float time_before_life_decrease = 0f;
	public int life_bonus = 10;
	public int shield_modifier = 10;
	public int[] element_affinity_modifiers;
	public List<Skill> skills;

	public NpcMosterBattle moster_battle {get; set;}
	public NpcMosterExploration moster_exploration {get; set;}
	public MosterEvolution moster_evolution {get; set;}
	public Collider2D exploration_collider;

	void Awake()
	{
		moster_battle = GetComponentInChildren<NpcMosterBattle>();
		moster_exploration = GetComponentInChildren<NpcMosterExploration>();
		moster_evolution = GetComponentInChildren<MosterEvolution>();

		moster_battle.moster_data = this;

		moster_exploration.moster_data = this;
		moster_exploration.visuals_transform.localScale = visuals_transform.localScale;
		moster_exploration.main_renderer.sprite = GetSprite();
		moster_exploration.main_renderer.color = Color.red;

		moster_evolution.moster_data = this;
	}

	public Sprite GetSprite()
	{
		return _sprite_renderer.sprite;
	}
}
