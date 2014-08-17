using UnityEngine;
using System.Collections;

public sealed class ScreenTeleportationPointCustom2 : ScreenTeleportationPoint {
	bool first_time = true;
	//fuck this/// <summary>
	/// C/	/// </summary>
	/// <returns>The custom transition.</returns>
	sealed protected override IEnumerator Coroutine_CustomTransition()
	{
		if (first_time == true)
		{
			first_time = false;
			yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToOpaque(1f));
			yield return new WaitForSeconds(0.15f);
			PlayerExploration.instance.transform.position = linked_teleportation_point.spawn_point.position;
			linked_teleportation_point.exploration_screen.MakeGoTo();
			PopupText.instance.Show(0, 0, 500, 200);
			PopupText.instance.text_label.text = "";
			yield return StartCoroutine(StringUtils.LaunchProgressiveLabel(
				"Now that he experienced what existence felt like, ",
				PopupText.instance.text_label
				));
			yield return StartCoroutine(WaitForValidation());
			yield return StartCoroutine(StringUtils.LaunchProgressiveLabel(
				"The Void could now start to comprehend how he could bring this reality to an end.\n",
				PopupText.instance.text_label
				));
			yield return StartCoroutine(WaitForValidation());
			PopupText.instance.Hide();
			PopupText.instance.text_label.text = "";
			yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToTransparent(1f));
		}
		else
		{
			yield return StartCoroutine(Coroutine_BaseTransition());
//			yield return StartCoroutine(base.Coroutine_CustomTransition());
		}
	}
	IEnumerator WaitForValidation()
	{
		while(true)
		{
			if (Input.GetButtonDown("Validate"))
				yield break;
			yield return new WaitForSeconds(0.01f);
		}
	}

}
