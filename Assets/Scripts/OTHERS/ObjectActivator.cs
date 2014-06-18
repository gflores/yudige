using UnityEngine;
using System.Collections;

public class ObjectActivator : MonoBehaviour {
	public GameObject[] to_activate;
	public GameObject[] to_deactivate;
	
	public void Change()
	{
		foreach(var obj in to_activate)
			obj.SetActive(true);
		foreach(var obj in to_deactivate)
			obj.SetActive(false);
	}
	
	public void Revert()
	{
		foreach(var obj in to_activate)
			obj.SetActive(false);
		foreach(var obj in to_deactivate)
			obj.SetActive(true);		
	}
	
}
