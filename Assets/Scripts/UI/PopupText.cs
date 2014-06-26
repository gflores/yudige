using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupText : MonoBehaviour
{
	public UILabel text_label;
	public UISprite text_sprite;

	static public PopupText instance;


	PopupText()
	{
		instance = this;
	}



	public void Show(int x, int y, int width, int height)
	{
		gameObject.SetActive(true);
		gameObject.transform.localPosition = new Vector3(x, y);
		text_sprite.transform.localScale = new Vector3(width, height);
		text_label.lineWidth = width - 20;
		text_label.lineHeight = height - 20;

	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}