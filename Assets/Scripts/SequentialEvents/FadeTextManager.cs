using UnityEngine;
using System.Collections;

public class FadeTextManager : MonoBehaviour {
	public static FadeTextManager instance;

	void Awake(){
		instance = this;
	}


}
