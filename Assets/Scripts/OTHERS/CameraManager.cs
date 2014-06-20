using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	public static CameraManager instance;

	public Animation main_camera_fade_animation;

	public Camera main_camera {get; set;}
	public Camera exploration_camera {get; set;}
	public Camera battle_camera {get; set;}

	SpriteRenderer main_camera_plane_sprite_renderer;
	string main_camera_fade_to_transparent = "ToTransparent";
	string main_camera_fade_to_opaque = "ToOpaque";

	void Awake(){
		instance = this;
		exploration_camera = GameObject.FindGameObjectWithTag("ExplorationCamera").camera;
		battle_camera = GameObject.FindGameObjectWithTag("BattleCamera").camera;
		main_camera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		main_camera_plane_sprite_renderer = main_camera_fade_animation.GetComponent<SpriteRenderer>();
	}

	public void SetColorToFadePlane(Color color)
	{
		main_camera_plane_sprite_renderer.color = color;
	}
	public IEnumerator COROUTINE_MainCameraFadeToTransparent(float time = 1f)
	{
		main_camera_fade_animation[main_camera_fade_to_transparent].speed = 1f / time;
		main_camera_fade_animation.Play(main_camera_fade_to_transparent);
		yield return new WaitForSeconds(time);
	}
	public IEnumerator COROUTINE_MainCameraFadeToOpaque(float time = 1f)
	{
		main_camera_fade_animation[main_camera_fade_to_opaque].speed = 1f / time;
		main_camera_fade_animation.Play(main_camera_fade_to_opaque);
		yield return new WaitForSeconds(time);
	}
}
