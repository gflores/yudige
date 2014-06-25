using UnityEngine;
using System.Collections;

public class TutoScriptedBattle : SequentialAction {
	public MosterData moster_to_battle;
	public float attack_min_delay = 4f;
	public float attack_max_delay = 6f;
	public Skill light_skill_availaible;
	public Skill dark_skill_availaible;
	public Skill fire_skill_availaible;
	public SequentialEventValidate explain_timeline;
	public SequentialEventValidate explain_on_player_take_damage;
	public SequentialEventValidate explain_enemy_launched_change_element;
	public SequentialEventValidate explain_enemy_changed_to_void;
	public SequentialEventValidate explain_weak_affinities;
	public SequentialEventValidate explain_strong_affinities;
	public SequentialEventValidate explain_normal_affinities;
	public SequentialEventValidate explain_atk_def_deadlock;

	int enemy_routine_step = 0;

	protected override IEnumerable ActionList()
	{
		yield return StartCoroutine(_0());
		yield return StartCoroutine(_LAST());
	}

	IEnumerator _0()
	{
		BattleManager.instance.regular_enemy_routine = false;
		BattleManager.instance.StartBattle(moster_to_battle.moster_battle);
		Debug.LogWarning("deactivating all element for defense");
		for (int i = 0; i != (int) Element.Count; ++i)
			PlayerBattle.instance.availaible_defense_elements[i] = false;
		foreach(var skill in Player.instance.current_moster.skills)
			skill.available = false;
		light_skill_availaible.available = true;
		StartCoroutine(EnemyRoutine());
		yield return new WaitForSeconds(5f);

		Debug.LogWarning("Block: 'Timeline'");
		yield return StartCoroutine(explain_timeline.Coroutine_LaunchStartSequence());
		StartCoroutine(Coroutine_CheckPlayerTakeDamage());
		Debug.LogWarning("Highlight: 'Attack'");
		Debug.LogWarning("WAITING FOR 'enemy take damage'");
		while (PlayerBattle.instance.last_skill_effect_applied == null)
			yield return new WaitForSeconds(0.01f);
		Debug.LogWarning("WAITING FOR 'player launch another skill'");
		while (PlayerBattle.instance.is_casting_skill == true)
			yield return new WaitForSeconds(0.01f);
		while (PlayerBattle.instance.is_casting_skill == false)
			yield return new WaitForSeconds(0.01f);
		enemy_routine_step = 1;
		yield return new WaitForSeconds(attack_max_delay + 0.5f);
		Debug.LogWarning("block: 'enemy launched change type'");
		yield return StartCoroutine(explain_enemy_launched_change_element.Coroutine_LaunchStartSequence());

		yield return StartCoroutine(Coroutine_WaitEnemyChangeElement(Element.DARK));
//		Debug.LogWarning("WAITING FOR 'enemy type changed to VOID'");
//		while (!(BattleManager.instance.enemy_has_element == true && BattleManager.instance.enemy_current_element == Element.DARK))
//			yield return new WaitForSeconds(0.01f);

		Debug.LogWarning("block: 'enemy has changed type to VOID'");
		PlayerBattle.instance.last_skill_effect_applied = null;
		yield return StartCoroutine(explain_enemy_changed_to_void.Coroutine_LaunchStartSequence());

		yield return StartCoroutine(Coroutine_WaitEnemyTakeDamageInElement(Element.LIGHT));
//		Debug.LogWarning("WAITING FOR 'enemy take ABS damage'");
//		while (!(PlayerBattle.instance.last_skill_effect_applied != null &&
//		       PlayerBattle.instance.last_skill_effect_applied.skill.element == Element.LIGHT))
//			yield return new WaitForSeconds(0.01f);

		Debug.LogWarning("block: 'ABS weak against VOID'");
		yield return StartCoroutine(explain_weak_affinities.Coroutine_LaunchStartSequence());
		Debug.LogWarning("Highlight: Fire attack");
		fire_skill_availaible.available = true;

		yield return StartCoroutine(Coroutine_WaitEnemyTakeDamageInElement(Element.FIRE));
//		Debug.LogWarning("WAITING FOR 'enemy take FIRE damage'");
//		while (PlayerBattle.instance.last_skill_effect_applied.skill.element != Element.FIRE)
//			yield return new WaitForSeconds(0.01f);

		Debug.LogWarning("block: 'FIRE strong against VOID'");
		yield return StartCoroutine(explain_strong_affinities.Coroutine_LaunchStartSequence());
		enemy_routine_step = 2;

		yield return StartCoroutine(Coroutine_WaitEnemyChangeElement(Element.LIGHT));
//		Debug.LogWarning("WAITING FOR 'enemy change to ABS defense");
//		while (!(BattleManager.instance.enemy_has_element == true && BattleManager.instance.enemy_current_element == Element.LIGHT))
//			yield return new WaitForSeconds(0.01f);

		yield return StartCoroutine(Coroutine_WaitEnemyTakeDamageInElement(Element.LIGHT));
//		Debug.LogWarning("WAITING FOR 'enemy take ABS'");
//		while (!(PlayerBattle.instance.last_skill_effect_applied != null &&
//		         PlayerBattle.instance.last_skill_effect_applied.skill.element == Element.LIGHT))
//			yield return new WaitForSeconds(0.01f);
		Debug.LogWarning("block: 'ABS is normal against ABS'");
		yield return StartCoroutine(explain_normal_affinities.Coroutine_LaunchStartSequence());
		dark_skill_availaible.available = true;
		Debug.LogWarning("Highlight: Void attack");
		yield return StartCoroutine(Coroutine_WaitEnemyTakeDamageInElement(Element.DARK));
		yield return new WaitForSeconds(2f);
//		Debug.LogWarning("WAITING FOR 'enemy take VOID damage'");
//		while (PlayerBattle.instance.last_skill_effect_applied.skill.element != Element.DARK)
//			yield return new WaitForSeconds(0.01f);
		PlayerBattle.instance.availaible_defense_elements[(int)Element.DARK] = true;
		Debug.LogWarning("Highlight: 'you can change your element too'");
		yield return StartCoroutine(Coroutine_WaitPlayerChangeElementTo(Element.DARK));

//		Debug.LogWarning("WAITING FOR 'player change to VOID'");
//		while (!(PlayerBattle.instance.has_element == true && PlayerBattle.instance.current_element == Element.DARK))
//			yield return new WaitForSeconds(0.01f);

		Debug.LogWarning("Block: 'can't attack and defend with the same type'");
		yield return StartCoroutine(explain_atk_def_deadlock.Coroutine_LaunchStartSequence());
		enemy_routine_step = 3;
		Debug.LogWarning("WAITING FOR 'player take ROCK damage while being VOID'");
		while (!(PlayerBattle.instance.has_element == true && PlayerBattle.instance.current_element == Element.DARK &&
		         BattleManager.instance.last_enemy_attacked_applied.element == Element.ROCK))
			yield return new WaitForSeconds(0.01f);
		Debug.LogWarning("Highlight: 'you can go back to neutral if you want'");
		ForceDoNextAction();
	}
	IEnumerator Coroutine_CheckPlayerTakeDamage()
	{
		while (BattleManager.instance.last_enemy_attacked_applied == null)
			yield return new WaitForSeconds(0.01f);
		Debug.LogWarning("Block: 'Explain shield'");
		yield return StartCoroutine(explain_on_player_take_damage.Coroutine_LaunchStartSequence());
	}

