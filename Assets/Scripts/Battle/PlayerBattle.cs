using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerBattle : MonoBehaviour {
	public static PlayerBattle instance;
	public float time_passed_before_shield_recovery = 2f;
	public float change_element_cast_time = 1f;
	public float burst_cast_time = 5f;
	public float cancel_combos_time = 5f;
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
	public int bonus_affinity_to_be_added_next {get; set;}
	public List<int> current_affinity_combos_bonus {get; set;}
	public bool using_reset {get; set;}
	void Awake()
	{
		instance = this;
		availaible_defense_elements = new List<bool>();
		for (int i = 0; i != (int)Element.Count; ++i)
			availaible_defense_elements.Add(true);
		current_affinity_combos_bonus = new List<int>();
		for (int i = 0; i != (int)Element.Count; ++i)
			current_affinity_combos_bonus.Add(0);

	}
	float _time_passed_before_increase_shield;
	void Update()
	{
		if (is_shield_live == true)
		{
			_time_passed_before_increase_shield += Time.deltaTime;
			if (_time_passed_before_increase_shield >= time_passed_before_shield_recovery)
			{
				_time_passed_before_increase_shield = 0f;
				current_shield = Mathf.Min(current_shield + 1, Player.instance.GetEffectiveMaxShield());
			}
		}
	}
	public int GetEffectiveBattleElementAffinity(Element e)
	{
		return Player.instance.GetEffectiveElementAffinity(e) + current_affinity_combos_bonus[(int)e];
	}
	public void ResetAffinities()
	{
		ResetConsumedSkills();
		for (int i = 0; i != (int)Element.Count; ++i)
			current_affinity_combos_bonus[i] = 0;
	}
	public void BurstAffinities()
	{
		if (CanBurst() == false)
		{
			Debug.LogWarning("DENIED: must have casted a skill before");
			return;
		}
		Debug.LogWarning("BURST !!!!! AFFINITIES");
		ScheduleBurstAttack(last_skill_effect_applied.skill);
	}
	public void CancelCombos()
	{
		if (is_casting_skill == true)
		{
			Debug.LogWarning("already casting something...");
			return;
		}
		for (int i = 0; i != (int)Element.Count; ++i)
			current_affinity_combos_bonus[i] = 0;
		StartCoroutine(Coroutine_RemoveElement());
		ScheduleCancelCombos();
	}
	public void ScheduleCancelCombos()
	{
		TimelineEvent timeline_event_to_schedule;
		using_reset = true;
		timeline_event_to_schedule = new TimelineEvent();
		timeline_event_to_schedule.name = "CANCEL COMBOS";
		timeline_event_to_schedule.side = TimelineSide.PLAYER;
		timeline_event_to_schedule.event_type = TimelineEventType.PLAYER_CANCEL_COMBOS;
		timeline_event_to_schedule.time_remaining = cancel_combos_time;
		timeline_event_to_schedule.on_complete_routine = Coroutine_CancelCombos();
		EventsTimeline.instance.Schedule(timeline_event_to_schedule);
		
		is_casting_skill = true;
	}
	IEnumerator Coroutine_CancelCombos()
	{
		using_reset = false;
		is_casting_skill = false;
		ResetAffinities();
		StartCoroutine(Coroutine_RemoveElement());
		bonus_affinity_to_be_added_next = 0;
		yield return new WaitForSeconds(0.001f);
	}

	public void ScheduleBurstAttack(Skill skill_to_schedule)
	{
		current_affinity_combos_bonus[(int)skill_to_schedule.element] += bonus_affinity_to_be_added_next;
		StartCoroutine(Coroutine_ChangeElement(skill_to_schedule.element));
		skill_to_schedule.is_consumed = true;
		TimelineEvent timeline_event_to_schedule;
		
		timeline_event_to_schedule = new TimelineEvent();
		timeline_event_to_schedule.name = skill_to_schedule.skill_name;
		timeline_event_to_schedule.side = TimelineSide.PLAYER;
		timeline_event_to_schedule.event_type = TimelineEventType.PLAYER_BURST_ATTACK;
		timeline_event_to_schedule.element = skill_to_schedule.element;
		timeline_event_to_schedule.time_remaining = burst_cast_time;
		timeline_event_to_schedule.on_complete_routine = Coroutine_SkillEffects(skill_to_schedule, true);
		EventsTimeline.instance.Schedule(timeline_event_to_schedule);
		
		is_casting_skill = true;
		casted_skill = skill_to_schedule;
	}

	void ResetConsumedSkills()
	{
		foreach(var skill in Player.instance.current_moster.skills)
			skill.is_consumed = false;
	}
	public void SetupForBattle()
	{
		using_reset = false;
		generic_animator.SetBool("InBattle", true);
		ResetAffinities();
		StartCoroutine(Coroutine_RemoveElement());
		bonus_affinity_to_be_added_next = 0;
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
		return PlayerBattle.instance.GetEffectiveBattleElementAffinity(current_element);
	}
	public void ClickOnSkill(Skill skill_clicked)
	{
		if (IsSkillAvailable(skill_clicked) == false)
			return ;

		if (IsLastSkillUsable())
		{
			Debug.LogWarning("last skill, BURST !");
			ScheduleBurstAttack(skill_clicked);
		}
		else
		{
			ScheduleSkill(skill_clicked);
		}
	}

	public void ScheduleSkill(Skill skill_to_schedule)
	{
		current_affinity_combos_bonus[(int)skill_to_schedule.element] += bonus_affinity_to_be_added_next;
		bonus_affinity_to_be_added_next = 0;
		StartCoroutine(Coroutine_ChangeElement(skill_to_schedule.element));
		skill_to_schedule.is_consumed = true;
		Player.instance.current_life = Mathf.Max(0, Player.instance.current_life - skill_to_schedule.cost);
		TimelineEvent timeline_event_to_schedule;
		
		timeline_event_to_schedule = new TimelineEvent();
		timeline_event_to_schedule.name = skill_to_schedule.skill_name;
		timeline_event_to_schedule.side = TimelineSide.PLAYER;
		timeline_event_to_schedule.event_type = TimelineEventType.PLAYER_NORMAL_ATTACK;
		timeline_event_to_schedule.element = skill_to_schedule.element;
		timeline_event_to_schedule.time_remaining = skill_to_schedule.cast_time;
		timeline_event_to_schedule.on_complete_routine = Coroutine_SkillEffects(skill_to_schedule);
		EventsTimeline.instance.Schedule(timeline_event_to_schedule);

		is_casting_skill = true;
		casted_skill = skill_to_schedule;
	}

	public List<ParticleSystem> attacks_FX {get; set;}

	IEnumerator Coroutine_AttackFX(ParticleSystem attack_FX, bool is_burst_attack)
	{
		attack_FX.transform.position = BattleManager.instance.battle_right_side_point.position;
		if (is_burst_attack == true)
		{
			attack_FX.startLifetime += BattleManager.instance.life_time_delta_for_burst_attack;
			attack_FX.startSpeed += BattleManager.instance.speed_delta_for_burst_attack;
			attack_FX.startSize += BattleManager.instance.extra_size_for_burst_attack;
		}

		attack_FX.Play();
		Vector3 dest_position = BattleManager.instance.battle_right_side_point.position;
		dest_position.x = BattleManager.instance.enemy_visual_transform.position.x;
		SoundManager.instance.PlayIndependant(SoundManager.instance.attack_start_sound);

		if (is_burst_attack == true)
		{
			TweenPosition.Begin(attack_FX.gameObject, 0.5f + BattleManager.instance.extra_time_for_burst_attack, dest_position);
			yield return new WaitForSeconds(BattleManager.instance.extra_time_for_burst_attack);
		}
		else
		{
			TweenPosition.Begin(attack_FX.gameObject, 0.5f, dest_position);
			yield return new WaitForSeconds(0.2f);
		}
		attack_FX.Stop();
		
	}

	public bool CanBurst()
	{
		return bonus_affinity_to_be_added_next != 0;
	}
	public bool skill_ended {get; set;}
	IEnumerator Coroutine_SkillEffects(Skill skill, bool is_burst = false)
	{
		ParticleSystem attack_FX = attacks_FX[(int)skill.element];
		Debug.LogWarning("is_burst = "+ is_burst);
		is_casting_skill = false;
		casted_skill = null;

		StartCoroutine(Coroutine_AttackFX(attack_FX, is_burst));
		if (is_burst == true)
		{
			yield return new WaitForSeconds(0.5f + BattleManager.instance.extra_time_for_burst_attack);
			attack_FX.startLifetime -= BattleManager.instance.life_time_delta_for_burst_attack;
			attack_FX.startSpeed -= BattleManager.instance.speed_delta_for_burst_attack;
			attack_FX.startSize -= BattleManager.instance.extra_size_for_burst_attack;
		}
		else
		{
			bonus_affinity_to_be_added_next = Mathf.CeilToInt(((float)GetEffectiveBattleElementAffinity(skill.element)) * skill.combos_bonus_affinity_ratio);
			yield return new WaitForSeconds(0.5f);
		}



		SkillEffects skill_effects = skill.GetEffects(is_burst);
		last_skill_effect_applied = skill_effects;
		if (is_burst == true)
		{
			bonus_affinity_to_be_added_next = 0;
			ResetAffinities();
			Player.instance.base_element_affinities[(int)skill.element] += 1;
		}
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
					BattleManager.instance.EnemyTakeDamage(Mathf.Max(0, skill_effects.damages - current_boss_affinity));
					break;
				case ElementRelation.STRONG:
					SoundManager.instance.PlayIndependant(SoundManager.instance.strong_damage_sound);
					StartCoroutine(SpecialEffectsManager.instance.critical_damage_shake.LaunchShake(BattleManager.instance.enemy_moster.visuals_transform));
					BattleManager.instance.EnemyTakeDamage((skill_effects.damages * 2) + current_boss_affinity);
					break;
				case ElementRelation.WEAK:
					SoundManager.instance.PlayIndependant(SoundManager.instance.weak_damage_sound);
					BattleManager.instance.enemy_moster.moster_data.moster_battle.generic_animator.SetTrigger("StayStill");
					BattleManager.instance.EnemyTakeDamage(Mathf.Max(0, (skill_effects.damages / 2) - current_boss_affinity));
					break;
				}
				Debug.LogWarning("element_relation:" + element_relation + ", enemy affinity: " + current_boss_affinity);
			}
			else
			{
				SoundManager.instance.PlayIndependant(SoundManager.instance.normal_damage_sound);
				StartCoroutine(SpecialEffectsManager.instance.normal_damage_shake.LaunchShake(BattleManager.instance.enemy_moster.visuals_transform));
				BattleManager.instance.EnemyTakeDamage(skill_effects.damages);
			}
			Debug.LogWarning("skill_effect: base_damage:" + skill_effects.damages);
			Debug.LogWarning("new_boss_life: " + BattleManager.instance.enemy_current_life);
		}


		if (BattleManager.instance.CheckEnemyDeath() == false)
		{
			Player.instance.CheckDeath();
		}
		skill_ended = true;
		yield return new WaitForSeconds(0.001f);
	}

	public void ClickOnElementDefense(Element element_clicked)
	{
		Debug.LogWarning("USELESS NOW !!");
//		if (is_changing_element == true)
//		{
//			Debug.LogWarning("DENIED: already changing element !");
//			return ;
//		}
//		if (is_casting_skill == true && casted_skill.element == element_clicked)
//		{
//			Debug.LogWarning("DENIED: a skill of the same element is being casted !");
//			return ;
//		}
//		//OK
//
//		if (has_element == true && current_element == element_clicked)
//		{
//			Debug.LogWarning("want: back to NEUTRAL");
//			ScheduleRemoveElement();
//		}
//		else
//		{
//			Debug.LogWarning("want: change to: " + element_clicked);
//			ScheduleChangeElement(element_clicked);
//		}
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
	bool IsLastSkillUsable()
	{
		int count = 0;
		foreach (var skill in Player.instance.current_moster.skills)
		{
			if (skill.available == true && skill.is_consumed == false)
				count += 1;
		}
		return count == 1;
	}
	public List<Skill> GenerateSkillsAvailable()
	{
		List<Skill> skills_available = new List<Skill>();

		foreach (var skill in Player.instance.current_moster.skills)
		{
			if (skill.available == true)
				skills_available.Add(skill);
		}
		return skills_available;
	}
	public List<Skill> GenerateSkillsAvailable(Element e)
	{
		List<Skill> skills_available = new List<Skill>();
		
		foreach (var skill in Player.instance.current_moster.skills)
		{
			if (skill.available == true && skill.element == e)
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
		BattleScreen.instance.DamageToPlayer(damages);
	}
	
	public bool IsSkillAvailable(Skill skill_clicked)
	{
		if (is_casting_skill == true)
		{
			//Debug.LogWarning("DENIED: already casting a skill !");
			return false;
		}
		return true;
	}
}
