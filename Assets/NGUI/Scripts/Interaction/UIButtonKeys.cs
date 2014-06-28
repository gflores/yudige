//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Attaching this script to a widget makes it react to key events such as tab, up, down, etc.
/// </summary>

[RequireComponent(typeof(Collider))]
[AddComponentMenu("NGUI/Interaction/Button Keys")]
public class UIButtonKeys : MonoBehaviour
{
	public bool startsSelected = false;
	public UIButtonKeys selectOnClick;
	public UIButtonKeys selectOnUp;
	public UIButtonKeys selectOnDown;
	public UIButtonKeys selectOnLeft;
	public UIButtonKeys selectOnRight;

	void OnEnable ()
	{
		if (startsSelected && UICamera.selectedObject == null)
		{
			if (!NGUITools.GetActive(UICamera.selectedObject))
			{
				UICamera.selectedObject = gameObject;
			}
			else
			{
				UICamera.Notify(gameObject, "OnHover", true);
			}
		}
	}
	 
	void OnKey (KeyCode key)
	{
		if (enabled && NGUITools.GetActive(gameObject))
		{
			UIButtonKeys ubk = GetNextGameObject(key);
			while (ubk != null && NGUITools.GetActive(ubk.gameObject) != true)
			{
				ubk = ubk.GetNextGameObject(key);
			}
			if (ubk != null)
				UICamera.selectedObject = ubk.gameObject;
		}
	}

	public UIButtonKeys GetNextGameObject(KeyCode key)
	{
		switch (key)
		{
		case KeyCode.LeftArrow:
			if (selectOnLeft != null) return selectOnLeft;
			break;
		case KeyCode.RightArrow:
			if (selectOnRight != null) return selectOnRight;
			break;
		case KeyCode.UpArrow:
			if (selectOnUp != null) return selectOnUp;
			break;
		case KeyCode.DownArrow:
			if (selectOnDown != null) return selectOnDown;
			break;
		case KeyCode.Tab:
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				if (selectOnLeft != null) return selectOnLeft;
				else if (selectOnUp != null) return selectOnUp;
				else if (selectOnDown != null) return selectOnDown;
				else if (selectOnRight != null) return selectOnRight;
			}
			else
			{
				if (selectOnRight != null) return selectOnRight;
				else if (selectOnDown != null) return  selectOnDown;
				else if (selectOnUp != null) return selectOnUp;
				else if (selectOnLeft != null) return selectOnLeft;
			}
			break;
		}
		return null;
	}

	void OnClick ()
	{
		if (enabled && selectOnClick != null)
		{
			UICamera.selectedObject = selectOnClick.gameObject;
		}
	}
}
