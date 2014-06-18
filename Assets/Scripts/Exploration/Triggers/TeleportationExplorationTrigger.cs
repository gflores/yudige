using UnityEngine;
using System.Collections;

public class TeleportationExplorationTrigger : BaseExplorationTrigger {
	public ScreenTeleportationPoint teleportation_point;

	protected override void OnPlayerEnter()
	{
		teleportation_point.TeleportToLinkedPoint();
	}
}
