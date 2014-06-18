using UnityEngine;
using System.Collections;

public class BaseExplorationTrigger : MonoBehaviour {
	protected virtual void OnPlayerEnter()
	{
	}
	void  OnTriggerEnter2D(Collider2D other)
	{
		if (TagManager.ContainsTag(other, "TAG_player_trigger_hitbox") == true)
		{
			OnPlayerEnter();
		}
	}

	protected virtual void OnPlayerExit()
	{
	}
	void  OnTriggerExit2D(Collider2D other)
	{
		if (TagManager.ContainsTag(other, "TAG_player_trigger_hitbox") == true)
		{
			OnPlayerExit();
		}
	}
}
