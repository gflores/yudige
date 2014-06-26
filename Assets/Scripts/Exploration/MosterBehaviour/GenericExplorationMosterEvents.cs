using UnityEngine;
using System.Collections;

public class GenericExplorationMosterEvents : MonoBehaviour {
	public MosterData moster;
	bool player_action_trigger_inside = false;
	public BaseExplorationInteraction on_validate;
	public BaseExplorationInteraction on_agress;

	void Start()
	{
		if (on_validate != null)
			on_validate.moster_data = moster;
		if (on_agress != null)
			on_agress.moster_data = moster;
	}
	void  OnTriggerEnter2D(Collider2D other)
	{
		if (TagManager.ContainsTag(other, "TAG_player_action_trigger_hitbox") == true)
		{
			player_action_trigger_inside = true;
		}
	}

	void  OnTriggerExit2D(Collider2D other)
	{
		if (TagManager.ContainsTag(other, "TAG_player_action_trigger_hitbox") == true)
		{
			player_action_trigger_inside = false;
		}
	}//titi

	void Update () {
		if (player_action_trigger_inside == true)
		{
			if (Player.instance.is_living_time_passing && Input.GetButtonDown("Validate") &&
			    StateManager.instance.current_states.Contains(StateManager.State.SCRIPTED_EVENT) == false &&
			    StateManager.instance.current_states.Contains(StateManager.State.EXPLORATION) == true)
			{
				if ((on_validate != null && on_validate.is_available == false) ||
				    (on_agress != null && on_agress.is_available == false))
					return ;
				if (on_validate != null)
				{
					on_validate.Reinit();
					StartCoroutine(on_validate.Coroutine_LaunchStartSequence());
				}
			}
			else if (Player.instance.is_living_time_passing && Input.GetButtonDown("Agress") &&
			         StateManager.instance.current_states.Contains(StateManager.State.SCRIPTED_EVENT) == false &&
			         StateManager.instance.current_states.Contains(StateManager.State.EXPLORATION) == true)
			{
				if ((on_validate != null && on_validate.is_available == false) ||
				    (on_agress != null && on_agress.is_available == false))
					return ;
				if (on_agress != null)
				{
					on_agress.Reinit();
					StartCoroutine(on_agress.Coroutine_LaunchStartSequence());
				}
				else
					BattleManager.instance.StartBattle(moster.moster_battle);
			}
		}
	}
}
