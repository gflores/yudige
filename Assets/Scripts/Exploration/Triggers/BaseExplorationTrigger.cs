using UnityEngine;
using System.Collections;

public class BaseExplorationTrigger : MonoBehaviour {
	public string tag_to_check;
	protected virtual void OnPlayerEnter()
	{
	}
	void  OnTriggerEnter2D(Collider2D other)
	{
		if ((tag_to_check == "" && TagManager.ContainsTag(other, "TAG_player_trigger_hitbox") == true) ||
		    (tag_to_check != "" && TagManager.ContainsTag(other, tag_to_check) == true))
		{
			OnPlayerEnter();
		}
	}

	protected virtual void OnPlayerExit()
	{
	}
	void  OnTriggerExit2D(Collider2D other)
	{
		if ((tag_to_check == "" && TagManager.ContainsTag(other, "TAG_player_trigger_hitbox") == true) ||
		    (tag_to_check != "" && TagManager.ContainsTag(other, tag_to_check) == true))
		{
			OnPlayerExit();
		}
	}
}
