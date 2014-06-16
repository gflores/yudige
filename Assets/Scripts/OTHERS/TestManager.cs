using UnityEngine;
using System.Collections;

public class TestManager : MonoBehaviour {
	public Skill skill_to_test;
	public SkillEffects tested_skill_effects;
	public NonPlayerMoster enemy_moster_to_test;
	public MosterData player_moster_to_evolve_test;
	public Element battle_selected_element_for_defense;

	void Update () {
		if (Input.GetKey(KeyCode.LeftShift))
	    {
			if (Input.GetKeyDown(KeyCode.A))
			{
				Debug.LogWarning("TEST skill effect");
				tested_skill_effects = skill_to_test.GetEffects();
			}
			if (Input.GetKeyDown(KeyCode.Z))
			{
				Debug.LogWarning("TEST schedule skill");
				PlayerBattle.instance.ClickOnSkill(skill_to_test);
			}
			if (Input.GetKeyDown(KeyCode.E))
			{
				Debug.LogWarning("TEST start battle");
				BattleManager.instance.StartBattle(enemy_moster_to_test);
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				Debug.LogWarning("TEST player evolving to: " + player_moster_to_evolve_test.moster_name);
				Player.instance.EvolveTo(player_moster_to_evolve_test);
			}
			if (Input.GetKeyDown(KeyCode.Q))
			{
				Debug.LogWarning("TEST click on change element: " + battle_selected_element_for_defense);
				PlayerBattle.instance.ClickOnElementDefense(battle_selected_element_for_defense);

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
			Debug.LogWarning("Save reset !!");
			SaveManager.instance.save_data = SaveManager.instance.GetNewSaveData();
			SaveManager.instance.LaunchSave();
		}
	}
}
