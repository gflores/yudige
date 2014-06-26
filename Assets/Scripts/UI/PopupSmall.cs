using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupSmall : MonoBehaviour
{
	public UILabel text_label;

	static public PopupSmall instance;


	PopupSmall()
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