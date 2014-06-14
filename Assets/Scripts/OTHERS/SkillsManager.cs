using UnityEngine;
using System.Collections;

public class SkillsManager : MonoBehaviour {
	public static SkillsManager instance;
	public Skill[] skills_list {get; set;}

	void Awake()
	{
		instance = this;
	}
	void Start()
	{
		skills_list = GetComponentsInChildren<Skill>();
	}
}
