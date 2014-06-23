using UnityEngine;
using System.Collections;

public class TotoExploration : BaseExplorationInteraction {

	public override IEnumerator StartOnValidation()
	{
		Debug.LogWarning("hello !!");
		yield return new WaitForSeconds(0.001f);
	}
}
