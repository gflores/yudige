
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
			label.color = sk.is_consumed ? Color.gray : Color.white;


			
		}
	}

	void OnClick()
	{
		if (StateManager.instance.current_states.Contains(StateManager.State.BATTLE) && !StateManager.instance.current_states.Contains(StateManager.State.SCRIPTED_EVENT)
		    && sk != null && button.isEnabled && sk.is_consumed == false)
			PlayerBattle.instance.ClickOnSkill (sk);
	}

	void OnHover(bool isOver)
	{
		BattleScreen.instance.SkillTimelinePreview(isOver ? sk : null);
	}
	
}