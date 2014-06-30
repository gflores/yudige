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
		int label_width = width - 20;
		int label_height = height - 20;
		
		gameObject.SetActive(true);
		gameObject.transform.localPosition = new Vector3(x, y);
		text_sprite.transform.localScale = new Vector3(width, height);
		text_label.transform.localPosition = new Vector3(-( label_width / 2), 0);
		text_label.lineWidth = label_width;
		text_label.lineHeight = label_height;

	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}