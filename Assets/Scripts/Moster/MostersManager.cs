using UnityEngine;
using System.Collections;

public class MostersManager : MonoBehaviour {
	public static MostersManager instance {get; set;}

	public MosterData[] mosters_list {get; set;}

	void Awake () {
		instance = this;
	}

	void Start () {
		mosters_list = GetComponentsInChildren<MosterData>();
	}
}
