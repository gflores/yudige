using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static GameManager instance;
	public int player_starting_life = 100;
	public MosterData starting_moster;
	public Camera exploration_camera {get; set;}
	public Camera battle_camera {get; set;}
	void Awake()
	{
		instance = this;
		exploration_camera = GameObject.FindGameObjectWithTag("ExplorationCamera").camera;
		battle_camera = GameObject.FindGameObjectWithTag("BattleCamera").camera;
	}
	void Start () {
		if (SaveManager.current_saved_game.is_new_game == true)
		{
			InitForNewGame();
		}
		else
		{
			SetGameFromSaveData();
		}
		StateManager.instance.current_states.Add(StateManager.State.EXPLORATION);
		StateManager.instance.UpdateFromStates();
		PlayerExploration.instance.UpdateMosterExploration();

	}

	void InitForNewGame()
	{
		Debug.LogWarning("New game started !!");
		Player.instance.current_life = player_starting_life;
		Player.instance.ApplyEvolutionChanges(starting_moster);
		MostersManager.instance.eliminated_mosters_list = new List<MosterData>();
		MostersManager.instance.evolved_mosters_list = new List<MosterData>();
	}
	public void SaveGame()
	{
		SetSaveDataFromGame();
		SaveManager.instance.LaunchSave();
	}
	public void LoadGame()
	{
		SaveManager.instance.LaunchLoad();
		SetGameFromSaveData();
	}
	public void SetGameFromSaveData()
	{
		Player.instance.current_life = SaveManager.current_saved_game.current_life;
		Player.instance.base_shield = SaveManager.current_saved_game.base_shield;
		Player.instance.base_element_affinities = SaveManager.current_saved_game.base_element_affinities;
		Player.instance.current_karma = SaveManager.current_saved_game.current_karma;
		Player.instance.current_moster = MostersManager.instance.IndexToMosterData(SaveManager.current_saved_game.current_moster_index);
		Player.instance.time_lived = SaveManager.current_saved_game.time_lived;

		MostersManager.instance.eliminated_mosters_list = new List<MosterData>();
		foreach(var moster_index in SaveManager.current_saved_game.eliminated_mosters_list)
			MostersManager.instance.eliminated_mosters_list.Add(MostersManager.instance.IndexToMosterData(moster_index));

		MostersManager.instance.evolved_mosters_list = new List<MosterData>();
		foreach(var moster_index in SaveManager.current_saved_game.evolved_mosters_list)
			MostersManager.instance.evolved_mosters_list.Add(MostersManager.instance.IndexToMosterData(moster_index));
		for (int i = 0; i != SaveManager.current_saved_game.pickup_karmic_point_state_list.Count; ++i)
		{
			if (SaveManager.current_saved_game.pickup_karmic_point_state_list[i] == false)
			{
				PickupKarmicPointManager.instance.pickup_karma_list[i].Destroy();
			}
		}
	}
	public void SetSaveDataFromGame()
	{
		SaveManager.current_saved_game.is_new_game = false;
		SaveManager.current_saved_game.current_life = Player.instance.current_life;
		SaveManager.current_saved_game.base_shield = Player.instance.base_shield;
		SaveManager.current_saved_game.base_element_affinities = Player.instance.base_element_affinities;
		SaveManager.current_saved_game.current_karma = Player.instance.current_karma;
		SaveManager.current_saved_game.current_moster_index = MostersManager.instance.MosterDataToIndex(Player.instance.current_moster);
		SaveManager.current_saved_game.time_lived = Player.instance.time_lived;

		SaveManager.current_saved_game.eliminated_mosters_list = new List<int>();
		foreach(var moster_data in MostersManager.instance.eliminated_mosters_list)
			SaveManager.current_saved_game.eliminated_mosters_list.Add(MostersManager.instance.MosterDataToIndex(moster_data));

		SaveManager.current_saved_game.evolved_mosters_list = new List<int>();
		foreach(var moster_data in MostersManager.instance.evolved_mosters_list)
			SaveManager.current_saved_game.evolved_mosters_list.Add(MostersManager.instance.MosterDataToIndex(moster_data));

	}
}
