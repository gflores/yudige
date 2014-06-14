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
}
