using UnityEngine;
using System.Collections;

public class MosterEvolution : MonoBehaviour {
	public int score_needed = 100;
	public float lived_time_value = 10f;
	public float evolved_mosters_nb = 0;
	public float eliminated_mosters_nb = 0;
	public MosterData moster_data {get; set;}

	public int current_lived_time_score_bonus = 50;
	public int all_affinities_score_bonus = 50;
	public int shield_score_bonus = 10;
	public int alpha_transform_nb_score_bonus = 0;
	public int beta_transform_nb_score_bonus = 0;

	public int _score_generated;

	public int GetCurrentScore()
	{
		int score_generated = 0;

		if (AffinityConditionFulfilled() == true)
			score_generated += all_affinities_score_bonus;
		if (Player.instance.GetEffectiveMaxShield() >= shield_score_bonus)
			score_generated += shield_score_bonus;
		if (MostersManager.instance.evolved_mosters_list.Count >= evolved_mosters_nb)
			score_generated += alpha_transform_nb_score_bonus;
		if (MostersManager.instance.eliminated_mosters_list.Count >= eliminated_mosters_nb)
			score_generated += beta_transform_nb_score_bonus;
		if (Player.instance.time_lived >= lived_time_value)
			score_generated += current_lived_time_score_bonus;
		_score_generated = score_generated;
		return score_generated;
	}
	bool AffinityConditionFulfilled()
	{
		for (int i = 0; i != (int) Element.Count; ++i)
		{
			if (Player.instance.GetEffectiveElementAffinity((Element) i) < moster_data.element_affinity_modifiers[i])
				return false;
		}
		return true;
	}
}
