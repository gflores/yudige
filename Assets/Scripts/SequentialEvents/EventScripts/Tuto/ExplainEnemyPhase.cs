using UnityEngine;
using System.Collections;

public class ExplainEnemyPhase : SequentialEventValidate {
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
		PopupSmall.instance.text_label.text = "Like for you, the affinities of your enemy goes up for each of its attacks, and get reset after a final burst attack which deals more damage.\n" +
			"This final burst attack will also change the defensive type of the enemy.\n";
		PopupSmall.instance.Show(110,-41, 527, 300);
		yield return null;
	}
}
