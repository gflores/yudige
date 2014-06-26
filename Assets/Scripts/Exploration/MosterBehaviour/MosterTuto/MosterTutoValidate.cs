using UnityEngine;
using System.Collections;

public class MosterTutoValidate : BaseExplorationInteraction {
	protected override IEnumerable ActionList()
	{
		OnStartEvent();
		//Start
		
		yield return StartCoroutine(_0());

		//End
		yield return StartCoroutine(_EndSequence());
		PopupDialog.instance.Hide();
		OnEndEvent();
	}
	IEnumerator _0(){
		PopupDialog.instance.name_label.text = moster_data.moster_name;
		PopupDialog.instance.text_label.text = "coucou.";
		PopupDialog.instance.Show ();
		yield return new WaitForSeconds(0.001f);
	}

	IEnumerator _EndSequence(){
		PopupDialog.instance.name_label.text = moster_data.moster_name;
		PopupDialog.instance.text_label.text = "bye bye";
		yield return new WaitForSeconds(0.001f);
	}
	



}
