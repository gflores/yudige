using UnityEngine;
using System.Collections;

public class ExplainShield : SequentialEventValidate {
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
		Element element = PlayerBattle.instance.current_element;
		string element_str = ElementManager.instance.ElementToString(element);
		int current_affinity = PlayerBattle.instance.GetEffectiveBattleElementAffinity(element);
		
		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0.3f));
		PopupSmall.instance.text_label.text = "Your health is protected by a shield which regenerate over time.\n" +
			"But it won't regenerate anymore after it has break\n";
		PopupSmall.instance.Show(110,-41, 527, 300);
		yield return null;
	}
}
