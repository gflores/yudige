using UnityEngine;
using System.Collections;

public class CurveHelper : MonoBehaviour {
	public delegate void ArgFloat_Float(float time, float val);
	
	static public IEnumerator LaunchCurveApply(AnimationCurve curve, ArgFloat_Float func, float time_ratio = 1f, float time_offset = 0f, bool loop = false)
	{
		if (curve.keys.Length == 0)
			yield break;
		float total_time = curve.keys[curve.keys.Length-1].time;
		
		do {
			for (float current_time = 0f; Mathf.Abs(current_time) < Mathf.Abs(total_time); current_time += Time.deltaTime * time_ratio)
			{
				func(current_time, curve.Evaluate(current_time));
				yield return new WaitForSeconds(0.001f);
			}
		} while (loop == true);
	}
}
