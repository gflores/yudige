using UnityEngine;
using System.Collections;

public class TagManager {
	
	static public bool ContainsTag(GameObject o, string tag)
	{
		return o.transform.Find(tag) != null;
	}

	static public bool ContainsTag(Component o, string tag)
	{
		return o.transform.Find(tag) != null;
	}

}
