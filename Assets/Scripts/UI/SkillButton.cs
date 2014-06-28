
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillButton : MonoBehaviour
{
	public UILabel label;

	public UIButton button;

	public SkillPanel parent;
	
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
			button.disabledColor = sk.is_consumed ? Color.red : Color.gray ;
			button.isEnabled = PlayerBattle.instance.IsSkillAvailable(sk) && sk.is_consumed == false;


			
		}
	}

	void OnClick()
	{
		if (sk != null && button.isEnabled)
			PlayerBattle.instance.ClickOnSkill (sk);
	}

	void OnHover(bool isOver)
	{
		parent.DisplayPreview(isOver);
		BattleScreen.instance.SkillTimelinePreview(isOver ? sk : null);
	}
	
}