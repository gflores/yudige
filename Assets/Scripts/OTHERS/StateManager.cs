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
		SELECT_TRANSFORM_MENU,
		BATTLE_INTRO,
		SCRIPTED_EVENT
	};
	public HashSet<State> current_states {get; set;}

	void Awake()
	{
		current_states = new HashSet<State>();
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
		if (current_states.Contains(State.EXPLORATION) == true)
		{
			GameManager.instance.exploration_camera.enabled = true;
			if (current_states.Contains(State.EXPLORATION_MENU) == true)
				ExplorationTimeFlowing(false);
			else
				ExplorationTimeFlowing(true);
		}
		else
		{
			GameManager.instance.exploration_camera.enabled = false;
			ExplorationTimeFlowing(false);
		}
	
		if (current_states.Contains(State.BATTLE) == true)
		{
			GameManager.instance.battle_gui_camera.enabled = true;
			GameManager.instance.battle_element_camera.enabled = true;
//			BattleTimeFlowing(true);
		}
		else
		{
			GameManager.instance.battle_gui_camera.enabled = false;
			GameManager.instance.battle_element_camera.enabled = false;
//			BattleTimeFlowing(false);
		}
		if (current_states.Contains(State.USE_KARMA_MENU) == true || current_states.Contains(State.EXPLORATION_MENU) == true )
		{
			GameManager.instance.karma_camera.enabled = true;
		}
		else
		{
			GameManager.instance.karma_camera.enabled = false;
		}
		if (current_states.Contains(State.SCRIPTED_EVENT) == true)
		{
			ExplorationTimeFlowing(false);
			BattleTimeFlowing(false);
		}
	}

	void ExplorationTimeFlowing(bool state)
	{
		if (state == true)
		{
			MostersManager.instance.can_check_evolution = true;
			Player.instance.is_living_time_passing = true;
			PlayerExploration.instance.EnableControls();
		}
		else
		{
			MostersManager.instance.can_check_evolution = false;
			Player.instance.is_living_time_passing = false;
			PlayerExploration.instance.DisableControls();
		}
	}
	void BattleTimeFlowing(bool state)
	{
		if (state == true)
		{
			Player.instance.is_living_time_passing = true;
		}
		else
		{
			Player.instance.is_living_time_passing = false;
		}
	}
}
