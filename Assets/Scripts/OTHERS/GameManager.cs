using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public bool standalone_scene_mode = true;
	public static GameManager instance;
	public float auto_save_frequency_time = 2f;
	public int player_starting_life = 100;
	public MosterData starting_moster;
	public MosterData baby_moster;
	public ExplorationScreen rebirth_screen;
	public Transform rebirth_spawn_point;
	public ExplorationScreen tuto_screen;
	public Transform tuto_spawn_point;
	
	public Camera main_camera {get; set;}
	public Camera exploration_camera {get; set;}
	public Camera battle_gui_camera {get; set;}
	public Camera battle_element_camera {get; set;}
	public ExplorationScreen current_screen {get; set;}
	public Camera karma_camera {get; set;}
	public Camera popup_camera {get; set;}
	void Awake()
	{
		if (standalone_scene_mode == true)
		{
			Debug.LogWarning("STANDALONE SCENE MODE ACTIVE, VERSION DE DEBUG");
		}
		instance = this;
		exploration_camera = GameObject.FindGameObjectWithTag("ExplorationCamera").camera;
		battle_gui_camera = GameObject.FindGameObjectWithTag("BattleGUICamera").camera;
		battle_element_camera = GameObject.FindGameObjectWithTag("BattleElementCamera").camera;
		main_camera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		karma_camera = GameObject.FindGameObjectWithTag("KarmaCamera").camera;
	}
	bool IsWantingNewGame()
	{
		if (standalone_scene_mode == true)
			return SaveManager.current_saved_game.is_new_game;
		else
			return MasterGameManager.want_new_game;
	}
	void Start () {
		Debug.LogWarning("new: " + IsWantingNewGame());
		if (IsWantingNewGame())
		{
			InitForNewGame();
		}
		else
		{
			SetGameFromSaveData();
		}
		StateManager.instance.current_states.Add(StateManager.State.EXPLORATION);
		StateManager.instance.UpdateFromStates();
		Player.instance.RefreshMoster();
		PlayerExploration.instance.UpdateMosterExploration();
		StartCoroutine(AutoSaveContinuously());
	}
	IEnumerator AutoSaveContinuously()
	{
		while (true)
		{
			yield return new WaitForSeconds(auto_save_frequency_time);
			SaveGame();
		}
	}
	bool IsInTutorial(){
		if (standalone_scene_mode)
			return SaveManager.current_saved_game.is_in_tutorial == true;
		else
			return MasterGameManager.is_in_tutorial;
	}
	public Transform GetSpawnPoint()
	{
		if (IsInTutorial())
			return tuto_spawn_point;
		else
			return rebirth_spawn_point;
	}
	public ExplorationScreen GetSpawnScreen()
	{
		if (IsInTutorial())
			return tuto_screen;
		else
			return rebirth_screen;
	}
	public void PrepareGame()
	{
		
	}
	void InitForNewGame()
	{
		Debug.LogWarning("New game started !!");
		Player.instance.current_life = player_starting_life;
		Player.instance.ApplyEvolutionChanges(starting_moster);
		MostersManager.instance.eliminated_mosters_list = new List<MosterData>();
		MostersManager.instance.evolved_mosters_list = new List<MosterData>();
		PlayerExploration.instance.transform.position = GetSpawnPoint().position;
		GetSpawnScreen().MakeGoTo();

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
		PlayerExploration.instance.transform.position = SaveManager.current_saved_game.player_position;
		ExplorationScreenManager.instance.IndexToExplorationScreen(SaveManager.current_saved_game.current_exploration_screen_index).MakeGoTo();
		Player.instance.current_life = SaveManager.current_saved_game.current_life;
		Player.instance.base_shield = SaveManager.current_saved_game.base_shield;
		Player.instance.base_element_affinities = SaveManager.current_saved_game.base_element_affinities;
		Player.instance.current_karma = SaveManager.current_saved_game.current_karma;
		Player.instance.current_moster = MostersManager.instance.IndexToMosterData(SaveManager.current_saved_game.current_moster_index);
		Player.instance.time_lived = SaveManager.current_saved_game.time_lived;

		MostersManager.instance.eliminated_mosters_list = new List<MosterData>();
		foreach(var moster_index in SaveManager.current_saved_game.eliminated_mosters_list)
		{
			MosterData moster_data = MostersManager.instance.IndexToMosterData(moster_index);
			MostersManager.instance.eliminated_mosters_list.Add(moster_data);
			moster_data.moster_exploration.gameObject.SetActive(false);
		}

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
		SaveManager.current_saved_game.player_position = PlayerExploration.instance.transform.position;
		
		SaveManager.current_saved_game.current_exploration_screen_index = ExplorationScreenManager.instance.ExplorationScreenToIndex(GameManager.instance.current_screen);
		
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
