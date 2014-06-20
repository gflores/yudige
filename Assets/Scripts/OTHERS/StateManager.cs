using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateManager : MonoBehaviour {
	public static StateManager instance;
	public enum State {
		EXPLORATION,
		BATTLE,
		REWARD,
		EXPLORATION_MENU,
		USE_KARMA_MENU,
		SELECT_TRANSFORM_MENU
	};
	public List<State> current_states {get; set;}

	void Awake()
	{
		current_states = new List<State>();
		instance = this;
	}
	public void SetControls(bool active)
	{
		if (current_states.Contains(State.EXPLORATION) == true)
		{
			if (active == true)
				PlayerExploration.instance.EnableControls();
			else
				PlayerExploration.instance.DisableControls();
		}
	}
	public void UpdateFromStates()
	{
		if (current_states.Contains(State.EXPLORATION_MENU) == true)
		{
			Player.instance.is_living_time_passing = false;
		}
		else
		{
			Player.instance.is_living_time_passing = true;
		}
		if (current_states.Contains(State.EXPLORATION) == true)
		{
			GameManager.instance.exploration_camera.enabled = true;
		}
		else
		{
			GameManager.instance.exploration_camera.enabled = false;
		}

	
		if (current_states.Contains(State.BATTLE) == true)
		{
			GameManager.instance.battle_camera.enabled = true;
		}
		else
		{
			GameManager.instance.battle_camera.enabled = false;
		}
	}
}
