using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NpcMosterBattle : MonoBehaviour {
	public AudioSource battle_theme;
	public MosterData moster_data{get; set;}
	public int life = 100;
	public float approx_nb_attack_before_burst = 20;
	public float burst_attack_ratio = 2f;
	public int same_element_in_a_row_min = 2;
	public int same_element_in_a_row_max = 4;
	public float attack_total_time = 5f;
	public float attack_min_gap_time = 3f;
	public float max_increased_attack_speed = 1.5f;
	public float after_burst_attack_time = 10f;
	public float after_change_element_time = 2f;
	public int max_different_defense_element_nb = 3;
	public float phase_increment_bonus_ratio = 0.5f;
	public int karma_points_rewards = 10;

	public Transform visuals_transform;
	public SpriteRenderer main_renderer;
	public Animator generic_animator;
	public Animator sprite_animator;

	public List<Element> current_phase_attacks_elements {get; set;}
	public List<Element> current_defense_elements_list {get; set;}
	public int[] base_element_bonuses {get; set;}
	public int[] phase_element_bonuses {get; set;}
	public int GetEffectiveAffinity(Element element)
	{
		return moster_data.element_affinity_modifiers[(int)element] + base_element_bonuses[(int)element] + phase_element_bonuses[(int)element];
	}
	public void SetupForBattle()
	{
		generic_animator.SetBool("InBattle", true);
		current_phase_attacks_elements = new List<Element>();
		int prev_value = -99999999;
		BattleManager.instance.enemy_current_life = life;
		base_element_bonuses = new int[]{0, 0, 0, 0};
		phase_element_bonuses = new int[]{0, 0, 0, 0};
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

	Element GetRandomElement()
	{
		float total_affinity = 0;

		foreach (var el_val in moster_data.element_affinity_modifiers)
			total_affinity += el_val;
		float[] proba_list = new float[]{0f, 0f, 0f, 0f};

		float rand = Random.value;
		float previous_val = 0f;
//		Debug.LogWarning("RAND: " + rand);
		for (int i = 0; i < (int)Element.Count; ++i)
		{
			float curr_val = previous_val + (float) moster_data.element_affinity_modifiers[i] / total_affinity;
//			Debug.LogWarning("element: " + i + ", value: " + curr_val);
			if (rand >= previous_val && rand <= curr_val)
			{
//				Debug.LogWarning("returning");
				return (Element)i;
			}
			previous_val = curr_val;
		}
//		Debug.LogWarning("failure (-99999)");
		return (Element)(-99999);
	}
	void GeneratePhaseAttacks()
	{
		current_phase_attacks_elements.Clear();

		current_phase_attacks_elements.Add(GetRandomElement());
		for (int i = 0; i < approx_nb_attack_before_burst - 2; ++i)
		{
			Element el;
			while((el = GetRandomElement()) == current_phase_attacks_elements.Last());
			current_phase_attacks_elements.Add(el);
		}
		if (current_defense_elements_list.Count == 0)
			GenerateBurstElementList();

		Element el2;
		while((el2 = GetRandomElement()) == current_defense_elements_list[0] || el2 == current_phase_attacks_elements.Last());
		current_phase_attacks_elements.Add(el2);
		return ;
		///NOUVEAU SYSTEM, le code suivant n'est plus utilisé
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
		attack.damage = GetEffectiveAffinity(attack.element);
		if (is_burst)
			attack.damage = Mathf.CeilToInt(attack.damage * burst_attack_ratio);
		return attack;
	}
	public EnemyAttack GetNextAttack()
	{
		EnemyAttack attack = new EnemyAttack();
		attack.is_burst = false;
		attack.element = current_phase_attacks_elements[0];
		current_phase_attacks_elements.RemoveAt(0);
		Debug.LogWarning("element: " + (int)attack.element);
		attack.damage = GetEffectiveAffinity(attack.element);
		return attack;
	}
	public EnemyAttack GetBurstAttack(Element burst_element)
	{
		EnemyAttack attack = new EnemyAttack();
		attack.is_burst = true;
		attack.element = burst_element;
		attack.damage = Mathf.CeilToInt((float)GetEffectiveAffinity(attack.element) * burst_attack_ratio);
		return attack;
	}
	public void SetupForNewPhase()
	{
		phase_element_bonuses = new int[]{0, 0, 0, 0};
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
	void Shuffle<T>(IList<T> list)  
	{  
		System.Random rng = new System.Random();  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}
	void GenerateBurstElementList()
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
//		Shuffle(current_defense_elements_list);
	}
	public Element GetNextElement()
	{
		if (current_defense_elements_list.Count == 0)
		{
			GenerateBurstElementList();
		}
		Element elem = current_defense_elements_list[0];
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
