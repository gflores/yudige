using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SaveManager : MonoBehaviour {
	public static SaveManager instance;
	public static SavedGameData current_saved_game;
	static private string PLAYERS_PREFS_STRING = "SaveData";
	
	public SaveData save_data;
	
	void Awake () {
		instance = this;
		LaunchLoad();
		current_saved_game = save_data.saved_game_data;
	}
	
	public void LaunchSave()
	{
		PlayerPrefs.SetString(PLAYERS_PREFS_STRING, Serialization.UnitySerializer.JSONSerialize(save_data));
		PlayerPrefs.Save();
		Debug.LogWarning("Saved !");
	}
	
	public void LaunchLoad()
	{
		save_data = Serialization.UnitySerializer.JSONDeserialize<SaveData>(PlayerPrefs.GetString(PLAYERS_PREFS_STRING));
		if (save_data == null)
		{
			Debug.LogWarning("Save never existed");
			save_data = GetNewSaveData();
		}
		Debug.LogWarning("Loaded !");
	}

	public SaveData GetNewSaveData()
	{
		return SaveData.GetNew();
	}

}

[System.Serializable]
public class SaveData{
	public SavedGameData saved_game_data;
	static public SaveData GetNew()
	{
		SaveData save_data = new SaveData();
		
		save_data.saved_game_data = SavedGameData.GetNew();
		
		return save_data;
	}
}

[System.Serializable]
public class SavedGameData
{
	public bool is_new_game;
	public int current_moster_index;
	public int current_life;
	public int base_shield;
	public int[] base_element_affinities;
	
	static public SavedGameData GetNew()
	{
		SavedGameData save_game_data;
		
		save_game_data = new SavedGameData();
		save_game_data.is_new_game = true;
		return save_game_data;
	}
}