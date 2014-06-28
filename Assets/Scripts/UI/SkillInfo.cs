using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillInfo : MonoBehaviour
{
	public UISprite background;
	public UILabel name_label;
	public UILabel cast_label;
	public UILabel damage_label;
	public UILabel bonus_label;

	public Skill sk;

	void FixedUpdate()
	{
		if (sk != null)
		{
			name_label.text = sk.skill_name;
			cast_label.text = "Cast time : " + sk.cast_time.ToString();
			damage_label.text = "Damage ratio : " +  sk.damage_ratio.ToString();
			bonus_label.text = "Bonus Ratio : " +  sk.combos_bonus_affinity_ratio.ToString();
		}
	}

}