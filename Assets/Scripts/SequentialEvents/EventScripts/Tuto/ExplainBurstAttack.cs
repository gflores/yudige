using UnityEngine;
using System.Collections;

public class ExplainBurstAttack : SequentialEventValidate {
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
		PopupSmall.instance.text_label.text = "Your last skill to be used will be twice as powerful.\n" +
			"Using your last skill will also make you recover all of your consumed skills";
		PopupSmall.instance.Show(110,-41, 527, 300);
		yield return null;
	}
}
