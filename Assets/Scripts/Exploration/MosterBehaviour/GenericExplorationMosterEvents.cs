using UnityEngine;
using System.Collections;

public class GenericExplorationMosterEvents : MonoBehaviour {
	public MosterData moster;
	bool player_action_trigger_inside = false;
	BaseExplorationInteraction base_exploration_interaction;

	void Start()
	{
		base_exploration_interaction = GetComponent<BaseExplorationInteraction>();
		if (base_exploration_interaction != null)
			base_exploration_interaction.moster = moster;
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
			if (Player.instance.is_living_time_passing && Input.GetButtonDown("Validate"))
			{
				if (base_exploration_interaction != null)
					StartCoroutine(base_exploration_interaction.StartOnValidation());
			}
			else if (Player.instance.is_living_time_passing && Input.GetButtonDown("Agress"))
			{
				if (base_exploration_interaction != null)
					StartCoroutine(base_exploration_interaction.StartOnAgression());
				else
					BattleManager.instance.StartBattle(moster.moster_battle);
			}
		}
	}
}
