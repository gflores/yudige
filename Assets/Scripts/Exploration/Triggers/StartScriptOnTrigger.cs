using UnityEngine;
using System.Collections;

public class StartScriptOnTrigger : BaseExplorationTrigger {
	public SequentialEventValidate event_to_trigger;

	void Awake(){
		if (event_to_trigger == null)
			event_to_trigger = GetComponent<SequentialEventValidate>();
	}
	protected override void OnPlayerEnter()
	{
		event_to_trigger.StartSequence();
	}
}
