using UnityEngine;
using System.Collections;

public class PlayerBattle : MonoBehaviour {
	public static PlayerBattle instance;

	public float change_shield_cast_time = 1f;
	public int current_shield {get; set;}
	public bool is_shield_live {get; set;}
	public bool has_element {get; set;}
	public Element current_element {get; set;}

	public bool is_changing_element {get; set;}
	public Element element_changing_to {get; set;}

	public bool is_casting_skill {get; set;}
	public Skill casted_skill {get; set;}
	void Awake()
	{
		instance = this;
	}

	public void SetupForBattle()
	{
		is_shield_live = true;
		current_shield = Player.instance.GetEffectiveMaxShield();
		has_element = false;
		is_changing_element = false;
		is_casting_skill = false;
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

		//OK

		ScheduleSkill(skill_clicked);
	}

	public void ScheduleSkill(Skill skill_to_schedule)
	{
		TimelineEvent timeline_event_to_schedule;
		
		timeline_event_to_schedule = new TimelineEvent();
		timeline_event_to_schedule.name = skill_to_schedule.skill_name;
		timeline_event_to_schedule.time_remaining = skill_to_schedule.cast_time;
		timeline_event_to_schedule.on_complete_routine = Coroutine_SkillEffects(skill_to_schedule);
		EventsTimeline.instance.Schedule(timeline_event_to_schedule);

		is_casting_skill = true;
		casted_skill = skill_to_schedule;
	}
	IEnumerator Coroutine_SkillEffects(Skill skill)
	{
		SkillEffects skill_effects = skill.GetEffects();


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
					BattleManager.instance.enemy_current_life -= Mathf.Max(0, skill_effects.damages - current_boss_affinity);
					break;
				case ElementRelation.STRONG:
					BattleManager.instance.enemy_current_life -= (skill_effects.damages * 2) + current_boss_affinity;
					break;
				case ElementRelation.WEAK:
					BattleManager.instance.enemy_current_life -= Mathf.Max(0, (skill_effects.damages / 2) - current_boss_affinity);
					break;
				}
				Debug.LogWarning("element_relation:" + element_relation);
			}
			else
				BattleManager.instance.enemy_current_life -= skill_effects.damages;
			Debug.LogWarning("skill_effect: base_damage:" + skill_effects.damages);
			Debug.LogWarning("new_boss_life: " + BattleManager.instance.enemy_current_life);
		}
		BattleManager.instance.CheckEnemyDeath();
		Player.instance.current_life -= skill.cost;
		Player.instance.CheckDeath();
		is_casting_skill = false;
		casted_skill = null;

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
		timeline_event_to_schedule.time_remaining = change_shield_cast_time;
		timeline_event_to_schedule.on_complete_routine = Coroutine_ChangeElement(element);
		EventsTimeline.instance.Schedule(timeline_event_to_schedule);

		is_changing_element = true;
		element_changing_to = element;
	}
	IEnumerator Coroutine_ChangeElement(Element element)
	{
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
		timeline_event_to_schedule.time_remaining = change_shield_cast_time;
		timeline_event_to_schedule.on_complete_routine = Coroutine_RemoveElement();
		EventsTimeline.instance.Schedule(timeline_event_to_schedule);

		is_changing_element = true;
	}

	IEnumerator Coroutine_RemoveElement()
	{
		RemoveElement();
		is_changing_element = false;
		yield return new WaitForSeconds(0.001f);
	}
	void RemoveElement()
	{
		has_element = false;
	}
}
