﻿using UnityEngine;
using System.Collections;

public class ScreenTeleportationPoint : MonoBehaviour {
	public ScreenTeleportationPoint linked_teleportation_point;
	public Transform spawn_point;
	public ExplorationScreen exploration_screen {get; set;}

	public void TeleportToLinkedPoint()
	{
		if (linked_teleportation_point == null)
			return ;
		if (GameManager.instance.current_screen == null || GameManager.instance.current_screen == exploration_screen)
		{
			if (TestManager.instance.quick_screens == true)
			{
				PlayerExploration.instance.transform.position = linked_teleportation_point.spawn_point.position;
				linked_teleportation_point.exploration_screen.MakeGoTo();
			}
			else
				StartCoroutine(ScreenTransition());
		}
	}
	IEnumerator ScreenTransition()
	{
		CameraManager.instance.SetColorToFadePlane(Color.black);
		StateManager.instance.current_states.Add(StateManager.State.SCRIPTED_EVENT);
		StateManager.instance.UpdateFromStates();
		yield return StartCoroutine(Coroutine_CustomTransition());
		StateManager.instance.current_states.Remove(StateManager.State.SCRIPTED_EVENT);
		StateManager.instance.UpdateFromStates();
	}
	protected virtual IEnumerator Coroutine_CustomTransition()
	{
		return Coroutine_BaseTransition();
////		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToOpaque(0.05f));
//		PlayerExploration.instance.transform.position = linked_teleportation_point.spawn_point.position;
//		linked_teleportation_point.exploration_screen.MakeGoTo();
////		yield return new WaitForSeconds(0.05f);
//
//		yield return StartCoroutine(SpecialEffectsManager.instance.Coroutine_StartTweenAlpha(
//			CameraManager.instance.main_camera_fade_animation.GetComponent<SpriteRenderer>(),
//			0.5f, 0, 0.2f));
//
////		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToTransparent(0.3f));
	}
	protected IEnumerator Coroutine_BaseTransition()
	{
		//		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToOpaque(0.05f));
		PlayerExploration.instance.transform.position = linked_teleportation_point.spawn_point.position;
		linked_teleportation_point.exploration_screen.MakeGoTo();
		//		yield return new WaitForSeconds(0.05f);
		
		yield return StartCoroutine(SpecialEffectsManager.instance.Coroutine_StartTweenAlpha(
			CameraManager.instance.main_camera_fade_animation.GetComponent<SpriteRenderer>(),
			0.5f, 0, 0.2f));
		
		//		yield return StartCoroutine(CameraManager.instance.COROUTINE_MainCameraFadeToTransparent(0.3f));
	}
}
