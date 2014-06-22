using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NpcMosterBattle : MonoBehaviour {
	public MosterData moster_data{get; set;}
	public int life = 100;
	public float approx_nb_attack_before_burst = 20;
	public float burst_attack_ratio = 2f;
	public int same_element_in_a_row_min = 2;
	public int same_element_in_a_row_max = 4;
	public float attack_total_time = 5f;
	public float attack_min_gap_time = 3f;
	public float max_increased_attack_speed = 1.5f;
	public float before_change_element_time = 10f;
	public float after_change_element_time = 2f;
	public int max_different_defense_element_nb = 3;

	public int karma_points_rewards = 10;

	public Transform visuals_transform;
	public SpriteRenderer main_renderer;
	public Animator generic_animator;
	public Animator sprite_animator;

	public List<Element> current_phase_attacks_elements {get; set;}
	public List<Element> current_defense_elements_list {get; set;}

	public void SetupForBattle()
	{
		current_phase_attacks_elements = new List<Element>();
		int prev_value = -99999999;
		BattleManager.instance.enemy_current_life = life;
		for (int i = 0; i != (int) Element.Count; ++i)
		{
			if (moster_data.element_affinity_modifiers[i] > prev_value)
			{
				prev_value = moster_data.element_affinity_modifiers[i];
				BattleManager.instance.enemy_current_element = (Element) i;
			}
		}
		current_defense_elements_list = new List<Element>();
		if (BattleManager.instance.regular_enemy_routine == true)
			BattleManager.instance.EnemyChangeElement(GetNextElement());
		else
			BattleManager.instance.enemy_has_element = false;

	}
	void GeneratePhaseAttacks()
	{
		current_phase_attacks_elements.Clear();

		float total_affinity = 0;
		
		foreach (var el_val in moster_data.element_affinity_modifiers)
			total_affinity += el_val;
		List<EnemyAttackSequence> sequence_bag = new List<EnemyAttackSequence>();
		for (int i = 0; i < (int)Element.Count; ++i)
		{
			float nb_ratio = (float) moster_data.element_affinity_modifiers[i] / total_affinity;
			int attacks_nb = Mathf.CeilToInt(nb_ratio * approx_nb_attack_before_burst);

			while (attacks_nb != 0)
			{
				EnemyAttackSequence sequence = new EnemyAttackSequence();
				sequence.element = (Element) i;
				sequence.number = Mathf.Min(attacks_nb, Random.Range(same_element_in_a_row_min, same_element_in_a_row_max));
				sequence_bag.Add(sequence);
				attacks_nb -= sequence.number;
			}
		}


		EnemyAttackSequence previous_sequence = null;
		while (sequence_bag.Count != 0)
		{
			EnemyAttackSequence sequence;

			sequence = sequence_bag[Random.Range(0, sequence_bag.Count - 1)];
			if (previous_sequence != null)
			{
				for (int i = 0; i != 100 && previous_sequence.element == sequence.element; ++i)
					sequence = sequence_bag[Random.Range(0, sequence_bag.Count - 1)];
			}
			previous_sequence = sequence;
			sequence_bag.Remove(sequence);
			for (int i = sequence.number; i != 0; --i)
				current_phase_attacks_elements.Add(sequence.element);
		}
	}
	public EnemyAttack GetAttack(Element element, bool is_burst = false)
	{
		EnemyAttack attack = new EnemyAttack();

		attack.is_burst = is_burst;
		attack.element = element;
		attack.damage = moster_data.element_affinity_modifiers[(int)attack.element];

		return attack;
	}
	public EnemyAttack GetNextAttack()
	{
		EnemyAttack attack = new EnemyAttack();
		attack.is_burst = false;
		attack.element = current_phase_attacks_elements[0];
		current_phase_attacks_elements.RemoveAt(0);
		attack.damage = moster_data.element_affinity_modifiers[(int)attack.element];
		return attack;
	}
	public EnemyAttack GetBurstAttack(Element burst_element)
	{
		EnemyAttack attack = new EnemyAttack();
		attack.is_burst = true;
		attack.element = burst_element;
		attack.damage = Mathf.CeilToInt((float)moster_data.element_affinity_modifiers[(int)attack.element] * burst_attack_ratio);
		return attack;
	}
	public void SetupForNewPhase()
	{
		GeneratePhaseAttacks();
	}
	public bool HasUsedAllAttacks()
	{
		return current_phase_attacks_elements.Count == 0;
	}
	public float[] GetBeforeAfterAttackTime()
	{
		float before_time;
		float after_time;

		float range = attack_total_time - attack_min_gap_time;
		float random_val = Random.Range(0f, range);

		before_time = (attack_min_gap_time / 2) + random_val;
		after_time = attack_total_time - before_time;
		Debug.LogWarning("beforetime : " + before_time + " aftertime: " + after_time);
		return new float[]{
			before_time,
			after_time
		};
	}
	public Element GetNextElement()
	{
		if (current_defense_elements_list.Count == 0)
		{
			List<EnemyAttackSequence> tmp_list = new List<EnemyAttackSequence>();

			for (int i = 0; i != (int) Element.Count; ++i)
			{
				EnemyAttackSequence tmp = new EnemyAttackSequence();

				tmp.element = (Element) i;
				tmp.number = moster_data.element_affinity_modifiers[i];
				tmp_list.Add(tmp);
			}

			for (int j = 0; j != max_different_defense_element_nb; ++j)
			{
				int previous_highest = -999999999;
				int highest_index = -1;
				for (int i = 0; i != tmp_list.Count; ++i)
				{
					if (tmp_list[i].number > previous_highest)
					{
						previous_highest = tmp_list[i].number;
						highest_index = i;
					}
				}
				Debug.LogWarning("index: " + highest_index);
				current_defense_elements_list.Add(tmp_list[highest_index].element);
				tmp_list.RemoveAt(highest_index);
			}
		}
		Element elem = current_defense_elements_list[Random.Range(0, current_defense_elements_list.Count - 1)];
		current_defense_elements_list.Remove(elem);
		return elem;
	}
}
[System.Serializable]
public class EnemyAttackSequence{
	public Element element;
	public int number;
}
[System.Serializable]
public class EnemyAttack{
	public Element element;
	public int damage;
	public bool is_burst = false;
}
