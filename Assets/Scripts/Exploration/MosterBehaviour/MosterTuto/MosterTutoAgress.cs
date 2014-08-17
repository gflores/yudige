using UnityEngine;
using System.Collections;

public class MosterTutoAgress : BaseExplorationInteraction {
	public TutoScriptedBattle tuto_scripted_battle;
	protected override IEnumerable ActionList()
	{
		OnStartEvent();
		//Start
		
		//yield return StartCoroutine(_0());
		//yield return StartCoroutine(_1());
		SoundManager.instance.current_music_played.Stop();
		SoundManager.instance.remove_element_sound.Play();
		yield return StartCoroutine(_2());
		yield return StartCoroutine(_3());

		//End
		PopupDialog.instance.Hide();
		tuto_scripted_battle.StartSequence();
//		BattleManager.instance.StartBattle(moster_data.moster_battle);
		OnEndEvent();
	}
	IEnumerator _0(){
		PopupSmall.instance.Hide();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(moster_data.moster_name,
		                                       "Still, ", true));
		PopupDialog.instance.Show ();
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _1(){
		PopupSmall.instance.Hide();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(moster_data.moster_name,
		                                       "being able to observe myself is a unique experience."));
		PopupDialog.instance.Show ();
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _2(){
		PopupSmall.instance.Hide();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(PopupDialog.instance.protag_name,
		                                       "To be or not to be.", true));
		PopupDialog.instance.Show ();
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _3(){
		PopupSmall.instance.Hide();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(PopupDialog.instance.protag_name,
		                                       " That's a choice you never had."));
		PopupDialog.instance.Show ();
		yield return new WaitForSeconds(0.001f);
	}

}
