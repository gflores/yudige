﻿using UnityEngine;
using System.Collections;

public class ExplainEnemyAttack : SequentialEventValidate {
	protected override IEnumerable ActionList()
	{
		OnStartEvent();
		//Start
		
		yield return StartCoroutine(_0());		
		yield return StartCoroutine(_EndSequence());
		
		//End
		OnEndEvent();
	}
	IEnumerator _EndSequence(){
		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0));
		Debug.LogWarning("Finishing!");
		yield return null;
	}
	
	IEnumerator _0(){
		Debug.LogWarning("IN block-ExplainEnemyAttack");
		yield return null;
	}
}