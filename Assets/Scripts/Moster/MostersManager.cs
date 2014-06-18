using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MostersManager : MonoBehaviour {
	public static MostersManager instance {get; set;}

	public MosterData[] mosters_list {get; set;}
	public List<MosterData> eliminated_mosters_list {get; set;}
	public List<MosterData> evolved_mosters_list {get; set;}

	public bool can_check_evolution {get; set;}
	float _evolution_check_total_time = 2f;
	float _evolution_check_time_increment = 1f;
	void Awake () {
		instance = this;
		mosters_list = GetComponentsInChildren<MosterData>();
		can_check_evolution = true;
	}

	void Start () {
		StartCoroutine(Coroutine_CheckEvolutions());
	}
	IEnumerator Coroutine_CheckEvolutions()
	{
		while (true)
		{
			float current_check_time = 0f;
			while (current_check_time < _evolution_check_total_time)
			{
				yield return new WaitForSeconds(_evolution_check_time_increment);
				if (can_check_evolution == true)
					current_check_time += _evolution_check_time_increment;
			}
			MosterData moster_data_to_evolve_to = GetMosterWithHighestEvolutionScore();
			if (moster_data_to_evolve_to != null)
				Player.instance.EvolveTo(moster_data_to_evolve_to);
			else
				Debug.LogWarning("Checked evolution: no evolution !");
		}
	}
	MosterData GetMosterWithHighestEvolutionScore()
	{
		List<MosterData> mosters_with_enough_score = new List<MosterData>();
		int current_highest_score = -1;
		foreach (var possible_evolution in Player.instance.current_moster.possible_evolution_list)
		{
			int score = possible_evolution.moster_evolution.GetCurrentScore();
			if (score >= possible_evolution.moster_evolution.score_needed)
			{
				if (score > current_highest_score)
				{
					current_highest_score = score;
					mosters_with_enough_score.Clear();
				}
				mosters_with_enough_score.Add(possible_evolution);
			}
		}
		if (mosters_with_enough_score.Count == 0)
			return null;

		foreach (var moster in mosters_with_enough_score)
		{
			Debug.LogWarning("can will maybe evolve to: " + moster.moster_name);
		}
		return mosters_with_enough_score[Random.Range(0, mosters_with_enough_score.Count)];
	}
	public MosterData IndexToMosterData(int index)
	{
		return mosters_list[index];
	}
	public int MosterDataToIndex(MosterData moster)
	{
		return mosters_list.IndexOf(moster);
	}

	public void AddToEliminated(MosterData moster)
	{
		eliminated_mosters_list.Add(moster);
	}
	public void AddToEvolved(MosterData moster)
	{
		evolved_mosters_list.Add(moster);
	}
	public bool IsEliminated(MosterData moster)
	{
		return eliminated_mosters_list.Contains(moster);
	}
	public bool WasEvolvedTo(MosterData moster)
	{
		return evolved_mosters_list.Contains(moster);
	}

}
