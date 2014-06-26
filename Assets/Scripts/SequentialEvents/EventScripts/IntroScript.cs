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

		//End
		yield return StartCoroutine(_EndSequence());
		OnEndEvent();
	}
	IEnumerator _EndSequence(){
		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0));
		Debug.LogWarning("Finishing!");
		yield return new WaitForSeconds(0.001f);
	}

	IEnumerator _0(){
		PopupDialog.instance.name_label.text = "Ta mere";
		PopupDialog.instance.text_label.text = "salut !";
		PopupDialog.instance.Show ();
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _1(){
		PopupDialog.instance.name_label.text = "Ta mere";
		PopupDialog.instance.text_label.text = "Tu est gay.";
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _2(){
		PopupDialog.instance.Hide();
//		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToTransparent(3f));
//		Debug.LogWarning("Press W for blabla");
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _3(){
//		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0.2f));
//		Debug.LogWarning("But bla bla bla");
		yield return new WaitForSeconds(0.001f);
	}


}
