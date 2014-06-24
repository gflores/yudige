using UnityEngine;
using System.Collections;

public class TeleportationExplorationTrigger : MonoBehaviour {
	public ScreenTeleportationPoint teleportation_point;

	void  OnTriggerEnter2D(Collider2D other)
	{
		if (teleportation_point.linked_teleportation_point == null && other.tag == "exploration_screen_trigger")
		{
			teleportation_point.linked_teleportation_point = other.GetComponent<TeleportationExplorationTrigger>().teleportation_point;
		}

		if (TagManager.ContainsTag(other, "TAG_player_head_trigger_hitbox") == true)
		{
			teleportation_point.TeleportToLinkedPoint();
		}
	}
	
}
