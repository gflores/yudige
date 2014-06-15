using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MosterData : MonoBehaviour {
	public string moster_name;
	public int life_bonus = 10;
	public int shield_modifier = 10;
	public int[] element_affinity_modifiers;
	public List<Skill> skills;
}