	IEnumerator Coroutine_WaitEnemyTakeDamageInElement(Element element)
	{
		PlayerBattle.instance.last_skill_effect_applied = null;
		Debug.LogWarning("WAITING FOR 'enemy take " + element + "damage'");
		while (!(PlayerBattle.instance.last_skill_effect_applied != null &&
		         PlayerBattle.instance.last_skill_effect_applied.skill.element == element))
			yield return new WaitForSeconds(0.01f);
	}
	IEnumerator Coroutine_WaitEnemyChangeElement(Element element)
	{
		Debug.LogWarning("WAITING FOR 'enemy change to " + element + "'");
		while (!(BattleManager.instance.enemy_has_element == true && BattleManager.instance.enemy_current_element == element))
			yield return new WaitForSeconds(0.01f);
	}
	IEnumerator Coroutine_WaitPlayerLaunchAttack(Element element)
	{
		Debug.LogWarning("WAITING FOR 'player launch skill" + element + "'");
		while (!(PlayerBattle.instance.is_casting_skill == true && PlayerBattle.instance.casted_skill.element == element))
			yield return new WaitForSeconds(0.01f);

	}

	IEnumerator Coroutine_WaitPlayerChangeElementTo(Element element)
	{
		Debug.LogWarning("WAITING FOR 'player change element to");
		while (!(PlayerBattle.instance.has_element == true && PlayerBattle.instance.current_element == element))
			yield return new WaitForSeconds(0.01f);
	}
	IEnumerator _1()
	{
//		Debug.LogWarning("waiting for M");
//		while (Input.GetKeyDown(KeyCode.M) == false)
//			yield return new WaitForSeconds(0.001f);
//		
//		ForceDoNextAction();
		yield return null;
	}
	IEnumerator _LAST()
	{
		Debug.LogWarning("reactivating all element for defense");
		BattleManager.instance.regular_enemy_routine = true;
		for (int i = 0; i != (int) Element.Count; ++i)
			PlayerBattle.instance.availaible_defense_elements[i] = true;
		foreach(var skill in Player.instance.current_moster.skills)
			skill.available = true;
		yield return new WaitForSeconds(0.001f);
	}

