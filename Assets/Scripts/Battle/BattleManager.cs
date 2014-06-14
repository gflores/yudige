using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour {
	public static BattleManager instance {get; set;}

	public NonPlayerMoster enemy_moster;

	void Awake () {
		instance = this;
	}

	public void StartBattle(NonPlayerMoster n_enemy_moster)
	{
		enemy_moster = n_enemy_moster;
		LaunchStartBattle();
	}
	public void LaunchStartBattle()
	{
		if (IsBattleAlreadyLaunched() == true)
			return ;
		StateManager.instance.current_states.Add(StateManager.State.BATTLE);
	}
	public bool IsBattleAlreadyLaunched()
	{
		return StateManager.instance.current_states.Contains(StateManager.State.BATTLE);
	}
}
