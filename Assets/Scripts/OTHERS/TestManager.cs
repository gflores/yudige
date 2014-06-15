using UnityEngine;
using System.Collections;

public class TestManager : MonoBehaviour {
	public Skill skill_to_test;
	public SkillEffects tested_skill_effects;
	public NonPlayerMoster enemy_moster_to_test;
	public MosterData player_moster_to_evolve_test;
	
	void Update () {
		if (Input.GetKey(KeyCode.LeftShift))
	    {
			if (Input.GetKeyDown(KeyCode.A))
			{
				tested_skill_effects = skill_to_test.GetEffects();
				Debug.LogWarning("TEST skill effect");
			}
			if (Input.GetKeyDown(KeyCode.Z))
			{
				BattleManager.instance.SchedulePlayerAttack(skill_to_test);
				Debug.LogWarning("TEST schedule skill");
			}
			if (Input.GetKeyDown(KeyCode.E))
			{
				BattleManager.instance.StartBattle(enemy_moster_to_test);
				Debug.LogWarning("TEST start battle");
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				Player.instance.EvolveTo(player_moster_to_evolve_test);
				Debug.LogWarning("TEST player evolving to: " + player_moster_to_evolve_test.moster_name);
			}
		}
		if (Input.GetKeyDown(KeyCode.F1))
		{
			SaveManager.instance.LaunchSave();
		}
		if (Input.GetKeyDown(KeyCode.F2))
		{
			SaveManager.instance.LaunchLoad();
		}
		if (Input.GetKeyDown(KeyCode.F3))
		{
			SaveManager.instance.save_data = SaveManager.instance.GetNewSaveData();
			SaveManager.instance.LaunchSave();
			Debug.LogWarning("Save reset !!");
		}
	}
}
