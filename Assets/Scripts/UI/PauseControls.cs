
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseControls : MonoBehaviour
{
	public UIPanel menu_panel;
	public UIPanel karma_panel;
	
	
	
	void Update()
	{
		
		if ( StateManager.instance.current_states.Contains(StateManager.State.EXPLORATION) && Input.GetButtonDown("Cancel"))
		{
			if (StateManager.instance.current_states.Contains(StateManager.State.EXPLORATION_MENU))
			{
				if (karma_panel.gameObject.activeSelf)
				{
					karma_panel.gameObject.SetActive(false);
					menu_panel.gameObject.SetActive(true);
				}
				else
				{
					Debug.Log("Exiting menu");
					StateManager.instance.current_states.Remove(StateManager.State.EXPLORATION_MENU);
					StateManager.instance.UpdateFromStates();
					menu_panel.gameObject.SetActive(false);
				}
			}
			else
			{
				Debug.Log("Entering Menu");
				StateManager.instance.current_states.Add(StateManager.State.EXPLORATION_MENU);
				StateManager.instance.UpdateFromStates();
				menu_panel.gameObject.SetActive(true);

			}
		}
	}
}