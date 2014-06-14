using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {
	public string skill_name;
	public Element skill_type;
	public int cost = 1;
	public bool deals_damage = true;
	public float damage_ratio = 1f;

	public SkillEffects GetEffects()
	{
		SkillEffects effects = new SkillEffects();

		effects.deals_damage = deals_damage;
//		effects.damages = 

		return effects;
	}

}
public class SkillEffects {
	public bool deals_damage = true;
	public bool damages;
}