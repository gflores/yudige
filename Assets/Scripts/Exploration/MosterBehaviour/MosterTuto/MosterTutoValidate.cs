using UnityEngine;
using System.Collections;

public class MosterTutoValidate : BaseExplorationInteraction {
	public GenericExplorationMosterEvents generic_events;
	protected override IEnumerable ActionList()
	{
		OnStartEvent();
		//Start
		
		yield return StartCoroutine(_0());
		yield return StartCoroutine(_1());
		yield return StartCoroutine(_2());
		yield return StartCoroutine(_3());
		yield return StartCoroutine(_4());
		yield return StartCoroutine(_5());
		yield return StartCoroutine(_6());
		yield return StartCoroutine(_7());
		yield return StartCoroutine(_8());
		yield return StartCoroutine(_9());
		yield return StartCoroutine(_10());
		yield return StartCoroutine(_11());
		yield return StartCoroutine(_12());
		yield return StartCoroutine(_13());
		yield return StartCoroutine(_14());
		yield return StartCoroutine(_15());
		yield return StartCoroutine(_16());
		yield return StartCoroutine(_17());
		//End
		PopupDialog.instance.Hide();
		PopupSmall.instance.Show(0, 0, 200, 100);
		PopupSmall.instance.text_label.text = "Press C to begin the procedure for suppression";
		generic_events.can_aggress = true;
		OnEndEvent();
	}
	IEnumerator _0(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(moster_data.moster_name,
		                                       "Oh my...", true));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _1(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(moster_data.moster_name,
		                                       "And I thought it was impossible for another being to be me.", true));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _2(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(PopupDialog.instance.protag_name,
		                                       "I simply am.", true));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _3(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(PopupDialog.instance.protag_name,
		                                       "\nThere is no point asking why or how I happen to be."));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _4(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(moster_data.moster_name,
		                                       "I hope you understand how surprised I may be by witnessing something impossible...", true));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _5(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(PopupDialog.instance.protag_name,
		                                       "I am also very confused experiencing this reality, so I can relate.", true));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _6(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(PopupDialog.instance.protag_name,
		                                       "With that said, ", true));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _7(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(PopupDialog.instance.protag_name,
		                                       "you shouldn’t try to wrap your head around my existence as it won’t make any sense to you,"));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _8(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(PopupDialog.instance.protag_name,
		                                       " an existent being."));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _9(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(moster_data.moster_name,
		                                       "I actually have a name,", true));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _10(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(moster_data.moster_name,
		                                       " I’m Charmondre."));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _11(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(PopupDialog.instance.protag_name,
		                                       "I know how important it is to put labels on entities to tell them apart.", true));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _12(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(PopupDialog.instance.protag_name,
		                                       " It is needed if you want to keep your own integrity."));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _13(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(moster_data.moster_name,
		                                       "You were right, ", true));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _14(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(moster_data.moster_name,
		                                       "you make no sense."));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _15(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(PopupDialog.instance.protag_name,
		                                       "Don't worry, ", true));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _16(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(PopupDialog.instance.protag_name,
		                                       "experiencing non-existence will help you understand."));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _17(){
		PopupDialog.instance.Show ();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(PopupDialog.instance.protag_name,
		                                       "Just wait until I figure out how the procedure for suppression works in this reality.", true));
		yield return new WaitForSeconds(0.001f);
	}
}
