using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public static Player instance;

	public MosterData current_moster;

	void Awake () {
		instance = this;
	}
	
	void Update () {
	
	}
}