	IEnumerator EnemyRoutine()
	{
		Debug.LogWarning("Launching: only Light attack");
		while (enemy_routine_step == 0)
		{
			BattleManager.instance.ScheduleEnemyAttack( BattleManager.instance.enemy_moster.GetAttack(Element.LIGHT));
			yield return new WaitForSeconds(Random.Range(attack_min_delay, attack_max_delay));
		}
		Debug.LogWarning("Launching: Change element to: Dark");
		BattleManager.instance.ScheduleEnemyChangeElement(Element.DARK);
		yield return new WaitForSeconds(Random.Range(attack_min_delay, attack_max_delay));
		while (enemy_routine_step == 1)
		{
			BattleManager.instance.ScheduleEnemyAttack( BattleManager.instance.enemy_moster.GetAttack(Element.LIGHT));
			yield return new WaitForSeconds(Random.Range(attack_min_delay, attack_max_delay));
		}
		Debug.LogWarning("Launching: Change element to: Light");
		BattleManager.instance.ScheduleEnemyChangeElement(Element.LIGHT);
		yield return new WaitForSeconds(Random.Range(attack_min_delay, attack_max_delay));
		while (enemy_routine_step == 2)
		{
			BattleManager.instance.ScheduleEnemyAttack( BattleManager.instance.enemy_moster.GetAttack(Element.LIGHT));
			yield return new WaitForSeconds(Random.Range(attack_min_delay, attack_max_delay));
		}
		Debug.LogWarning("Launching: only Rock attacks");
		while (enemy_routine_step == 3)
		{
			BattleManager.instance.ScheduleEnemyAttack( BattleManager.instance.enemy_moster.GetAttack(Element.ROCK));
			yield return new WaitForSeconds(Random.Range(attack_min_delay, attack_max_delay));
		}

	}
//	IEnumerator ForceEnemyDefense(Element element)
//	{
//		while (true)
//		{
//			BattleManager.instance.enemy_moster.current_defense_elements_list.Clear();
//			BattleManager.instance.enemy_moster.current_defense_elements_list.Add(element);
//			yield return new WaitForSeconds(0.001f);
//		}
//	}
//	void MakeForceEnemyAttacks(Element element_attack, Element element_defense)
//	{
//		BattleManager.instance.enemy_moster.current_defense_elements_list.Clear();
//		BattleManager.instance.enemy_moster.current_defense_elements_list.Add(element);
//		StartCoroutine("ForceEnemyAttacks", element);
//	}
//	void MakeForceEnemyAttackStopAndDefen(Element element)
//	{
//
//	}
//	IEnumerator _ForceEnemyDefenseCoroutine()
//	{
//		StopCoroutine("ForceEnemyAttacks");
//		yield return new WaitForSeconds(0.1f);
//		StartCoroutine("ForceEnemyDefense", element);
//		yield return new WaitForSeconds(0.1f);
//	}
}
