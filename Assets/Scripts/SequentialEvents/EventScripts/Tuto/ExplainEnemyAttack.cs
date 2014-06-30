using UnityEngine;
using System.Collections;

public class ExplainEnemyAttack : SequentialEventValidate {
	protected override IEnumerable ActionList()
	{
		OnStartEvent();
		//Start
		
		yield return StartCoroutine(_0());		
		yield return StartCoroutine(_1());		

		//End
		OnEndEvent();
		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0f));
		PopupSmall.instance.Hide();
	}
	IEnumerator _0(){
		Element element = PlayerBattle.instance.current_element;
		string element_str = ElementManager.instance.ElementToString(element);
		int current_affinity = PlayerBattle.instance.GetEffectiveBattleElementAffinity(element);
		
		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0.3f));
		PopupSmall.instance.text_label.text = "The enemy has started to attack.\n" +
			"Like you, his attacks have a type. You can predict the type of the attack looking at its color:\n" +
			"Void is black, Absolute is white, Rock is brown, and Fire is red.";
		PopupSmall.instance.Show(20,80, 527, 200);
		yield return null;
	}
	IEnumerator _1(){
		Element element = PlayerBattle.instance.current_element;
		string element_str = ElementManager.instance.ElementToString(element);
		int current_affinity = PlayerBattle.instance.GetEffectiveBattleElementAffinity(element);
		
		PopupSmall.instance.text_label.text = "Your defensive type instantly change when you start casting a skill.";
		PopupSmall.instance.Show(20,80, 527, 150);
		yield return null;
	}
}
