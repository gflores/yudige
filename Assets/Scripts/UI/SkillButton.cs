
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillButton : MonoBehaviour
{
	public UILabel label;
	
	public Skill sk;
	
	
	void Update()
	{
		if (sk != null)
		{
			label.text = sk.skill_name;
		} else
		{
			label.text = "";

		}
	}

	void OnClick()
	{
		if (sk != null)
			PlayerBattle.instance.ClickOnSkill (sk);
	}
}