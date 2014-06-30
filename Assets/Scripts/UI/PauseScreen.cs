
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseScreen : MonoBehaviour
{
	public SkillInfo element1slot1;
	public SkillInfo element1slot2;
	public SkillInfo element1slot3;
	public SkillInfo element2slot1;
	public SkillInfo element2slot2;
	public SkillInfo element2slot3;
	public SkillInfo element3slot1;
	public SkillInfo element3slot2;
	public SkillInfo element3slot3;
	public SkillInfo element4slot1;
	public SkillInfo element4slot2;
	public SkillInfo element4slot3;
	public UILabel life_label;
	public UILabel shield_label;
	public UILabel name_label;



	void FixedUpdate()
	{
		life_label.text = "Health : " + Player.instance.current_life;
		shield_label.text = "Shield : " + Player.instance.GetEffectiveMaxShield();
		name_label.text = Player.instance.current_moster.moster_name;

		SetSkillInfo();
	}

	private void SetSkillInfo()
	{
		List<Skill> skills;
		skills = PlayerBattle.instance.GenerateSkillsAvailable (Element.DARK);
		element1slot1.sk = (skills.Count > 0) ? skills [0] : null;
		element1slot2.sk = (skills.Count > 1) ? skills [1] : null;
		element1slot3.sk = (skills.Count > 2) ? skills [2] : null;
		element1slot1.gameObject.SetActive (element1slot1.sk != null);
		element1slot2.gameObject.SetActive (element1slot2.sk != null);
		element1slot3.gameObject.SetActive (element1slot3.sk != null);

		skills = PlayerBattle.instance.GenerateSkillsAvailable (Element.LIGHT);
		element2slot1.sk = (skills.Count > 0) ? skills [0] : null;
		element2slot2.sk = (skills.Count > 1) ? skills [1] : null;
		element2slot3.sk = (skills.Count > 2) ? skills [2] : null;
		element2slot1.gameObject.SetActive (element2slot1.sk != null);
		element2slot2.gameObject.SetActive (element2slot2.sk != null);
		element2slot3.gameObject.SetActive (element2slot3.sk != null);

		skills = PlayerBattle.instance.GenerateSkillsAvailable (Element.ROCK);
		element3slot1.sk = (skills.Count > 0) ? skills [0] : null;
		element3slot2.sk = (skills.Count > 1) ? skills [1] : null;
		element3slot3.sk = (skills.Count > 2) ? skills [2] : null;
		element3slot1.gameObject.SetActive (element3slot1.sk != null);
		element3slot2.gameObject.SetActive (element3slot2.sk != null);
		element3slot3.gameObject.SetActive (element3slot3.sk != null);

		skills = PlayerBattle.instance.GenerateSkillsAvailable (Element.FIRE);
		element4slot1.sk = (skills.Count > 0) ? skills [0] : null;
		element4slot2.sk = (skills.Count > 1) ? skills [1] : null;
		element4slot3.sk = (skills.Count > 2) ? skills [2] : null;
		element4slot1.gameObject.SetActive (element4slot1.sk != null);
		element4slot2.gameObject.SetActive (element4slot2.sk != null);
		element4slot3.gameObject.SetActive (element4slot3.sk != null);

		
	}

	void OnTitleScreen()
	{
		Application.LoadLevel("main_menu");
	}

	void OnExit()
	{
		Debug.Log ("peace out");
		Application.Quit();
	}
}