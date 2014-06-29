using UnityEngine;
using System.Collections;

public class AppearByScreenNb : MonoBehaviour {
	public int screen_nb = 3;
	int _current_screen_nb = 0;
	public void UpdateCounter()
	{
		_current_screen_nb++;
		if (_current_screen_nb % screen_nb == 0)
		{
			gameObject.SetActive(true);
		}
		else
		{
			gameObject.SetActive(false);
		}
	}
}
