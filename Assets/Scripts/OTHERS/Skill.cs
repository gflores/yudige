using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {
	public string skill_name;
	public Element element;
	public int cost = 1;
	public bool deals_damage = true;
	public float damage_ratio = 1f;
	public float combos_bonus_affinity_ratio = 1f;
	public float cast_time = 1f;
	public bool available = true;
	public bool is_consumed = false;

	public SkillEffects GetEffects(bool is_burst = false)
	{
		SkillEffects effects = new SkillEffects();

		effects.deals_damage = deals_damage;
//		Player.instance
		if (is_burst == true)
		{
			effects.damages = PlayerBattle.instance.bonus_affinity_to_be_added_next;
		}
		else
		{
			float tmp_damages = (float)PlayerBattle.instance.GetEffectiveBattleElementAffinity(element) * damage_ratio;
			effects.damages = Mathf.CeilToInt(tmp_damages);
		}
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