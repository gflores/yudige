using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	public static CameraManager instance;

	public Animation main_camera_fade_animation;
	public Animation exploration_plane_behind_player_animation;
	public Camera main_camera {get; set;}
	public Camera exploration_camera {get; set;}
	public Camera battle_camera {get; set;}

	SpriteRenderer main_camera_plane_sprite_renderer;
	SpriteRenderer exploration_plane_behind_player_sprite_renderer;
	string transparent_animation_name = "ToTransparent";
	string opaque_animation_name = "ToOpaque";
	string exploration_camera_start_battle_name = "CameraExplorationStartBattle";
	Animation exploration_camera_animation;

	void Awake(){
		instance = this;
		exploration_camera = GameObject.FindGameObjectWithTag("ExplorationCamera").camera;
		exploration_camera_animation = exploration_camera.GetComponent<Animation>();

		battle_camera = GameObject.FindGameObjectWithTag("BattleCamera").camera;
		main_camera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		main_camera_plane_sprite_renderer = main_camera_fade_animation.GetComponent<SpriteRenderer>();

		exploration_plane_behind_player_sprite_renderer = exploration_plane_behind_player_animation.GetComponent<SpriteRenderer>();
	}

	public void SetColorToFadePlane(Color color)
	{
		main_camera_plane_sprite_renderer.color = color;
	}
	public void SetColorToBehindPlane(Color color)
	{
		exploration_plane_behind_player_sprite_renderer.color = color;
	}

	public IEnumerator COROUTINE_ExplorationPlaneBehindToTransparent(float time = 1f)
	{
		yield return StartCoroutine(Coroutine_LaunchAnimation(exploration_plane_behind_player_animation, transparent_animation_name, time));
	}
	public IEnumerator COROUTINE_ExplorationPlaneBehindToOpaque(float time = 1f)
	{
		yield return StartCoroutine(Coroutine_LaunchAnimation(exploration_plane_behind_player_animation, opaque_animation_name, time));
	}

	public IEnumerator COROUTINE_LaunchExplorationCameraStartBattleAnimation(float time = 1f)
	{
		yield return StartCoroutine(Coroutine_LaunchAnimation(exploration_camera_animation, exploration_camera_start_battle_name, time));
	}
	public IEnumerator COROUTINE_MainCameraFadeToTransparent(float time = 1f)
	{
		yield return StartCoroutine(Coroutine_LaunchAnimation(main_camera_fade_animation, transparent_animation_name, time));
	}
	public IEnumerator COROUTINE_MainCameraFadeToOpaque(float time = 1f)
	{
		yield return StartCoroutine(Coroutine_LaunchAnimation(main_camera_fade_animation, opaque_animation_name, time));
	}
	IEnumerator Coroutine_LaunchAnimation(Animation anim, string animation_name, float time)
	{
		anim[animation_name].speed = 1f / time;
		anim.Play(animation_name);
		yield return new WaitForSeconds(time);
	}
}
