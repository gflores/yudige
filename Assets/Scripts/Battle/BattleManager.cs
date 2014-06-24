using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {
	public float intro_battle_exploration_time = 1f;
	public float intro_in_battle_time = 1f;
	public Transform player_visual_transform;
	public Transform enemy_visual_transform;

	public static BattleManager instance {get; set;}

	public NpcMosterBattle enemy_moster {get; set;}

	public bool enemy_has_element {get; set;}
	public Element enemy_current_element {get; set;}
	public int enemy_current_life {get; set;}
	public EnemyAttack scheduled_enemy_attack {get; set;}
	public EnemyAttack last_enemy_attacked_applied {get; set;}
	void Awake () {
		instance = this;
	}

	void Start(){
		PlayerBattle.instance.attacks_FX = new List<ParticleSystem>();
		foreach(var fx in attacks_FX)
		{
			GameObject go = (GameObject)Instantiate(fx.gameObject);
			PlayerBattle.instance.attacks_FX.Add(go.GetComponent<ParticleSystem>());
		}
	}
	//HELPERS BEGIN
	public bool IsBattleAlreadyLaunched()
	{
		return StateManager.instance.current_states.Contains(StateManager.State.BATTLE);
	}
	//HELPERS END


	//BATTLE MANAGEMENT BEGIN
	public void StartBattle(NpcMosterBattle n_enemy_moster)
	{
		enemy_moster = n_enemy_moster;
		LaunchStartBattle();
	}

	public void LaunchStartBattle()
	{
		if (IsBattleAlreadyLaunched() == true)
			return ;
		last_enemy_attacked_applied = null;
		StateManager.instance.current_states.Add(StateManager.State.SCRIPTED_EVENT);
		StateManager.instance.UpdateFromStates();
		StartCoroutine(Coroutine_StartBattle());
		CameraManager.instance.battle_plane_behind_player_animation.GetComponent<SpriteRenderer>().color = Color.clear;
		enemy_moster.main_renderer.color = Color.white;
	}
	IEnumerator Coroutine_IntroBattleExplorationFlash()
	{
		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToTransparent(intro_battle_exploration_time/2));
	}
	IEnumerator Coroutine_StartBattle()
	{
		SoundManager.instance.PlayIndependant(SoundManager.instance.battle_start_sound);
		SoundManager.instance.PlayIfDifferent(enemy_moster.battle_theme);
		Vector3 previous_position = CameraManager.instance.exploration_camera.transform.position;
		Vector3 dest_camera_position = (enemy_moster.moster_data.moster_exploration.main_renderer.transform.position + 
		                                PlayerExploration.instance.main_renderer.transform.position) / 2;
		dest_camera_position.z = previous_position.z;
		float initial_exploration_camera_size = CameraManager.instance.exploration_camera.orthographicSize;
		Debug.LogWarning("init size: " + initial_exploration_camera_size);

		CameraManager.instance.SetColorToFadePlane(Color.white);

		yield return StartCoroutine(Coroutine_IntroBattleExplorationFlash());
		TweenPosition.Begin(CameraManager.instance.exploration_camera.gameObject, intro_battle_exploration_time / 2, dest_camera_position);
		StartCoroutine(CameraManager.instance.COROUTINE_LaunchExplorationCameraStartBattleAnimation(intro_battle_exploration_time / 2 - 0.1f));
		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToOpaque(intro_battle_exploration_time/2 +0.1f));

		//ExplorationIntro finished
		foreach(var defense_FX in defenses_FX)
			defense_FX.Stop();
		foreach(var attack_FX in attacks_FX)
			attack_FX.Stop();

		enemy_moster.SetupForBattle();
		PlayerBattle.instance.SetupForBattle();
		BattleScreen.instance.SetupForBattle();
		if (regular_enemy_routine == true)
			StartCoroutine(Coroutine_EnemyLoop());
		StateManager.instance.current_states.Remove(StateManager.State.EXPLORATION);
		StateManager.instance.current_states.Remove(StateManager.State.SCRIPTED_EVENT);
		StateManager.instance.current_states.Add(StateManager.State.BATTLE);
		StateManager.instance.UpdateFromStates();

		PlayerBattle.instance.visuals_transform.position = player_visual_transform.position;
		Rotater2D.LookAt(PlayerBattle.instance.visuals_transform, player_visual_transform.position + Vector3.left);
		enemy_moster.visuals_transform.position = enemy_visual_transform.position;
		Rotater2D.LookAt(enemy_moster.visuals_transform, enemy_visual_transform.position + Vector3.right);
		//BattleIntroStarting
