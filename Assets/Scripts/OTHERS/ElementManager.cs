using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Element {
	DARK = 0,
	LIGHT,
	ROCK,
	FIRE,
	Count
}
public enum ElementRelation{
	STRONG,
	WEAK,
	NORMAL
};

public class ElementManager : MonoBehaviour {
	public static ElementManager instance {get; set;}
	public ElementInfo[] element_infos_list {get; set;}

	public List<ElementRelationDescription> element_relations_list;
	
	void Awake () {
		instance = this;
	}
	
	void Start () {
		element_infos_list = GetComponentsInChildren<ElementInfo>();
	}

	public ElementInfo GetElementInfo(Element element)
	{
		return System.Array.Find(element_infos_list, (e_info) => e_info.element == element);
	}
	public ElementRelation GetRelationBetween(Element attacking, Element defending)
	{
		foreach (var element_relation in element_relations_list)
		{
			if (attacking == element_relation.strong && defending == element_relation.weak)
				return ElementRelation.STRONG;
			else if (defending == element_relation.strong && attacking == element_relation.weak)
				return ElementRelation.WEAK;
		}
		return ElementRelation.NORMAL;
	}
}
[System.Serializable]
public class ElementRelationDescription
{
	public Element strong;
	public Element weak;
}

