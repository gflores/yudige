using UnityEngine;
using System.Collections;

public class ExplainWaitForSkill : SequentialEventValidate {
	protected override IEnumerable ActionList()
	{
		OnStartEvent();
		//Start
		
		yield return StartCoroutine(_0());		
		yield return StartCoroutine(_1());		

		//End
		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0f));
		PopupSmall.instance.Hide();
		OnEndEvent();
	}

	IEnumerator _0(){
		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0.3f));
		PopupSmall.instance.text_label.text = "The effectivness of a skill depends on it's type affinity.";
		PopupSmall.instance.Show(20,-41, 527, 46);
		yield return null;
	}
	IEnumerator _1(){
		PopupSmall.instance.text_label.text = "A skill takes a certain amount of time to be cast.\n" +
			"You can see the cast time on the timeline. When a skill reaches the very bottom, it will take effect.";
		PopupSmall.instance.Show (30,30, 500, 100);
		yield return null;
	}
}
