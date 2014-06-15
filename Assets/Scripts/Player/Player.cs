using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public static Player instance;

	public MosterData current_moster;

	public int base_shield {get; set;}
	public int[] base_element_affinities{get;set;}

	public int current_life {get; set;}
	public int current_shield {get; set;}

	void Awake () {
		instance = this;
		base_element_affinities = new int[(int)Element.Count];
	}
	
	public int GetEffectiveMaxShield()
	{
		return base_shield + current_moster.shield_modifier;
	}

	public int GetEffectiveElementAffinity(Element element)
	{
		return base_element_affinities[(int)element] + current_moster.element_affinity_modifiers[(int)element];
	}

	public void EvolveTo(MosterData new_moster)
	{
		ApplyEvolutionChanges(new_moster);
	}
	public void ApplyEvolutionChanges(MosterData new_moster)
	{
		current_life += new_moster.life_bonus;
		current_moster = new_moster;
	}
}