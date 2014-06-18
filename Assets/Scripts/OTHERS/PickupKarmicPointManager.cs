using UnityEngine;
using System.Collections;

public class PickupKarmicPointManager : MonoBehaviour {
	static public PickupKarmicPointManager instance;

	public PickupKarmaExplorationTrigger[] pickup_karma_list {get; set;}
	void Awake()
	{
		instance = this;
		pickup_karma_list = GetComponentsInChildren<PickupKarmaExplorationTrigger>();
	}
}
