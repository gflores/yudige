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
		PopupSmall.instance.text_label.text = "The effectivness of a skill depends on its type affinity";
		PopupSmall.instance.Show(110,-41, 527, 46);
		yield return null;
	}
	IEnumerator _1(){
		PopupSmall.instance.text_label.text = "Skill has to be casted during a certain amount of time\n" +
			"You can see it on the timeline. When a skill reach the very bottom, it will be launched";
		PopupSmall.instance.Show (226,30, 500, 100);
		yield return null;
	}
}
