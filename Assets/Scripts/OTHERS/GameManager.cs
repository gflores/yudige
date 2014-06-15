using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance;
	public int player_starting_life = 100;
	public MosterData starting_moster;
	
	void Awake()
	{
		instance = this;
	}
	void Start () {
		if (SaveManager.current_saved_game.is_new_game == true)
		{
			InitForNewGame();
		}
		StateManager.instance.current_states.Add(StateManager.State.EXPLORATION);
	}

	void InitForNewGame()
	{
		Player.instance.current_life = player_starting_life;
		Player.instance.ApplyEvolutionChanges(starting_moster);
	}
}
