﻿using UnityEngine;
using System.Collections;

public class ExplainBurstReset : SequentialEventValidate {
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
		PopupSmall.instance.text_label.text = "When all your skills are recovered, you lose all the bonus affinities generated by the previous skills.";
		PopupSmall.instance.Show(20,80, 527, 150);
		yield return null;
	}
	IEnumerator _1(){
		Element element = PlayerBattle.instance.current_element;
		string element_str = ElementManager.instance.ElementToString(element);
		int current_affinity = PlayerBattle.instance.GetEffectiveBattleElementAffinity(element);
		
		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0.3f));
		PopupSmall.instance.text_label.text = "However you permanently gain an affinity point in the element of your burst attack\n" +
			"Notice how your " + element_str + " affinity went from " + (current_affinity - 1) + " to " + current_affinity +".";
		PopupSmall.instance.Show(20,80, 527, 150);
		yield return null;
	}
}
