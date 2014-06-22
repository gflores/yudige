using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {
	public string skill_name;
	public Element element;
	public int cost = 1;
	public bool deals_damage = true;
	public float damage_ratio = 1f;
	public float cast_time = 1f;
	public bool availaible = true;

	public SkillEffects GetEffects()
	{
		SkillEffects effects = new SkillEffects();

		effects.deals_damage = deals_damage;
//		Player.instance

		float tmp_damages = (float)Player.instance.GetEffectiveElementAffinity(element) * damage_ratio;
		effects.damages = Mathf.CeilToInt(tmp_damages);
		effects.skill = this;
		return effects;
	}

}

[System.Serializable]
public class SkillEffects {
	public Skill skill;
	public bool deals_damage = true;
	public int damages;
}