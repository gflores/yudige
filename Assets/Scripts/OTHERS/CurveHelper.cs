using UnityEngine;
using System.Collections;

public class CurveHelper : MonoBehaviour {
	public delegate void ArgFloat_Float(float time, float val);
	
	static CurveHelper _instance;
	static public CurveHelper GetInstance(){return _instance;}
	
	void Awake()
	{
		_instance = this;
	}
	
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


//	static public IEnumerator LaunchColorTint(SkeletonAnimation skel, AnimationCurve curve,
//		float red, float green, float blue, bool revert_at_end = true, bool loop = false)
//	{
//		float prev_red = skel.skeleton.R;
//		float prev_green = skel.skeleton.G;
//		float prev_blue = skel.skeleton.B;
//		
//		
//		ArgFloat_Float func = (float time, float val) => {			
//				skel.skeleton.R =  Mathf.Lerp(prev_red, red, val);
//				skel.skeleton.G =  Mathf.Lerp(prev_green, green, val);
//				skel.skeleton.B =  Mathf.Lerp(prev_blue, blue, val);
//			};
//
//		//func_insert BEGIN
//		float time_ratio = 1f; float time_offset = 0f;
//		if (curve.keys.Length == 0)
//			yield break;
//		float total_time = curve.keys[curve.keys.Length-1].time;
//		do {
//			for (float current_time = 0f; Mathf.Abs(current_time) < Mathf.Abs(total_time); current_time += Time.deltaTime * time_ratio)
//			{
//				func(current_time, curve.Evaluate(current_time));
//				yield return new WaitForSeconds(0.001f);
//				//ssTime.fixedTime
//			}
//		} while (loop == true);
//		//func_insert END
//		if (revert_at_end)
//		{
//			skel.skeleton.R = prev_red;
//			skel.skeleton.G = prev_green;
//			skel.skeleton.B = prev_blue;
//		}
//	}

}
