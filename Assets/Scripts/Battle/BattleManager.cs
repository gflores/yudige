using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour {
	public static BattleManager instance {get; set;}

	public NonPlayerMoster enemy_moster {get; set;}
	public Skill skill_to_schedule {get; set;}

	public Element enemy_current_element {get; set;}
	public int enemy_current_life {get; set;}

	public int player_current_shield {get; set;}

	void Awake () {
		instance = this;
	}

	//HELPERS BEGIN
	public bool IsBattleAlreadyLaunched()
	{
		return StateManager.instance.current_states.Contains(StateManager.State.BATTLE);
	}
	//HELPERS END


	//BATTLE MANAGEMENT BEGIN
	public void StartBattle(NonPlayerMoster n_enemy_moster)
	{
		enemy_moster = n_enemy_moster;
		LaunchStartBattle();
	}

	public void LaunchStartBattle()
	{
		if (IsBattleAlreadyLaunched() == true)
			return ;
		StartCoroutine(Coroutine_StartBattle());
	}

	IEnumerator Coroutine_StartBattle()
	{
		StateManager.instance.current_states.Add(StateManager.State.BATTLE);
		enemy_current_life = enemy_moster.life;

		StartCoroutine(Coroutine_EnemyLoop());
		yield return new WaitForSeconds(0.001f);
	}
	//BATTLE MANAGEMENT END

	//ENEMY ACTIONS BEGIN
	IEnumerator Coroutine_EnemyLoop()
	{
		while (true)
		{
			yield return new WaitForSeconds(5f);
			EnemyChangeElement(EnemyGetNextElement());
		}
	}
	void EnemyChangeElement(Element new_element)
	{
		Debug.LogWarning("enemy change element from: " + enemy_current_element + "to: " + new_element);
		enemy_current_element = new_element;
	}
	Element EnemyGetNextElement()
	{
		Element to_return;

		while ((to_return = (Element)Random.Range(0, (int)Element.Count)) == enemy_current_element);
		return to_return;
	}
	public int GetEnemyMaxLife()
	{
		return enemy_moster.life;
	}
	void OnEnemyDeath()
	{
		Debug.LogWarning("ENEMY IS DEAD !!");
	}
	//ENEMY ACTIONS END

	//PLAYER ACTIONS BEGIN
	public void SchedulePlayerAttack(Skill n_skill_to_schedule)
	{
		skill_to_schedule = n_skill_to_schedule;
		LaunchSchedulePlayerAttack();
	}

	public void LaunchSchedulePlayerAttack()
	{
		TimelineEvent timeline_event_to_schedule;

		timeline_event_to_schedule = new TimelineEvent();
		timeline_event_to_schedule.name = skill_to_schedule.skill_name;
		timeline_event_to_schedule.time_remaining = skill_to_schedule.cast_time;
		timeline_event_to_schedule.on_complete_routine = Coroutine_SkillEffects(skill_to_schedule);
		EventsTimeline.instance.Schedule(timeline_event_to_schedule);
	}
	IEnumerator Coroutine_SkillEffects(Skill skill)
	{
		SkillEffects skill_effects = skill.GetEffects();

		if (skill_effects.deals_damage)
		{
			ElementRelation element_relation = ElementManager.instance.GetRelationBetween(
				skill.element, enemy_current_element
			);
			int current_boss_affinity = enemy_moster.moster_data.element_affinity_modifiers[(int)enemy_current_element];
			switch (element_relation)
			{
			case ElementRelation.NORMAL:
				enemy_current_life -= Mathf.Max(0, skill_effects.damages - current_boss_affinity);
				break;
			case ElementRelation.STRONG:
				enemy_current_life -= Mathf.Max(0, (skill_effects.damages * 2) + current_boss_affinity);
				break;
			case ElementRelation.WEAK:
				enemy_current_life -= Mathf.Max(0, (skill_effects.damages / 2) - current_boss_affinity);
				break;
			}
			Debug.LogWarning("skill_effect: base_damage:" + skill_effects.damages);
			Debug.LogWarning("element_relation:" + element_relation);
			Debug.LogWarning("new_boss_life: " + enemy_current_life);
		}
		if (enemy_current_life <= 0)
		{
			enemy_current_life = 0;
			OnEnemyDeath();
		}
		yield return new WaitForSeconds(0.001f);
	}
	//PLAYER ACTIONS END
}
