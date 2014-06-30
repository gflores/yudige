using UnityEngine;
using System.Collections;

public class HannowValidate : BaseExplorationInteraction {

	protected override IEnumerable ActionList()
	{
		OnStartEvent();
		//Start
		
		yield return StartCoroutine(_0());
		PopupDialog.instance.Hide();
		OnEndEvent();

	}

	IEnumerator _0(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(moster_data.moster_name,
		                                       "Hi", true));
		yield return new WaitForSeconds(0.001f);
	}
}