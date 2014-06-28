using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutoScriptedBattle : SequentialAction {
	public MosterData moster_to_battle;
	public float attack_min_delay = 4f;
	public float attack_max_delay = 6f;
	public Skill light_skill_availaible;
	public Skill dark_skill_availaible;
	public Skill fire_skill_availaible;
	public SequentialEventValidate explain_wait_for_skill;
	public SequentialEventValidate explain_enemy_took_damage;
	public SequentialEventValidate explain_combos_bonus;
	public SequentialEventValidate explain_burst_attack;
	public SequentialEventValidate explain_burst_reset;
	public SequentialEventValidate explain_enemy_attack;
	public SequentialEventValidate explain_shield;
	public SequentialEventValidate explain_reset;
	public SequentialEventValidate explain_enemy_phase;

	//int enemy_routine_step = 0;
	List<Element> elements_played;

	protected override IEnumerable ActionList()
	{
		yield return StartCoroutine(_0());
//		yield return StartCoroutine(_LAST());
	}
	void DeactivateResetButton()
	{
		Debug.LogWarning("reset button deactivated");
	}
	void ReactivateResetButton()
	{
		Debug.LogWarning("reset button reactivated");
	}

	IEnumerator _0()
	{
		elements_played = new List<Element>();
		DeactivateResetButton();
		BattleManager.instance.pause_before_new_phase = true;
		BattleManager.instance.StartBattle(moster_to_battle.moster_battle);
//		foreach(var skill in Player.instance.current_moster.skills)
//			skill.available = false;
//		light_skill_availaible.available = true;
//		dark_skill_availaible.available = true;
//		fire_skill_availaible.available = true;
		Element skill_element;
		while (StateManager.instance.current_states.Contains(StateManager.State.BATTLE) == false)
			yield return null;
		yield return new WaitForSeconds(0.5f);

		//PREMIERE ATTAQUE
		Debug.LogWarning("Highlight: 'Attack'");
		PopupSmall.instance.text_label.text = "Select a skill";
		PopupSmall.instance.Show (0,0, 200, 50);
		yield return StartCoroutine(Coroutine_WaitPlayerLaunchAnyAttack());
		PopupSmall.instance.Hide();
		skill_element = PlayerBattle.instance.casted_skill.element;
		Debug.LogWarning("Block: 'when a skill has been used, its unavailable'");
		yield return StartCoroutine(explain_wait_for_skill.Coroutine_LaunchStartSequence());
		yield return StartCoroutine(Coroutine_WrappedWaitFromSkillToDamage(skill_element));

		//DEUXIEME ATTAQUE
		Debug.LogWarning("Highlight: 'note that your previous skill attack has been consumed once you used it. Choose another skill'");
		PopupSmall.instance.text_label.text = "Note that your previous skill has been consumed once you used it.\n" +
			"Choose another skill.";
		PopupSmall.instance.Show (0,0, 300, 100);
		yield return StartCoroutine(Coroutine_WaitPlayerLaunchAnyAttack());
		PopupSmall.instance.Hide();
		skill_element = PlayerBattle.instance.casted_skill.element;
		Debug.LogWarning("Block: 'explain combos_bonus'");
		yield return StartCoroutine(explain_combos_bonus.Coroutine_LaunchStartSequence());
		yield return StartCoroutine(Coroutine_WrappedWaitFromSkillToDamage(skill_element));

		//TROIXIEME ATTAQUE
		yield return StartCoroutine(Coroutine_WaitPlayerLaunchAnyAttack());
		skill_element = PlayerBattle.instance.casted_skill.element;
		yield return StartCoroutine(Coroutine_WrappedWaitFromSkillToDamage(skill_element));

		//QUATRIEME ATTAQUE
		yield return StartCoroutine(Coroutine_WaitPlayerLaunchAnyAttack());
		skill_element = PlayerBattle.instance.casted_skill.element;
		yield return StartCoroutine(Coroutine_WrappedWaitFromSkillToDamage(skill_element));

		//DERNIERE ATTAQUE
		yield return StartCoroutine(Coroutine_WaitPlayerLaunchAnyAttack());
		skill_element = PlayerBattle.instance.casted_skill.element;
		Debug.LogWarning("Block: 'explain burst attack'");
		yield return StartCoroutine(explain_burst_attack.Coroutine_LaunchStartSequence());
		yield return StartCoroutine(Coroutine_WrappedWaitFromSkillToDamage(skill_element));
		Debug.LogWarning("Block: 'when you burst, you reset'");
		yield return StartCoroutine(explain_burst_reset.Coroutine_LaunchStartSequence());

		yield return new WaitForSeconds(1f);
		BattleManager.instance.pause_before_new_phase = false;
		yield return new WaitForSeconds(2f);
		Debug.LogWarning("Block: 'this is an enemy attack'");
		yield return StartCoroutine(explain_enemy_attack.Coroutine_LaunchStartSequence());
		yield return StartCoroutine(Coroutine_CheckPlayerTakeDamage());
		yield return new WaitForSeconds(3f);

		Debug.LogWarning("Block: explain enemy phase");
		yield return StartCoroutine(explain_enemy_phase.Coroutine_LaunchStartSequence());

		yield return new WaitForSeconds(3f);
		StartCoroutine(Coroutine_CanUseReset());

		foreach(var skill in Player.instance.current_moster.skills)
			skill.available = true;

		ForceDoNextAction();
	}
	IEnumerator Coroutine_CanUseReset()
	{
		ReactivateResetButton();
		
		Debug.LogWarning("Highlight: 'you can use reset'");
		PopupSmall.instance.text_label.text = "You may also manually trigger the reset of your skills by using the \"Reset\" command.";
		PopupSmall.instance.Show (0,0, 300, 100);
		while (PlayerBattle.instance.using_reset == false)
			yield return null;
		PopupSmall.instance.Hide();
		Debug.LogWarning("Block: explain reset");
		yield return StartCoroutine(explain_reset.Coroutine_LaunchStartSequence());
	}
	IEnumerator Coroutine_CheckPlayerTakeDamage()
	{
		while (BattleManager.instance.last_enemy_attacked_applied == null)
			yield return new WaitForSeconds(0.01f);
		Debug.LogWarning("Block: 'Explain shield'");
		yield return StartCoroutine(explain_shield.Coroutine_LaunchStartSequence());
	}

//	IEnumerator Coroutine_WaitEnemyTakeDamageInElement(Element element)
//	{
//		PlayerBattle.instance.last_skill_effect_applied = null;
//		Debug.LogWarning("WAITING FOR 'enemy take " + element + "damage'");
//		while (!(PlayerBattle.instance.last_skill_effect_applied != null &&
//		         PlayerBattle.instance.last_skill_effect_applied.skill.element == element))
//			yield return new WaitForSeconds(0.01f);
//	}
	IEnumerator Coroutine_WaitEnemyTakeAnyDamage()
	{
		PlayerBattle.instance.skill_ended = false;

		Debug.LogWarning("WAITING FOR 'enemy take any damage'");
		while (PlayerBattle.instance.skill_ended == false)
			yield return new WaitForSeconds(0.001f);
	}
//	IEnumerator Coroutine_WaitEnemyChangeElement(Element element)
//	{
//		Debug.LogWarning("WAITING FOR 'enemy change to " + element + "'");
//		while (!(BattleManager.instance.enemy_has_element == true && BattleManager.instance.enemy_current_element == element))
//			yield return new WaitForSeconds(0.01f);
//	}
//	IEnumerator Coroutine_WaitPlayerLaunchAttack(Element element)
//	{
//		Debug.LogWarning("WAITING FOR 'player launch skill" + element + "'");
//		while (!(PlayerBattle.instance.is_casting_skill == true && PlayerBattle.instance.casted_skill.element == element))
//			yield return new WaitForSeconds(0.01f);
//
//	}
	IEnumerator Coroutine_WaitPlayerLaunchAnyAttack()
	{
		Debug.LogWarning("WAITING FOR 'player launch any skill'");
		while (!(PlayerBattle.instance.is_casting_skill == true))
			yield return new WaitForSeconds(0.01f);
		
	}

//	IEnumerator Coroutine_WaitPlayerChangeElementTo(Element element)
//	{
//		Debug.LogWarning("WAITING FOR 'player change element to");
//		while (!(PlayerBattle.instance.has_element == true && PlayerBattle.instance.current_element == element))
//			yield return new WaitForSeconds(0.01f);
//	}
//	IEnumerator _1()
//	{
//		Debug.LogWarning("waiting for M");
//		while (Input.GetKeyDown(KeyCode.M) == false)
//			yield return new WaitForSeconds(0.001f);
//		
//		ForceDoNextAction();
//		yield return null;
//	}
//	IEnumerator _LAST()
//	{
//		Debug.LogWarning("reactivating all element for defense");
//		BattleManager.instance.regular_enemy_routine = true;
//		for (int i = 0; i != (int) Element.Count; ++i)
//			PlayerBattle.instance.availaible_defense_elements[i] = true;
//		foreach(var skill in Player.instance.current_moster.skills)
//			skill.available = true;
//		yield return new WaitForSeconds(0.001f);
//	}
//
//	IEnumerator EnemyRoutine()
//	{
//		Debug.LogWarning("Launching: only Light attack");
//		while (enemy_routine_step == 0)
//		{
//			BattleManager.instance.ScheduleEnemyAttack( BattleManager.instance.enemy_moster.GetAttack(Element.LIGHT));
//			yield return new WaitForSeconds(Random.Range(attack_min_delay, attack_max_delay));
//		}
//		Debug.LogWarning("Launching: Change element to: Dark");
//		BattleManager.instance.ScheduleEnemyChangeElement(Element.DARK);
//		yield return new WaitForSeconds(Random.Range(attack_min_delay, attack_max_delay));
//		while (enemy_routine_step == 1)
//		{
//			BattleManager.instance.ScheduleEnemyAttack( BattleManager.instance.enemy_moster.GetAttack(Element.LIGHT));
//			yield return new WaitForSeconds(Random.Range(attack_min_delay, attack_max_delay));
//		}
//		Debug.LogWarning("Launching: Change element to: Light");
//		BattleManager.instance.ScheduleEnemyChangeElement(Element.LIGHT);
//		yield return new WaitForSeconds(Random.Range(attack_min_delay, attack_max_delay));
//		while (enemy_routine_step == 2)
//		{
//			BattleManager.instance.ScheduleEnemyAttack( BattleManager.instance.enemy_moster.GetAttack(Element.LIGHT));
//			yield return new WaitForSeconds(Random.Range(attack_min_delay, attack_max_delay));
//		}
//		Debug.LogWarning("Launching: only Rock attacks");
//		while (enemy_routine_step == 3)
//		{
//			BattleManager.instance.ScheduleEnemyAttack( BattleManager.instance.enemy_moster.GetAttack(Element.ROCK));
//			yield return new WaitForSeconds(Random.Range(attack_min_delay, attack_max_delay));
//		}
//
//	}

	IEnumerator Coroutine_WrappedWaitFromSkillToDamage(Element skill_element)
	{
		((ExplainEnemyTookDamage)explain_enemy_took_damage).player_power = PlayerBattle.instance.GetCurrentElementAffinity();
		while (PlayerBattle.instance.is_casting_skill == true)
			yield return new WaitForSeconds(0.001f);
		PlayerBattle.instance.is_casting_skill = true;
		yield return StartCoroutine(Coroutine_WaitEnemyTakeAnyDamage());
		((ExplainEnemyTookDamage)explain_enemy_took_damage).player_element = skill_element;
		((ExplainEnemyTookDamage)explain_enemy_took_damage).enemy_element = BattleManager.instance.enemy_current_element;
		((ExplainEnemyTookDamage)explain_enemy_took_damage).enemy_power = BattleManager.instance.enemy_moster.GetEffectiveAffinity(BattleManager.instance.enemy_current_element);
		yield return new WaitForSeconds(0.5f);
		PlayerBattle.instance.is_casting_skill = false;
		Element enemy_element = BattleManager.instance.enemy_current_element;
		if (elements_played.Contains(skill_element) == false)
		{
			elements_played.Add(skill_element);
			Debug.LogWarning("Block: 'Enemy took damage'");
			explain_enemy_took_damage.Reinit();
			yield return StartCoroutine(explain_enemy_took_damage.Coroutine_LaunchStartSequence());
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
