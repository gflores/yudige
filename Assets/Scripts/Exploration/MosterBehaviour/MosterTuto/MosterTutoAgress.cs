using UnityEngine;
using System.Collections;

public class MosterTutoAgress : BaseExplorationInteraction {
	public TutoScriptedBattle tuto_scripted_battle;
	protected override IEnumerable ActionList()
	{
		OnStartEvent();
		//Start
		
		yield return StartCoroutine(_0());
		
		//End
		PopupDialog.instance.Hide();
		tuto_scripted_battle.StartSequence();
//		BattleManager.instance.StartBattle(moster_data.moster_battle);
		OnEndEvent();
	}
	IEnumerator _0(){
		PopupDialog.instance.name_label.text = moster_data.moster_name;
		PopupDialog.instance.text_label.text = "Nique ta race !";
		PopupDialog.instance.Show ();
		yield return new WaitForSeconds(0.001f);
	}

}
