
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillButton : MonoBehaviour
{
	public UILabel label;

	public UIButton button;
	
	public Skill sk;

	void Start()
	{
		button = this.GetComponent<UIButton> ();
	}
	
	
	void Update()
	{
		if (sk != null)
		{
			label.text = sk.skill_name;
			button.isEnabled = PlayerBattle.instance.IsSkillAvailable(sk);
		}
	}

	void OnClick()
	{
		if (sk != null)
			PlayerBattle.instance.ClickOnSkill (sk);
	}
	
}