using UnityEngine;
using System.Collections;

public class MostersManager : MonoBehaviour {
	public static MostersManager instance {get; set;}

	public MosterData[] _moster_list {get; set;}

	void Awake () {
		instance = this;
	}

	void Start () {
		_moster_list = GetComponentsInChildren<MosterData>();
	}
}
