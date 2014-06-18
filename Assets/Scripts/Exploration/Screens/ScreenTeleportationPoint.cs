using UnityEngine;
using System.Collections;

public class ScreenTeleportationPoint : MonoBehaviour {
	public ScreenTeleportationPoint linked_teleportation_point;
	public Transform spawn_point;
	public ExplorationScreen exploration_screen {get; set;}

	public void TeleportToLinkedPoint()
	{
		if (linked_teleportation_point == null)
			return ;
		PlayerExploration.instance.transform.position = linked_teleportation_point.spawn_point.position;
		linked_teleportation_point.exploration_screen.SetCameraToThis();
	}
}
