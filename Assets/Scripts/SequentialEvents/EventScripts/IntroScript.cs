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
		OnEndEvent();
		yield return StartCoroutine(_EndSequence());
	}
	IEnumerator _EndSequence(){
		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0));
		Debug.LogWarning("Finishing!");
		yield return new WaitForSeconds(0.001f);
	}

	IEnumerator _0(){
		CameraManager.instance.SetColorToFadePlane(Color.black);
		Debug.LogWarning("Text1");
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _1(){
		Debug.LogWarning("Text2");
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _2(){
		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToTransparent(3f));
		Debug.LogWarning("Press W for blabla");
		yield return new WaitForSeconds(0.001f);
	}
	IEnumerator _3(){
		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0.2f));
		Debug.LogWarning("But bla bla bla");
		yield return new WaitForSeconds(0.001f);
	}


}
