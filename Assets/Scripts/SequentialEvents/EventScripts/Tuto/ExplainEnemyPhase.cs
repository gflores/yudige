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
		PopupSmall.instance.text_label.text = "Just like you, the affinities of your enemy goes up for each of it's attacks, and get reset after a final burst attack which deals more damage.\n" +
			"This final burst attack will also change the defensive type of the enemy.\n";
		PopupSmall.instance.Show(20,80, 527, 200);
		yield return null;
	}
}
