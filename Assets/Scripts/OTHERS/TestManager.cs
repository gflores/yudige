using UnityEngine;
using System.Collections;

public class TestManager : MonoBehaviour {
	public Skill skill_to_test;
	public SkillEffects tested_skill_effects;
	public NpcMosterBattle enemy_moster_to_test;
	public MosterData player_moster_to_evolve_test;
	public Element battle_selected_element_for_defense;
	public Element karma_boost_element_selected;
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
				Debug.LogWarning("TEST click on skill");
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
			if (Input.GetKeyDown(KeyCode.W))
			{
				Debug.LogWarning("Use karma on life");
				Player.instance.UseKarmaForLife();
			}
			if (Input.GetKeyDown(KeyCode.X))
			{
				Debug.LogWarning("Use karma on shield");
				Player.instance.UseKarmaForShield();
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				Debug.LogWarning("Use karma on element affinity: " + karma_boost_element_selected);
				Player.instance.UseKarmaForElementAffinity(karma_boost_element_selected);
			}
			if (Input.GetKeyDown(KeyCode.K))
			{
				Debug.LogWarning("Gain Karma ");
				Player.instance.current_karma += 1;
			}
		}
		if (Input.GetKey(KeyCode.RightShift))
		{
			if (Input.GetKey(KeyCode.A))
			{
				CameraManager.instance.SetColorToFadePlane(Color.red);
				 StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToTransparent());
			}
			if (Input.GetKey(KeyCode.Z))
			{
				CameraManager.instance.SetColorToFadePlane(Color.blue);
				StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToOpaque(3f));
			}
			if (Input.GetKey(KeyCode.D))
			{
				Debug.LogWarning("TEST: player death");
				Player.instance.OnDeath();
			}
		}

		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F1))
		{
			GameManager.instance.SetSaveDataFromGame();
		}
		else if (Input.GetKeyDown(KeyCode.F1))
		{
			SaveManager.instance.LaunchSave();
		}

		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F2))
		{
			GameManager.instance.SetSaveDataFromGame();
		}
		else if (Input.GetKeyDown(KeyCode.F2))
		{
			SaveManager.instance.LaunchLoad();
		}
		if (Input.GetKeyDown(KeyCode.F3))
		{
			Debug.LogWarning("Save reset !!");
			SaveManager.instance.save_data = SaveManager.instance.GetNewSaveData();
			SaveManager.instance.LaunchSave();
		}

		if (Input.GetKeyDown(KeyCode.F11))
		{
			Debug.LogWarning("SAVING GAME");
			GameManager.instance.SaveGame();
		}
		if (Input.GetKeyDown(KeyCode.F12))
		{
			Debug.LogWarning("LOADING GAME");
			GameManager.instance.LoadGame();
		}
	}
}
