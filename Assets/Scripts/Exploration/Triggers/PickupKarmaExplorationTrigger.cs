using UnityEngine;
using System.Collections;

public class PickupKarmaExplorationTrigger : BaseExplorationTrigger {

	public int karma_nb = 1;
	public ObjectActivator activator {get; set;}

	void Awake()
	{
		activator = GetComponent<ObjectActivator>();
	}
	protected override void OnPlayerEnter()
	{
		Player.instance.current_karma += karma_nb;
		SaveManager.current_saved_game.pickup_karmic_point_state_list[PickupKarmicPointManager.instance.pickup_karma_list.IndexOf(this)] = false;
		Destroy();
	}

	public void Destroy()
	{
		activator.Change();
	}
}
