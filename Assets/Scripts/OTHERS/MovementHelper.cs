using UnityEngine;
using System.Collections;

public class MovementHelper : MonoBehaviour
{
//	static public void TweenPositionToPoint(Transform subject, Transform dest_transform, float time)
//	{
//		Transform old_parent = dest_transform.parent;
//		dest_transform.parent = subject.parent;
//		TweenPosition.Begin(subject.gameObject, time, dest_transform.localPosition);
//		dest_transform.parent = old_parent;
//	}
//
//	static public IEnumerator LaunchSmoothScale(Transform t, Vector3 dest_scale, float time, bool want_precision = false)
//	{
//		Vector3 delta_scale = (dest_scale - t.localScale);
//		float prev_time = 0f;
//		
//		if (time > 0)
//		{
//			for (float curr_t = 0; curr_t < time;)
//			{
//				t.localScale = t.localScale + delta_scale * (Time.deltaTime / time);
//				prev_time = curr_t;
//				curr_t += Time.deltaTime;
//				if (want_precision == true && curr_t > time)
//					t.localScale = dest_scale;
//				yield return new WaitForSeconds(0.001f);
//			}
////			t.localScale = t.localScale + delta_scale * ((time - prev_time) / time);
//		}
//		if (want_precision == true)
//			t.localScale = dest_scale;
//	}
//
//	static public IEnumerator LaunchSmoothRotation(Transform t, Vector3 dest_rotation, float time, ReactToTimeTwist react_to_time_twist = null)
//	{
//		if (time > 0)
//		{
//			Vector3 delta_rotation = (dest_rotation - t.localEulerAngles) / time;
//			if (react_to_time_twist != null)
//			{
//				for (float curr_t = 0; curr_t < time; curr_t += Time.deltaTime * react_to_time_twist.GetMoveSpeedRatio())
//				{
//					t.Rotate(delta_rotation * Time.deltaTime * react_to_time_twist.GetMoveSpeedRatio());
//					yield return new WaitForSeconds(0.001f);
//				}
//			}
//			else
//			{
//				for (float curr_t = 0; curr_t < time; curr_t += Time.deltaTime)
//				{
//					t.Rotate(delta_rotation * Time.deltaTime);
//					yield return new WaitForSeconds(0.001f);
//				}
//			}
//		}
////		t.localEulerAngles = dest_rotation;
//	}
//
//	static public IEnumerator LaunchSmoothMovement(Transform t, Vector2 dest_pos, float time, bool want_precision = true, ReactToTimeTwist react_to_time_twist = null)
//	{
//		Vector2 current_pos = t.position;
//		Vector2 velocity_vector = (dest_pos - current_pos).normalized * (Vector2.Distance(current_pos, dest_pos) / time);
//		
//		if (react_to_time_twist != null)
//		{
//			for (float curr_t = 0; curr_t < time; curr_t += Time.deltaTime * react_to_time_twist.GetMoveSpeedRatio())
//			{
//				t.Translate(velocity_vector * Time.deltaTime * react_to_time_twist.GetMoveSpeedRatio());
//				yield return new WaitForSeconds(0.001f);
//			}
//		}
//		else
//		{
//			for (float curr_t = 0; curr_t < time; curr_t += Time.deltaTime)
//			{
//				t.Translate(velocity_vector * Time.deltaTime);
//				yield return new WaitForSeconds(0.001f);
//			}			
//		}
//		if (want_precision == true)
//			t.position = (Vector3)dest_pos + new Vector3(0, 0, t.position.z);
//	}
//	
//	static public IEnumerator LaunchSmoothMovementWithVelocity(Transform t, Vector2 dest_pos, Vector2 velocity_vector, bool want_precision = true)
//	{
//		float time = Vector2.Distance(t.position, dest_pos) / velocity_vector.magnitude;
//		
//		for (float curr_t = 0; curr_t < time; curr_t += Time.deltaTime)
//		{
//			t.Translate(velocity_vector * Time.deltaTime);
//			yield return new WaitForSeconds(0.001f);
//		}
//		if (want_precision == true)
//			t.position = dest_pos;
//	}
//	
//	static public IEnumerator LaunchSmoothMovementWithVelocity(Transform t, Vector2 dest_pos, float velocity, float smooth_start_distance, float smooth_end_distance, float minimum_speed_ratio, ReactToTimeTwist react_to_time_twist = null)
//	{
//		Vector2 delta_move;
//		float estimated_travel_distance = Vector2.Distance(t.position, dest_pos);
//		float starting_end_smooth_distance = estimated_travel_distance - smooth_end_distance;
//		float current_traveled_distance = 0f;
//		float base_velocity = velocity;
//		
//		
//		if (current_traveled_distance < smooth_start_distance)
//			delta_move = (dest_pos - (Vector2)t.position).normalized * Time.deltaTime * velocity * Mathf.Max(current_traveled_distance /smooth_start_distance, minimum_speed_ratio) * (react_to_time_twist != null ? react_to_time_twist.GetMoveSpeedRatio() : 1f);
//		else if (current_traveled_distance > starting_end_smooth_distance)
//			delta_move = (dest_pos - (Vector2)t.position).normalized * Time.deltaTime * velocity * Mathf.Max(1f - (current_traveled_distance - starting_end_smooth_distance) / smooth_end_distance, minimum_speed_ratio) * (react_to_time_twist != null ? react_to_time_twist.GetMoveSpeedRatio() : 1f);
//		else
//			delta_move = (dest_pos - (Vector2)t.position).normalized * Time.deltaTime * velocity * (react_to_time_twist != null ? react_to_time_twist.GetMoveSpeedRatio() : 1f);
//
//		while (Vector2.Distance(t.position, dest_pos) > delta_move.magnitude)
//		{
//			t.Translate(delta_move);
//			if (current_traveled_distance < smooth_start_distance)
//				delta_move = (dest_pos - (Vector2)t.position).normalized * Time.deltaTime * velocity * Mathf.Max(current_traveled_distance /smooth_start_distance, minimum_speed_ratio) * (react_to_time_twist != null ? react_to_time_twist.GetMoveSpeedRatio() : 1f);
//			else if (current_traveled_distance > starting_end_smooth_distance)
//				delta_move = (dest_pos - (Vector2)t.position).normalized * Time.deltaTime * velocity * Mathf.Max(1f - (current_traveled_distance - starting_end_smooth_distance) / smooth_end_distance, minimum_speed_ratio) * (react_to_time_twist != null ? react_to_time_twist.GetMoveSpeedRatio() : 1f);
//			else
//				delta_move = (dest_pos - (Vector2)t.position).normalized * Time.deltaTime * velocity * (react_to_time_twist != null ? react_to_time_twist.GetMoveSpeedRatio() : 1f);
//			current_traveled_distance += delta_move.magnitude;
//			yield return new WaitForSeconds(0.001f);
//		}
//		t.position = dest_pos;
//	}
//
//	static public IEnumerator LaunchSmoothMovementWithVelocity(Transform t, Vector2 dest_pos, float velocity, bool want_precision = true)
//	{
//		Vector2 delta_move = (dest_pos - (Vector2)t.position).normalized * Time.deltaTime * velocity;
//		while (Vector2.Distance(t.position, dest_pos) > delta_move.magnitude)
//		{
//			t.Translate(delta_move);
//			delta_move = (dest_pos - (Vector2)t.position).normalized * Time.deltaTime * velocity;
//			yield return new WaitForSeconds(0.001f);
//		}
//		t.position = dest_pos;
//	}
//	
//	static public IEnumerator LaunchSmoothMovementWithVelocityBlockable(Transform t, Vector2 dest_pos, float velocity, GetBlockedByWalls get_blocked_by_walls, ReactToTimeTwist react_to_time_twist = null)
//	{
//		Vector2 delta_move = (dest_pos - (Vector2)t.position).normalized * Time.deltaTime * velocity * (react_to_time_twist != null ? react_to_time_twist.GetMoveSpeedRatio() : 1f);
//		while (Vector2.Distance(t.position, dest_pos) > delta_move.magnitude)
//		{
//			if (get_blocked_by_walls.IsBlocked == false)
//				t.Translate(delta_move);
//			delta_move = (dest_pos - (Vector2)t.position).normalized * Time.deltaTime * velocity * (react_to_time_twist != null ? react_to_time_twist.GetMoveSpeedRatio() : 1f);
//			yield return new WaitForSeconds(0.001f);
//		}
//		t.position = dest_pos;
//	}
//
//	
//	static public IEnumerator LaunchSmoothMovementWithVelocityAndLimits(Transform t, Vector2 dest_pos, float velocity, Vector2 left_bot_limit, Vector2 right_top_limit)
//	{
//		Vector2 current_pos = t.position;
//		Vector2 velocity_vector = (dest_pos - current_pos).normalized * velocity;
//		float time = Vector2.Distance(t.position, dest_pos) / velocity_vector.magnitude;
//
//		for (float curr_t = 0; curr_t < time; curr_t += Time.deltaTime)
//		{
//			t.Translate(velocity_vector * Time.deltaTime);
//			yield return new WaitForSeconds(0.001f);
//			current_pos = t.position;
//			if (current_pos.x < left_bot_limit.x || current_pos.x > right_top_limit.x || current_pos.y < left_bot_limit.y || current_pos.y > right_top_limit.y)
//			{
//				dest_pos = new Vector2(
//					Mathf.Clamp(current_pos.x, left_bot_limit.x, right_top_limit.x),
//					Mathf.Clamp(current_pos.y, left_bot_limit.y, right_top_limit.y)
//				);
////				dest_pos = current_pos - (velocity_vector * Time.deltaTime);
//				break;
//			}
//		}
//		t.position = dest_pos;
//	}
//
//	static public IEnumerator LaunchSmoothMovement3D(Transform t, Vector3 dest_pos, float time)
//	{
//		Vector3 current_pos = t.position;
//		Vector3 velocity_vector = (dest_pos - current_pos).normalized * (Vector3.Distance(current_pos, dest_pos) / time);
//		
//		for (float curr_t = 0; curr_t < time; curr_t += Time.deltaTime)
//		{
//			t.Translate(velocity_vector * Time.deltaTime);
//			yield return new WaitForSeconds(0.001f);
//		}
//		t.position = dest_pos;
//	}
//
//
//	static public IEnumerator LaunchRushToOrEscapeIfNear(Transform actor, Vector2 target_pos, int move_amplitude, int security_perimeter, float time)
//	{
//		Vector2 res_vector = target_pos - (Vector2)actor.position;
//		float distance = res_vector.magnitude;
//		Vector2 final_position = new Vector2();
//		if (distance < security_perimeter + move_amplitude)
//			final_position = (Vector2)actor.position + ((-res_vector.normalized) * move_amplitude);
//		else if (distance < security_perimeter + (move_amplitude * 2))
//			final_position = (Vector2)actor.position + VectorUtil.Random2D * move_amplitude;
//		else
//			final_position = (Vector2)actor.position + ((res_vector.normalized) * move_amplitude);
//			
//		return LaunchSmoothMovement(
//			actor,
//			KeyBoundaries.GetInstance("LaunchRushToOrEscapeIfNear").ApplyLimitation(final_position),
//			time);
//	}
//
//	static public IEnumerator LaunchGraduallyGoToOrStop(Transform actor, Transform target, int move_amplitude, int stop_perimeter, float time)
//	{
//		for (float current_time = 0; current_time < time; current_time += Time.deltaTime)
//		{
//			if (Mathf.Abs(actor.position.x - target.position.x) > stop_perimeter)
//			{
//				if (actor.position.x > target.position.x)
//					actor.position = actor.position + new Vector3(-move_amplitude * Time.deltaTime, 0, 0);
//				else
//					actor.position = actor.position + new Vector3(move_amplitude * Time.deltaTime, 0, 0);
//			}
//			yield return new WaitForSeconds(0.001f);
//		}
//	}

}

