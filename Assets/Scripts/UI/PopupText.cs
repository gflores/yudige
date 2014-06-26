using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupText : MonoBehaviour
{
	public UILabel text_label;

	static public PopupText instance;


	PopupText()
	{
		instance = this;
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