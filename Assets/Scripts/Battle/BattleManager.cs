using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {
	public static BattleManager instance {get; set;}

	public NonPlayerMoster enemy_moster {get; set;}

	public bool enemy_has_element {get; set;}
	public Element enemy_current_element {get; set;}
	public int enemy_current_life {get; set;}
	public EnemyAttack scheduled_enemy_attack {get; set;}
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
		enemy_moster.SetupForBattle();
		PlayerBattle.instance.SetupForBattle();
		StartCoroutine(Coroutine_EnemyLoop());
		yield return new WaitForSeconds(0.001f);
	}
	//BATTLE MANAGEMENT END

	//ENEMY ACTIONS BEGIN
	IEnumerator Coroutine_EnemyLoop()
	{
		while (true)
		{
			enemy_moster.SetupForNewPhase();
			while (enemy_moster.HasUsedAllAttacks() == false)
			{
				float[] before_after_time_to_wait = enemy_moster.GetBeforeAfterAttackTime();

				yield return new WaitForSeconds(before_after_time_to_wait[0]);
				scheduled_enemy_attack = enemy_moster.GetNextAttack();
				ScheduleEnemyAttack(scheduled_enemy_attack);
				yield return new WaitForSeconds(before_after_time_to_wait[1]);
			}
			yield return new WaitForSeconds(enemy_moster.attack_min_gap_time / 2);
			scheduled_enemy_attack = enemy_moster.GetBurstAttack(enemy_current_element);
			ScheduleEnemyAttack(scheduled_enemy_attack);
			yield return new WaitForSeconds(enemy_moster.before_change_element_time);
			ScheduleEnemyChangeElement(enemy_moster.GetNextElement());
		}
	}
	void ScheduleEnemyChangeElement(Element element)
	{
		TimelineEvent timeline_event_to_schedule;
		
		timeline_event_to_schedule = new TimelineEvent();
		timeline_event_to_schedule.name = "E.Element change:" + element.ToString();
		timeline_event_to_schedule.time_remaining = EventsTimeline.instance.total_time;
		timeline_event_to_schedule.on_complete_routine = Coroutine_EnemyChangeElement(element);
		EventsTimeline.instance.Schedule(timeline_event_to_schedule);
	}
	IEnumerator Coroutine_EnemyChangeElement(Element element)
	{
		EnemyChangeElement(element);
		yield return new WaitForSeconds(0.001f);
	}
	void ScheduleEnemyAttack(EnemyAttack attack_to_schedule)
	{
		TimelineEvent timeline_event_to_schedule;
		
		timeline_event_to_schedule = new TimelineEvent();
		timeline_event_to_schedule.name = attack_to_schedule.damage.ToString();
		timeline_event_to_schedule.time_remaining = EventsTimeline.instance.total_time;
		timeline_event_to_schedule.on_complete_routine = Coroutine_EnemyAttackEffects(attack_to_schedule);
		EventsTimeline.instance.Schedule(timeline_event_to_schedule);
	}
	IEnumerator Coroutine_EnemyAttackEffects(EnemyAttack enemy_attack)
	{
		if (enemy_attack.is_burst == true)
		{
			Debug.LogWarning("Boss attack changed to NEUTRAL !");
			enemy_has_element = false;
		}
		if (PlayerBattle.instance.has_element == true)
		{
			ElementRelation element_relation = ElementManager.instance.GetRelationBetween(
				enemy_attack.element, PlayerBattle.instance.current_element
			);
			int current_affinity = PlayerBattle.instance.GetCurrentElementAffinity();
			switch (element_relation)
			{
			case ElementRelation.NORMAL:
				Player.instance.current_life -= Mathf.Max(0, enemy_attack.damage - current_affinity);
				break;
			case ElementRelation.STRONG:
				Player.instance.current_life -= (enemy_attack.damage * 2) + current_affinity;
				break;
			case ElementRelation.WEAK:
				Player.instance.current_life -= Mathf.Max(0, (enemy_attack.damage / 2) - current_affinity);
				break;
			}
			Debug.LogWarning("Player took damage, element_relation:" + element_relation);
		}
		else
		{
			Player.instance.current_life -= enemy_attack.damage;
			//NEUTRAL
		}
		Debug.LogWarning("enemy_attack: base_damage:" + enemy_attack.damage);
		Debug.LogWarning("new_player_life: " + Player.instance.current_life);

		Player.instance.CheckDeath();
		yield return new WaitForSeconds(0.001f);
	}
	public void EnemyChangeElement(Element new_element)
	{
		Debug.LogWarning("enemy change element from: " + enemy_current_element + "to: " + new_element);
		enemy_current_element = new_element;
	}
	public int GetEnemyMaxLife()
	{
		return enemy_moster.life;
	}
	public bool CheckEnemyDeath()
	{
		if (enemy_current_life <= 0)
		{
			enemy_current_life = 0;
			OnEnemyDeath();
			return true;
		}
		return false;
	}
	void OnEnemyDeath()
	{
		Debug.LogWarning("ENEMY IS DEAD !!");
	}

	public int GetEnemyCurrentElementAffinity()
	{
		return enemy_moster.moster_data.element_affinity_modifiers[(int)enemy_current_element];
	}
	//ENEMY ACTIONS END

	//PLAYER ACTIONS BEGIN

	public void OnPlayerDeath()
	{
		Debug.LogWarning("mort en combat");
	}
	//PLAYER ACTIONS END
}
