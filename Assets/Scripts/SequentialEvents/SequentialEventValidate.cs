using UnityEngine;
using System.Collections;

public class SequentialEventValidate : SequentialAction {

	public void StartSequence()
	{
		WantDoNextAction();
	}

	protected virtual void BeforeUpdate(){}
	protected virtual void AfterUpdate(){}
	void Update()
	{
		BeforeUpdate();
		if (Input.GetKeyDown(KeyCode.Z) == true)
		{
			WantDoNextAction();
		}
		AfterUpdate();
	}
}
