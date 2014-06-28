using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupDialog : MonoBehaviour
{
	public UILabel name_label;
	public UILabel text_label;

	static public PopupDialog instance;
	public string protag_name = "The Void";

	PopupDialog()
	{
		instance = this;
	}

	public void MakeSay(string talk_name, string talk_text)
	{
		StartCoroutine(Coroutine_MakeSay(talk_name, talk_text));
	}
	public IEnumerator Coroutine_MakeSay(string talk_name, string talk_text, bool reinitialize_text = false)
	{
		Show();
		name_label.text = talk_name;
		if (reinitialize_text == false)
			text_label.text = "";
		yield return StartCoroutine(StringUtils.LaunchProgressiveLabel(
			talk_text,
			text_label
		));
	}
	public void Show()
	{
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}