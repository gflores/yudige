using UnityEngine;
using System.Collections;

public class StartScriptOnTrigger : BaseExplorationTrigger {
	public SequentialAction event_to_trigger;

	void Awake(){
		if (event_to_trigger == null)
			event_to_trigger = GetComponent<SequentialAction>();
	}
	protected override void OnPlayerEnter()
	{
		event_to_trigger.StartSequence();
	}
}