//		float initial_battle_camera_size = CameraManager.instance.battle_camera.orthographicSize;

		//BattleIntro finished
		//ExplorationIntro cleanup
		yield return new WaitForSeconds(0.1f);
		CameraManager.instance.exploration_camera.orthographicSize = initial_exploration_camera_size;
		CameraManager.instance.exploration_camera.transform.position = previous_position;

		//START FADE IN
		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToTransparent(intro_in_battle_time));

		yield return new WaitForSeconds(0.001f);
	}
	//BATTLE MANAGEMENT END

	//ENEMY ACTIONS BEGIN
	public bool regular_enemy_routine = true;
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
			if (enemy_has_element == true)
			{
				scheduled_enemy_attack = enemy_moster.GetBurstAttack(enemy_current_element);
				ScheduleEnemyAttack(scheduled_enemy_attack);
				yield return new WaitForSeconds(enemy_moster.before_change_element_time);
			}
			ScheduleEnemyChangeElement(enemy_moster.GetNextElement());
		}
	}
	public void ScheduleEnemyChangeElement(Element element)
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
	public void ScheduleEnemyAttack(EnemyAttack attack_to_schedule)
	{
		TimelineEvent timeline_event_to_schedule;
		
		timeline_event_to_schedule = new TimelineEvent();
		timeline_event_to_schedule.name = attack_to_schedule.damage.ToString();
		timeline_event_to_schedule.time_remaining = EventsTimeline.instance.total_time;
		timeline_event_to_schedule.on_complete_routine = Coroutine_EnemyAttackEffects(attack_to_schedule);
		EventsTimeline.instance.Schedule(timeline_event_to_schedule);
	}
	public List<ParticleSystem> attacks_FX;
	public Transform battle_left_side_point;
	public Transform battle_right_side_point;
	float life_time_delta_for_burst_attack = 0.5f;
	float speed_delta_for_burst_attack = 1f;
	float extra_time_for_burst_attack = 1f;
	float extra_size_for_burst_attack = 0.5f;

	IEnumerator Coroutine_AttackFX(ParticleSystem attack_FX, bool is_burst_attack)
	{
		attack_FX.transform.position = battle_left_side_point.position;
		if (is_burst_attack == true)
		{
			attack_FX.startLifetime += life_time_delta_for_burst_attack;
			attack_FX.startSpeed += speed_delta_for_burst_attack;
			attack_FX.startSize += extra_size_for_burst_attack;
		}
		attack_FX.Play();
		SoundManager.instance.PlayIndependant(SoundManager.instance.attack_start_sound);
		Vector3 dest_position = battle_right_side_point.position;
		dest_position.x = player_visual_transform.position.x;
		if (is_burst_attack == true)
		{
			TweenPosition.Begin(attack_FX.gameObject, 0.5f + extra_time_for_burst_attack, dest_position);
			yield return new WaitForSeconds(extra_time_for_burst_attack);
		}
		else
		{
			TweenPosition.Begin(attack_FX.gameObject, 0.5f, dest_position);
			yield return new WaitForSeconds(0.2f);
		}
		attack_FX.Stop();

	}
	IEnumerator Coroutine_EnemyAttackEffects(EnemyAttack enemy_attack)
	{
		ParticleSystem attack_FX = attacks_FX[(int)enemy_attack.element];
		StartCoroutine(Coroutine_AttackFX(attack_FX, enemy_attack.is_burst));
		if (enemy_attack.is_burst == true)
		{
			Debug.LogWarning("Boss attack changed to NEUTRAL !");
			enemy_has_element = false;
			foreach(var defense_FX in defenses_FX)
				defense_FX.Stop();
			SoundManager.instance.PlayIndependant(SoundManager.instance.remove_element_sound);
			yield return new WaitForSeconds(0.5f + extra_time_for_burst_attack);
		}
		else
			yield return new WaitForSeconds(0.5f);
		if (enemy_attack.is_burst == true)
		{
			attack_FX.startLifetime -= life_time_delta_for_burst_attack;
			attack_FX.startSpeed -= speed_delta_for_burst_attack;
			attack_FX.startSize -= extra_size_for_burst_attack;
		}

		last_enemy_attacked_applied = enemy_attack;
		if (PlayerBattle.instance.has_element == true)
		{
			ElementRelation element_relation = ElementManager.instance.GetRelationBetween(
				enemy_attack.element, PlayerBattle.instance.current_element
			);
			int current_affinity = PlayerBattle.instance.GetCurrentElementAffinity();
			switch (element_relation)
			{
			case ElementRelation.NORMAL:
				SoundManager.instance.PlayIndependant(SoundManager.instance.normal_damage_sound);
				StartCoroutine(SpecialEffectsManager.instance.normal_damage_shake.LaunchShake(PlayerBattle.instance.visuals_transform));
				PlayerBattle.instance.TakeDamage(enemy_attack.damage - current_affinity);
				break;
			case ElementRelation.STRONG:
				SoundManager.instance.PlayIndependant(SoundManager.instance.strong_damage_sound);
				StartCoroutine(SpecialEffectsManager.instance.critical_damage_shake.LaunchShake(PlayerBattle.instance.visuals_transform));
				PlayerBattle.instance.TakeDamage((enemy_attack.damage * 2) + current_affinity);
				break;
			case ElementRelation.WEAK:
				SoundManager.instance.PlayIndependant(SoundManager.instance.weak_damage_sound);
				PlayerBattle.instance.generic_animator.SetTrigger("StayStill");
				PlayerBattle.instance.TakeDamage((enemy_attack.damage / 2) - current_affinity);
				break;
			}
			Debug.LogWarning("Player took damage, element_relation:" + element_relation);
		}
		else
		{
			SoundManager.instance.PlayIndependant(SoundManager.instance.normal_damage_sound);
			StartCoroutine(SpecialEffectsManager.instance.normal_damage_shake.LaunchShake(PlayerBattle.instance.visuals_transform));
			PlayerBattle.instance.TakeDamage(enemy_attack.damage);
			//NEUTRAL
		}
		Debug.LogWarning("enemy_attack: base_damage:" + enemy_attack.damage);
		Debug.LogWarning("new_player_life: " + Player.instance.current_life);

		Player.instance.CheckDeath();
		yield return new WaitForSeconds(0.001f);
	}
	public List<ParticleSystem> defenses_FX;
	public void EnemyChangeElement(Element new_element)
	{
		ParticleSystem defense_fx = defenses_FX[(int) new_element];

		SoundManager.instance.PlayIndependant(SoundManager.instance.change_element_sound);
		foreach(var defense_FX in defenses_FX)
			defense_FX.Stop();
		defense_fx.Play();
		if (enemy_has_element == true)
		Debug.LogWarning("enemy change element from: " + enemy_current_element + "to: " + new_element);
		else
			Debug.LogWarning("enemy change element from NEUTRAL to: " + new_element);
		enemy_has_element = true;
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
		InteruptBattleCoroutines();
		StateManager.instance.current_states.Add(StateManager.State.SCRIPTED_EVENT);
		StateManager.instance.UpdateFromStates();
		CameraManager.instance.battle_gui_camera.enabled = false;
		StartCoroutine(Coroutine_EnemyDeathScene());
	}

	IEnumerator Coroutine_EnemyDeathScene()
	{
		SoundManager.instance.PlayIfDifferent(SoundManager.instance.enemy_death_music);
		SpecialEffectsManager.instance.moster_transformation_shake.time_ratio = 1 / 4f;
		StartCoroutine(SpecialEffectsManager.instance.moster_transformation_shake.LaunchShake(enemy_moster.visuals_transform));
		enemy_moster.generic_animator.SetBool("IsSick", true);
		SoundManager.instance.PlayIndependant(SoundManager.instance.death_sfx);
		yield return StartCoroutine(SpecialEffectsManager.instance.Coroutine_StartTweenAlpha(
			enemy_moster.main_renderer,
			1f, 0f, 2f));
		
		yield return new WaitForSeconds(2f);
		enemy_moster.moster_data.moster_exploration.gameObject.SetActive(false);

		enemy_moster.generic_animator.SetBool("IsSick", false);

		Player.instance.current_karma += enemy_moster.karma_points_rewards;
		MostersManager.instance.AddToEliminated(enemy_moster.moster_data);
		Debug.LogWarning("ENEMY IS DEAD !!");

		StateManager.instance.current_states.Remove(StateManager.State.BATTLE);
		StateManager.instance.current_states.Add(StateManager.State.EXPLORATION);
		StateManager.instance.UpdateFromStates();

		StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToTransparent(1f));
		yield return StartCoroutine(CameraManager.instance.COROUTINE_LaunchExplorationCameraEndBattleAnimation(1f));
		GameManager.instance.current_screen.MakeGoTo();
		StateManager.instance.current_states.Remove(StateManager.State.SCRIPTED_EVENT);
		StateManager.instance.UpdateFromStates();
		EndBattle();
	}
	public void ForceEnemyDeath(){
		OnEnemyDeath();
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
		Player.instance.LaunchDeath();
	}
	public IEnumerator Coroutine_PlayerBattleDeathRoutine()
	{
		yield return new WaitForSeconds(0.01f);
	}
	public void InteruptBattleCoroutines()
	{
		foreach(var atk in PlayerBattle.instance.attacks_FX)
			atk.Stop();
		foreach(var atk in attacks_FX)
			atk.Stop();
		foreach(var atk in PlayerBattle.instance.defenses_FX)
			atk.Stop();
		foreach(var atk in defenses_FX)
			atk.Stop();
		StopAllCoroutines();
		PlayerBattle.instance.StopAllCoroutines();
		EventsTimeline.instance.StopAllCoroutines();
	}
	public void EndBattle()
	{
		enemy_moster.visuals_transform.position = new Vector2(-99999, 999999);
		PlayerBattle.instance.is_shield_live = false;
		InteruptBattleCoroutines();
		StateManager.instance.current_states.Remove(StateManager.State.BATTLE);
		StateManager.instance.current_states.Add(StateManager.State.EXPLORATION);
		StateManager.instance.UpdateFromStates();
	}
	//PLAYER ACTIONS END
}
