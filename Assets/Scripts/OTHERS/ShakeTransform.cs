using UnityEngine;
using System.Collections;

public class ShakeTransform : MonoBehaviour {
	public Transform target_transform;

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
	
	Camera _camera_component = null;

	void Awake()
	{
		if (target_transform == null)
			target_transform = transform;
		_camera_component = target_transform.GetComponent<Camera>();
		StartCoroutine(LaunchShake());
	}
	int _transformations_number = 0;
	public IEnumerator LaunchShake()
	{
		_transformations_number++;
		StartCoroutine(LaunchMoveX());

		_transformations_number++;
		StartCoroutine(LaunchMoveY());

		_transformations_number++;
		StartCoroutine(LaunchScaleX());

		_transformations_number++;
		StartCoroutine(LaunchScaleY());

		_transformations_number++;
		StartCoroutine(LaunchRotate());
		if (_camera_component != null)
		{
			_transformations_number++;
			StartCoroutine(LaunchCameraZoom());
		}
		while (IsShakeFinished() == false)
			yield return null;
	}
	public bool IsShakeFinished()
	{
		return _transformations_number == 0;
	}

	public IEnumerator LaunchCameraZoom()
	{
		float prev_value = _camera_component.orthographicSize;
		
		yield return StartCoroutine(CurveHelper.LaunchCurveApply(cameras_zoom_curve, (time, val) => {
			_camera_component.orthographicSize = Mathf.Lerp(0f, prev_value, val);
		}, time_ratio));
		_camera_component.orthographicSize = prev_value;
		_transformations_number--;
	}
	
	public IEnumerator LaunchMoveX()
	{
		float prev_delta = 0;
		
		yield return StartCoroutine(CurveHelper.LaunchCurveApply(pos_x_curve, (time, val) => {
			float delta = Random.Range(-random_pos_x, random_pos_x) + val * value_ratio;
			target_transform.position = new Vector3(target_transform.position.x - prev_delta + delta , target_transform.position.y, target_transform.position.z);
			prev_delta = delta;
		}, time_ratio));
		target_transform.position = new Vector3(target_transform.position.x - prev_delta, target_transform.position.y, target_transform.position.z);
		_transformations_number--;
	}
	public IEnumerator LaunchMoveY()
	{
		float prev_delta = 0;
		
		yield return StartCoroutine(CurveHelper.LaunchCurveApply(pos_y_curve, (time, val) => {
			float delta = Random.Range(-random_pos_y, random_pos_y) + val * value_ratio;
			target_transform.position = new Vector3(target_transform.position.x, target_transform.position.y - prev_delta + delta, target_transform.position.z);
			prev_delta = delta;
		}, time_ratio));
		target_transform.position = new Vector3(target_transform.position.x, target_transform.position.y - prev_delta, target_transform.position.z);
		_transformations_number--;
	}

	public IEnumerator LaunchScaleX()
	{
		float prev_delta = 0;
		
		yield return StartCoroutine(CurveHelper.LaunchCurveApply(scale_x_curve, (time, val) => {
			float delta = Random.Range(-random_scale_x, random_scale_x) + val * value_ratio;
			target_transform.localScale = new Vector3(target_transform.localScale.x - prev_delta + delta , target_transform.localScale.y, target_transform.localScale.z);
			prev_delta = delta;
		}, time_ratio));
		target_transform.localScale = new Vector3(target_transform.localScale.x - prev_delta , target_transform.localScale.y, target_transform.localScale.z);
		_transformations_number--;
	}

	public IEnumerator LaunchScaleY()
	{
		float prev_delta = 0;
		
		yield return StartCoroutine(CurveHelper.LaunchCurveApply(scale_y_curve, (time, val) => {
			float delta = Random.Range(-random_scale_y, random_scale_y) + val * value_ratio;
			target_transform.localScale = new Vector3(target_transform.localScale.x , target_transform.localScale.y - prev_delta + delta, target_transform.localScale.z);
			prev_delta = delta;
		}, time_ratio));
		target_transform.localScale = new Vector3(target_transform.localScale.x , target_transform.localScale.y - prev_delta, target_transform.localScale.z);
		_transformations_number--;
	}


	public IEnumerator LaunchRotate()
	{
		float prev_delta = 0;
		
		yield return StartCoroutine(CurveHelper.LaunchCurveApply(angle_curve, (time, val) => {
			float delta = Random.Range(-random_angle, random_angle) + val * value_ratio;
			target_transform.localEulerAngles = new Vector3(target_transform.localEulerAngles.x , target_transform.localEulerAngles.y, target_transform.localEulerAngles.z - prev_delta + delta);
			prev_delta = delta;
		}, time_ratio));
		target_transform.localEulerAngles = new Vector3(target_transform.localEulerAngles.x , target_transform.localEulerAngles.y, target_transform.localEulerAngles.z - prev_delta);
		_transformations_number--;
	}
}
