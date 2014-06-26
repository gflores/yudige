
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
		slot1.gameObject.SetActive (slot1.sk != null);
		slot2.gameObject.SetActive (slot2.sk != null);
		slot3.gameObject.SetActive (slot3.sk != null);
		affinity_label.text = PlayerBattle.instance.GetEffectiveBattleElementAffinity(element).ToString();

	}

	void AffinityChange()
	{
		PlayerBattle.instance.ClickOnElementDefense (element);
		BattleScreen.instance.DamageToBoss ("ma tete");
		PopupText.instance.Show (0,0, 200, 50);
	}
}