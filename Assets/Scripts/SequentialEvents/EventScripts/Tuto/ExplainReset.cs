﻿using UnityEngine;
using System.Collections;

public class ExplainReset : SequentialEventValidate {
	protected override IEnumerable ActionList()
	{

		OnStartEvent();
		//Start
		
		yield return StartCoroutine(_0());

		//End
		OnEndEvent();
		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0f));
		PopupSmall.instance.Hide();
	}
	IEnumerator _0(){
		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0.3f));
		PopupSmall.instance.text_label.text = "When you manually reset, your type instantly go to being neutral, meaning you will take damages regardless of your affinities, or the attack type\n";
		PopupSmall.instance.Show(110,-41, 527, 300);
		yield return null;
	}
}
