using UnityEngine;
using System.Collections;

public class PilliersManager : MonoBehaviour {
	static public PilliersManager instance;
	public GameObject[] pilliers;
	void Awake(){
		instance = this;
	}

	public void RefreshPilliers()
	{
		int mosters_eliminated = MostersManager.instance.eliminated_mosters_list.Count;
		for (int i = 0; i != pilliers.Length; ++i)
		{
			if (mosters_eliminated > i )
				pilliers[i].SetActive(false);
			else
				pilliers[i].SetActive(true);

		}
	}
}
