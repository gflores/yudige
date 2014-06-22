using UnityEngine;
using System.Collections;

public class Rotater2D : MonoBehaviour {
	static public void LookAt(Transform item, Vector3 target_pos)
	{
		item.localEulerAngles = new Vector3(
			item.localEulerAngles.x,
			item.localEulerAngles.y,
			Math3d.AngleFromUp(target_pos - item.position)
			);
	}
	
	static public void LookAt(Transform item, Transform target_transform)
	{
		LookAt(item, target_transform.position);
	}
}
