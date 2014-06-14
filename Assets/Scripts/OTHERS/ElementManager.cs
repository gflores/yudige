using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementManager : MonoBehaviour {
	public static ElementManager instance {get; set;}
	public Element[] elements_list {get; set;}

	public List<ElementRelation> element_relations;
	
	void Awake () {
		instance = this;
	}
	
	void Start () {
		elements_list = GetComponentsInChildren<Element>();
	}


}
[System.Serializable]
public class ElementRelation
{
	public Element strong;
	public Element weak;
}