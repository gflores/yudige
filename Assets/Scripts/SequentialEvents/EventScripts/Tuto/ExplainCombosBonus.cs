using UnityEngine;
using System.Collections;

public class ExplainCombosBonus : SequentialEventValidate {
	protected override IEnumerable ActionList()
	{
		OnStartEvent();
		//Start
		
		yield return StartCoroutine(_0());		

		PopupSmall.instance.Hide();
		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0));
		//End
		OnEndEvent();
	}
	IEnumerator _0(){
		Element element = PlayerBattle.instance.current_element;
		string element_str = ElementManager.instance.ElementToString(element);
		int current_affinity = PlayerBattle.instance.GetEffectiveBattleElementAffinity(element);
		int prev_affinity = current_affinity - PlayerBattle.instance.current_affinity_combos_bonus[(int)element];

		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0.3f));
		PopupSmall.instance.text_label.text = "Consuming your previous skill generates a bonus affinity to be added to the next skill used.\n"+
			"Notice how your " + element_str + " affinity went from " + prev_affinity + "to " + current_affinity +".";
		PopupSmall.instance.Show(20,80, 527, 150);
		yield return null;
	}
}
