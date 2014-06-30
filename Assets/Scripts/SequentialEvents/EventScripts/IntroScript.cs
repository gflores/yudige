using UnityEngine;
using System.Collections;

public class IntroScript : SequentialEventValidate {
	public FadeText fade_text;
	public Dialog dialog;

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
		PopupDialog.instance.Hide();
		ForceDoNextAction();

		//End
		OnEndEvent();
	}

	IEnumerator _0(){
		PlayerExploration.instance.main_renderer.enabled = false;

		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 1));
		PopupText.instance.Show(0, 200, 500, 300);
		PopupText.instance.text_label.text = "";
		yield return StartCoroutine(StringUtils.LaunchProgressiveLabel(
			"There existed a being who wanted to destroy everything\n",
			PopupText.instance.text_label
		));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _1(){
		yield return StartCoroutine(StringUtils.LaunchProgressiveLabel(
			"This being first decided on a name to allow his own existence to be possible: the Void\n",
			PopupText.instance.text_label
			));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _2(){
		PopupText.instance.text_label.text = "";
		yield return StartCoroutine(StringUtils.LaunchProgressiveLabel(
			"The reason he wanted to exist was that only existing things could affect other existent beings.\n",
			PopupText.instance.text_label
			));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _3(){
		yield return StartCoroutine(StringUtils.LaunchProgressiveLabel(
			"However this world had a very particular constraint.\n",
			PopupText.instance.text_label
			));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _4(){
		PopupText.instance.text_label.text = "";
		yield return StartCoroutine(StringUtils.LaunchProgressiveLabel(
			"The "+MostersManager.instance.mosters_list.Length+" already existing things has already been established.\n",
			PopupText.instance.text_label
			));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _5(){
		yield return StartCoroutine(StringUtils.LaunchProgressiveLabel(
			"So the Void had no other choices than becoming an already existing \"something\"",
			PopupText.instance.text_label
			));
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _6(){
		PopupText.instance.text_label.text = "";
		PopupText.instance.Hide();
		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToTransparent(2f));
		PopupSmall.instance.text_label.text = "Press Z to come into existance";
		PopupSmall.instance.Show(0, 0, 200, 100);
	}
	IEnumerator _7(){
		PopupSmall.instance.Hide();
		CameraManager.instance.SetColorToFadePlane(Color.white);
		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToOpaque(0.5f));
		PlayerExploration.instance.main_renderer.enabled = true;
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToTransparent(3f));
		GameManager.instance.current_screen.ApplyBackgroundMusic();
		yield return StartCoroutine(
			PopupDialog.instance.Coroutine_MakeSay(PopupDialog.instance.protag_name,
		                                       "How conveniently easy it is to go from non-existence to existence.", true));
	}
	IEnumerator _8(){
		yield return StartCoroutine(PopupDialog.instance.Coroutine_MakeSay(PopupDialog.instance.protag_name,
		                                                                   "But strangely, I have the feeling that doing the other way around won’t be as simple.", true));
	}
}
