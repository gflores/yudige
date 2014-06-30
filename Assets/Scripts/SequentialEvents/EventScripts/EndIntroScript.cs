using UnityEngine;
using System.Collections;

public class EndIntroScript : SequentialEventValidate {

	protected override IEnumerable ActionList()
	{
		OnStartEvent();
		//Start
		
//		yield return StartCoroutine(_0());
		PopupDialog.instance.Hide();
		ForceDoNextAction();

		MasterGameManager.is_in_tutorial = false;
		//utile pour le mode standalone BEGIN
		SaveManager.current_saved_game.is_new_game = true;
		SaveManager.current_saved_game.is_in_tutorial = false;
		SaveManager.instance.LaunchSave();
		//utile pour le mode standalone END
		Application.LoadLevel(Application.loadedLevel);
		Time.timeScale = 1;
		//End
		OnEndEvent();
		yield return null;
	}
	
	IEnumerator _0(){
		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0));
		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToOpaque(2f));
		PopupText.instance.Show(0, 200, 500, 300);
		PopupText.instance.text_label.text = "";
		yield return StartCoroutine(StringUtils.LaunchProgressiveLabel(
			"end tuto\n",
			PopupText.instance.text_label
			));
		yield return new WaitForSeconds(0.001f);
	}
}
