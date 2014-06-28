
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
	public GameObject preview;
	public UILabel preview_label;


	void Awake()
	{
		slot1.parent = this;
		slot2.parent = this;
		slot3.parent = this;
	}

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
		preview_label.text = (PlayerBattle.instance.GetEffectiveBattleElementAffinity(element) + PlayerBattle.instance.bonus_affinity_to_be_added_next).ToString() ;

	}

	void AffinityChange()
	{
		PlayerBattle.instance.ClickOnElementDefense (element);
		BattleScreen.instance.DamageToBoss ("ma tete");
		HighlightManager.instance.HighlightObject( HighlightManager.instance.element1slot1);
		PopupSmall.instance.Show (0,0, 200, 50);
	}

	public void DisplayPreview(bool b)
	{
		preview.SetActive(b);

	}
}