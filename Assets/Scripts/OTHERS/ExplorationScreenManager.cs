using UnityEngine;
using System.Collections;

public class ExplorationScreenManager : MonoBehaviour {
	static public ExplorationScreenManager instance;
	public ExplorationScreen[] exploration_screens_list {get; set;}
	void Awake(){
		instance = this;
		exploration_screens_list = GetComponentsInChildren<ExplorationScreen>();
	}
	
	public ExplorationScreen IndexToExplorationScreen(int index)
	{
		return exploration_screens_list[index];
	}
	
	public int ExplorationScreenToIndex(ExplorationScreen s)
	{
		return exploration_screens_list.IndexOf(s);
	}
}
