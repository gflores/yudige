using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerBattle : MonoBehaviour {
	public static PlayerBattle instance;
	public float time_passed_before_shield_recovery = 2f;
	public float change_element_cast_time = 1f;

	public Animator generic_animator;
	public Animator sprite_animator;
	public SpriteRenderer main_renderer;
	public Transform visuals_transform;

	public int current_shield {get; set;}
	public bool is_shield_live {get; set;}
	public bool has_element {get; set;}
	public Element current_element {get; set;}

	public bool is_changing_element {get; set;}
	public Element element_changing_to {get; set;}

	public bool is_casting_skill {get; set;}
	public Skill casted_skill {get; set;}
	public List<bool> availaible_defense_elements {get; set;}
	public SkillEffects last_skill_effect_applied {get; set;}

	void Awake()
	{
		instance = this;
		availaible_defense_elements = new List<bool>();
		for (int i = 0; i != (int)Element.Count; ++i)
			availaible_defense_elements.Add(true);
	}
	float _time_passed_before_increase_shield;
	void Update()
	{
		if (is_shield_live == true && Player.instance.is_living_time_passing == true)
		{
			_time_passed_before_increase_shield += Time.deltaTime;
			if (_time_passed_before_increase_shield >= time_passed_before_shield_recovery)
			{
				_time_passed_before_increase_shield = 0f;
				current_shield = Mathf.Min(current_shield + 1, Player.instance.GetEffectiveMaxShield());
			}
		}
	}
	public void ResetAffinities()
	{
		Debug.LogWarning("RESETING AFFINITIES");
	}
	public void BurstAffinities()
	{
		Debug.LogWarning("BURST !!!!! AFFINITIES");
	}
	public void SetupForBattle()
	{
		generic_animator.SetBool("InBattle", true);
		is_shield_live = true;
		current_shield = Player.instance.GetEffectiveMaxShield();
		has_element = false;
		is_changing_element = false;
		is_casting_skill = false;
		main_renderer.color = Color.white;
		foreach(var defense_FX in defenses_FX)
			defense_FX.Stop();
		foreach(var attack_FX in attacks_FX)
			attack_FX.Stop();
	}

	public int GetCurrentElementAffinity()
	{
		return Player.instance.GetEffectiveElementAffinity(current_element);
	}
	public void ClickOnSkill(Skill skill_clicked)
	{
		if (is_casting_skill == true)
		{
			Debug.LogWarning("DENIED: already casting a skill !");
			return ;
		}
		if (is_changing_element == true && element_changing_to == skill_clicked.element)
		{
			Debug.LogWarning("DENIED: already removing or changing this element to your defense");
			return ;
		}
		if (has_element == true && skill_clicked.element == current_element)
		{
			Debug.LogWarning("DENIED: cannot use a skill with the same element as the current defense element");
			return ;
		}
		if (skill_clicked.availaible == false)
		{
			Debug.LogWarning("DENIED: the skill is deactivated");
			return ;
		}
		if (skill_clicked.cost >= Player.instance.current_life)
		{
			Debug.LogWarning("DENIED: would instantly kill the player because of the cost");
			return ;
		}
		//OK

		ScheduleSkill(skill_clicked);
	}

	public void ScheduleSkill(Skill skill_to_schedule)
	{
		Player.instance.current_life = Mathf.Max(0, Player.instance.current_life - skill_to_schedule.cost);
		TimelineEvent timeline_event_to_schedule;
		
		timeline_event_to_schedule = new TimelineEvent();
		timeline_event_to_schedule.name = skill_to_schedule.skill_name;
		timeline_event_to_schedule.time_remaining = skill_to_schedule.cast_time;
		timeline_event_to_schedule.on_complete_routine = Coroutine_SkillEffects(skill_to_schedule);
		EventsTimeline.instance.Schedule(timeline_event_to_schedule);

		is_casting_skill = true;
		casted_skill = skill_to_schedule;
	}

	public List<ParticleSystem> attacks_FX {get; set;}

	IEnumerator Coroutine_AttackFX(ParticleSystem attack_FX)
	{
		attack_FX.transform.position = BattleManager.instance.battle_right_side_point.position;
		attack_FX.Play();
		Vector3 dest_position = BattleManager.instance.battle_right_side_point.position;
		dest_position.x = BattleManager.instance.enemy_visual_transform.position.x;
		SoundManager.instance.PlayIndependant(SoundManager.instance.attack_start_sound);
		TweenPosition.Begin(attack_FX.gameObject, 0.5f, dest_position);
		yield return new WaitForSeconds(0.2f);
		attack_FX.Stop();
		
	}


	IEnumerator Coroutine_SkillEffects(Skill skill)
	{
		StartCoroutine(Coroutine_AttackFX(attacks_FX[(int)skill.element]));
		yield return new WaitForSeconds(0.5f);

		SkillEffects skill_effects = skill.GetEffects();
		last_skill_effect_applied = skill_effects;

		if (skill_effects.deals_damage)
		{
			if (BattleManager.instance.enemy_has_element == true)
			{
				ElementRelation element_relation = ElementManager.instance.GetRelationBetween(
					skill.element, BattleManager.instance.enemy_current_element
					);
				int current_boss_affinity = BattleManager.instance.GetEnemyCurrentElementAffinity();
				switch (element_relation)
				{
				case ElementRelation.NORMAL:
					SoundManager.instance.PlayIndependant(SoundManager.instance.normal_damage_sound);
					StartCoroutine(SpecialEffectsManager.instance.normal_damage_shake.LaunchShake(BattleManager.instance.enemy_moster.visuals_transform));
					BattleManager.instance.enemy_current_life -= Mathf.Max(0, skill_effects.damages - current_boss_affinity);
					break;
				case ElementRelation.STRONG:
					SoundManager.instance.PlayIndependant(SoundManager.instance.strong_damage_sound);
					StartCoroutine(SpecialEffectsManager.instance.critical_damage_shake.LaunchShake(BattleManager.instance.enemy_moster.visuals_transform));
					BattleManager.instance.enemy_current_life -= (skill_effects.damages * 2) + current_boss_affinity;
					break;
				case ElementRelation.WEAK:
					SoundManager.instance.PlayIndependant(SoundManager.instance.weak_damage_sound);
					BattleManager.instance.enemy_moster.moster_data.moster_battle.generic_animator.SetTrigger("StayStill");
					BattleManager.instance.enemy_current_life -= Mathf.Max(0, (skill_effects.damages / 2) - current_boss_affinity);
					break;
				}
				Debug.LogWarning("element_relation:" + element_relation);
			}
			else
			{
				SoundManager.instance.PlayIndependant(SoundManager.instance.normal_damage_sound);
				StartCoroutine(SpecialEffectsManager.instance.normal_damage_shake.LaunchShake(BattleManager.instance.enemy_moster.visuals_transform));
				BattleManager.instance.enemy_current_life -= skill_effects.damages;
			}
			Debug.LogWarning("skill_effect: base_damage:" + skill_effects.damages);
			Debug.LogWarning("new_boss_life: " + BattleManager.instance.enemy_current_life);
		}
		is_casting_skill = false;
		casted_skill = null;


		if (BattleManager.instance.CheckEnemyDeath() == false)
		{
			Player.instance.CheckDeath();
		}

		yield return new WaitForSeconds(0.001f);
	}

	public void ClickOnElementDefense(Element element_clicked)
	{
		if (is_changing_element == true)
		{
			Debug.LogWarning("DENIED: already changing element !");
			return ;
		}
		if (is_casting_skill == true && casted_skill.element == element_clicked)
		{
			Debug.LogWarning("DENIED: a skill of the same element is being casted !");
			return ;
		}
		//OK

		if (has_element == true && current_element == element_clicked)
		{
			Debug.LogWarning("want: back to NEUTRAL");
			ScheduleRemoveElement();
		}
		else
		{
			Debug.LogWarning("want: change to: " + element_clicked);
			ScheduleChangeElement(element_clicked);
		}
	}
	void ScheduleChangeElement(Element element)
	{
		TimelineEvent timeline_event_to_schedule;
		
		timeline_event_to_schedule = new TimelineEvent();
		timeline_event_to_schedule.name = "P.Element change:" + element.ToString();
		timeline_event_to_schedule.time_remaining = change_element_cast_time;
		timeline_event_to_schedule.on_complete_routine = Coroutine_ChangeElement(element);
		EventsTimeline.instance.Schedule(timeline_event_to_schedule);

		is_changing_element = true;
		element_changing_to = element;
	}
	public List<ParticleSystem> defenses_FX;
	IEnumerator Coroutine_ChangeElement(Element element)
	{
		ParticleSystem defense_fx = defenses_FX[(int) element];

		foreach(var defense_FX in defenses_FX)
			defense_FX.Stop();
		defense_fx.Play();
		SoundManager.instance.PlayIndependant(SoundManager.instance.change_element_sound);
		ChangeElement(element);
		is_changing_element = false;
		yield return new WaitForSeconds(0.001f);
	}
	void ChangeElement(Element element)
	{
		has_element = true;
		current_element = element;
	}

	void ScheduleRemoveElement()
	{
		TimelineEvent timeline_event_to_schedule;
		
		timeline_event_to_schedule = new TimelineEvent();
		timeline_event_to_schedule.name = "P.Element NEUTRAL";
		timeline_event_to_schedule.time_remaining = change_element_cast_time;
		timeline_event_to_schedule.on_complete_routine = Coroutine_RemoveElement();
		EventsTimeline.instance.Schedule(timeline_event_to_schedule);

		is_changing_element = true;
	}

	IEnumerator Coroutine_RemoveElement()
	{
		foreach(var defense_FX in defenses_FX)
			defense_FX.Stop();
		SoundManager.instance.PlayIndependant(SoundManager.instance.remove_element_sound);
		RemoveElement();
		is_changing_element = false;
		yield return new WaitForSeconds(0.001f);
	}
	void RemoveElement()
	{
		has_element = false;
	}

	public void UpdateMosterBattle()
	{
		sprite_animator.runtimeAnimatorController = Player.instance.current_moster.GetAnimatorController();
//		main_renderer.color = Color.green;
		visuals_transform.localScale = Player.instance.current_moster.visuals_transform.localScale;
	}
	public List<Skill> GenerateSkillsAvailable()
	{
		List<Skill> skills_available = new List<Skill>();

		foreach (var skill in Player.instance.current_moster.skills)
		{
			if (skill.availaible == true)
				skills_available.Add(skill);
		}
		return skills_available;
	}
	public List<Skill> GenerateSkillsAvailable(Element e)
	{
		List<Skill> skills_available = new List<Skill>();
		
		foreach (var skill in Player.instance.current_moster.skills)
		{
			if (skill.availaible == true && skill.element == e)
				skills_available.Add(skill);
		}
		return skills_available;
	}
	public void TakeDamage(int damages)
	{
		if (is_shield_live == true)
		{
			current_shield = Mathf.Max(0, current_shield - damages);
			if (current_shield <= 0)
			{
				is_shield_live = false;
				Debug.LogWarning("The player shield broke !!");
			}
		}
		else
			Player.instance.current_life = Mathf.Max(0, Player.instance.current_life - damages);
	}
}
