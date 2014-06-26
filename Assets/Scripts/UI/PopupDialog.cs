using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupDialog : MonoBehaviour
{
	public UILabel name_label;
	public UILabel text_label;

	static public PopupDialog instance;


	PopupDialog()
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