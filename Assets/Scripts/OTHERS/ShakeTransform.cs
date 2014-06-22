using UnityEngine;
using System.Collections;

public class ShakeTransform : MonoBehaviour {
	public AnimationCurve pos_x_curve;
	public float random_pos_x;
	public AnimationCurve pos_y_curve;
	public float random_pos_y;
	
	public AnimationCurve scale_x_curve;
	public float random_scale_x;
	public AnimationCurve scale_y_curve;
	public float random_scale_y;
	
	public AnimationCurve angle_curve;
	public float random_angle;
	
	public float value_ratio = 1f;
	public float time_ratio = 1f;

	public AnimationCurve cameras_zoom_curve;

	void Start()
	{
//		StartCoroutine(LaunchShake(transform));
	}
	public IEnumerator LaunchShake(Transform target_transform)
	{
		ShakeCounter counter = new ShakeCounter();
		counter._transformations_number = 0;
		Camera _camera_component = target_transform.GetComponent<Camera>();

		counter._transformations_number++;
		StartCoroutine(LaunchMoveX(target_transform, counter));

		counter._transformations_number++;
		StartCoroutine(LaunchMoveY(target_transform, counter));

		counter._transformations_number++;
		StartCoroutine(LaunchScaleX(target_transform, counter));

		counter._transformations_number++;
		StartCoroutine(LaunchScaleY(target_transform, counter));

		counter._transformations_number++;
		StartCoroutine(LaunchRotate(target_transform, counter));
		if (_camera_component != null)
		{
			counter._transformations_number++;
			StartCoroutine(LaunchCameraZoom(target_transform, _camera_component, counter));
		}
		while (counter._transformations_number != 0)
			yield return null;
	}

	IEnumerator LaunchCameraZoom(Transform target_transform, Camera _camera_component, ShakeCounter counter)
	{
		float prev_value = _camera_component.orthographicSize;
		
		yield return StartCoroutine(CurveHelper.LaunchCurveApply(cameras_zoom_curve, (time, val) => {
			_camera_component.orthographicSize = Mathf.Lerp(0f, prev_value, val);
		}, time_ratio));
		_camera_component.orthographicSize = prev_value;
		counter._transformations_number--;
	}
	
	IEnumerator LaunchMoveX(Transform target_transform, ShakeCounter counter)
	{
		float prev_delta = 0;
		
		yield return StartCoroutine(CurveHelper.LaunchCurveApply(pos_x_curve, (time, val) => {
			float delta = (Random.Range(-random_pos_x, random_pos_x) + val) * value_ratio;
			target_transform.position = new Vector3(target_transform.position.x - prev_delta + delta , target_transform.position.y, target_transform.position.z);
			prev_delta = delta;
		}, time_ratio));
		target_transform.position = new Vector3(target_transform.position.x - prev_delta, target_transform.position.y, target_transform.position.z);
		counter._transformations_number--;
	}
	IEnumerator LaunchMoveY(Transform target_transform, ShakeCounter counter)
	{
		float prev_delta = 0;
		
		yield return StartCoroutine(CurveHelper.LaunchCurveApply(pos_y_curve, (time, val) => {
			float delta = (Random.Range(-random_pos_y, random_pos_y) + val) * value_ratio;
			target_transform.position = new Vector3(target_transform.position.x, target_transform.position.y - prev_delta + delta, target_transform.position.z);
			prev_delta = delta;
		}, time_ratio));
		target_transform.position = new Vector3(target_transform.position.x, target_transform.position.y - prev_delta, target_transform.position.z);
		counter._transformations_number--;
	}

	IEnumerator LaunchScaleX(Transform target_transform, ShakeCounter counter)
	{
		float prev_delta = 0;
		
		yield return StartCoroutine(CurveHelper.LaunchCurveApply(scale_x_curve, (time, val) => {
			float delta = (Random.Range(-random_scale_x, random_scale_x) + val) * value_ratio;
			target_transform.localScale = new Vector3(target_transform.localScale.x - prev_delta + delta , target_transform.localScale.y, target_transform.localScale.z);
			prev_delta = delta;
		}, time_ratio));
		target_transform.localScale = new Vector3(target_transform.localScale.x - prev_delta , target_transform.localScale.y, target_transform.localScale.z);
		counter._transformations_number--;
	}

	IEnumerator LaunchScaleY(Transform target_transform, ShakeCounter counter)
	{
		float prev_delta = 0;
		
		yield return StartCoroutine(CurveHelper.LaunchCurveApply(scale_y_curve, (time, val) => {
			float delta = (Random.Range(-random_scale_y, random_scale_y) + val) * value_ratio;
			target_transform.localScale = new Vector3(target_transform.localScale.x , target_transform.localScale.y - prev_delta + delta, target_transform.localScale.z);
			prev_delta = delta;
		}, time_ratio));
		target_transform.localScale = new Vector3(target_transform.localScale.x , target_transform.localScale.y - prev_delta, target_transform.localScale.z);
		counter._transformations_number--;
	}


	IEnumerator LaunchRotate(Transform target_transform, ShakeCounter counter)
	{
		float prev_delta = 0;
		
		yield return StartCoroutine(CurveHelper.LaunchCurveApply(angle_curve, (time, val) => {
			float delta = (Random.Range(-random_angle, random_angle) + val) * value_ratio;
			target_transform.localEulerAngles = new Vector3(target_transform.localEulerAngles.x , target_transform.localEulerAngles.y, target_transform.localEulerAngles.z - prev_delta + delta);
			prev_delta = delta;
		}, time_ratio));
		target_transform.localEulerAngles = new Vector3(target_transform.localEulerAngles.x , target_transform.localEulerAngles.y, target_transform.localEulerAngles.z - prev_delta);
		counter._transformations_number--;
	}
}
public class ShakeCounter{
	public int _transformations_number = 0;
}