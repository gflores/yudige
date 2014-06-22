﻿using UnityEngine;
using System.Collections;

public class ExplorationScreen : MonoBehaviour {
	public AudioSource background_music;
	public Transform cameras_point;
	ScreenTeleportationPoint[] screen_teleportation_point_list {get; set;}
	void Awake () {
		screen_teleportation_point_list = GetComponentsInChildren<ScreenTeleportationPoint>();
		foreach(var point in screen_teleportation_point_list)
		{
			point.exploration_screen = this;
		}
	}
	public void MakeGoTo()
	{
		GameManager.instance.current_screen = this;
		SetCameraToThis();
		ApplyBackgroundMusic();
	}
	public void SetCameraToThis()
	{
		Vector3 new_cam_pos = cameras_point.position;
		new_cam_pos.z = GameManager.instance.exploration_camera.transform.position.z;
		GameManager.instance.exploration_camera.transform.position = new_cam_pos;
	}

	public void ApplyBackgroundMusic()
	{
		if (background_music != null)
		{
			SoundManager.instance.PlayIfDifferent(background_music);
		}
	}
}
