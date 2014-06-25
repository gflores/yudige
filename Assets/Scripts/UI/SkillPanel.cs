
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillPanel : MonoBehaviour
{
	public Element element;

	public SkillButton slot1;
	public SkillButton slot2;
	public SkillButton slot3;
	public UIButton affinity;
	public UILabel affinity_label;

	void Update()
	{
		List<Skill> skills = PlayerBattle.instance.GenerateSkillsAvailable (element);
		slot1.sk = (skills.Count > 0) ? skills [0] : null;
		slot2.sk = (skills.Count > 1) ? skills [1] : null;
		slot3.sk = (skills.Count > 2) ? skills [2] : null;
		affinity_label.text = Player.instance.base_element_affinities [(int)element].ToString();
	}

	void AffinityChange()
	{
		PlayerBattle.instance.ClickOnElementDefense (element);
		BattleScreen.instance.DamageToBoss ("ma tete");
	}
}