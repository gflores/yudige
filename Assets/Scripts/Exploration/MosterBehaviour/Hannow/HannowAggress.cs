using UnityEngine;
using System.Collections;

public class HannowAggress : BaseExplorationInteraction {
	
	protected override IEnumerable ActionList()
	{
		OnStartEvent();
		//Start
		
		yield return StartCoroutine(_0());
		PopupDialog.instance.Hide();
		BattleManager.instance.StartBattle(moster_data.moster_battle);
		OnEndEvent();
	}
	
	IEnumerator _0(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(moster_data.moster_name,
		                                       "...", true));
		yield return new WaitForSeconds(0.001f);
	}
}