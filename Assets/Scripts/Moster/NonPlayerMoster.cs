using UnityEngine;
using System.Collections;

public class NonPlayerMoster : MonoBehaviour {
	public MosterData moster_data;
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
}