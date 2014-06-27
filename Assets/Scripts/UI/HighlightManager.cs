
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighlightManager : MonoBehaviour
{
	public GameObject element1slot1;
	public GameObject element1slot2;
	public GameObject element1slot3;
	public GameObject element2slot1;
	public GameObject element2slot2;
	public GameObject element2slot3;
	public GameObject element3slot1;
	public GameObject element3slot2;
	public GameObject element3slot3;
	public GameObject element4slot1;
	public GameObject element4slot2;
	public GameObject element4slot3;

	public GameObject reset;
	private TweenPosition tp;

	static public HighlightManager instance;

	void Awake()
	{
		instance = this;
	}

	public void HighlightObject(GameObject obj)
	{

		tp = TweenPosition.Begin(obj, 0.25f, obj.transform.localPosition + new Vector3(0, 10, 0));
		tp.style = UITweener.Style.PingPong;

	}
	public void Stop()
	{
		tp.Reset();
	}
}

