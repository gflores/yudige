using UnityEngine;
using System.Collections;

public class PlayerExploration : MonoBehaviour {
	public static PlayerExploration instance;
	void Awake()
	{
		instance = this;
	}
}
