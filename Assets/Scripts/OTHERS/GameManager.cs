using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance;
	void Awake()
	{
		instance = this;
	}
	void Start () {
		StateManager.instance.current_states.Add(StateManager.State.EXPLORATION);
	}
}
