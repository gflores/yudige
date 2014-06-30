using UnityEngine;
using System.Collections;

public class SequentialEventValidate : SequentialAction {
	public bool is_active = false;
	public bool auto_pause = false;
	public override void StartSequence()
	{
		is_active = true;
		is_available = false;
		WantDoNextAction();
	}
	public IEnumerator Coroutine_LaunchStartSequence()
	{
		StartSequence();
		while (is_active == true)
			yield return new WaitForSeconds(0.001f);
	}
	protected virtual void BeforeUpdate(){}
	protected virtual void AfterUpdate(){}
	void Update()
	{
		BeforeUpdate();
		if (Input.GetButtonDown("Validate") == true && is_active == true)
		{
			Debug.LogWarning("okay ??");
			WantDoNextAction();
		}
		
		AfterUpdate();
	}

	protected void OnStartEvent()
	{
		if (auto_pause == true)
			MakePause();
		StateManager.instance.current_states.Add(StateManager.State.SCRIPTED_EVENT);
		StateManager.instance.UpdateFromStates();
	}
	public bool is_available = true;
	protected void OnEndEvent()
	{
		if (auto_pause == true)
			MakeUnpause();
		is_active = false;
		if (StateManager.instance.current_states.Contains(StateManager.State.BATTLE_INTRO) == false)
			StateManager.instance.current_states.Remove(StateManager.State.SCRIPTED_EVENT);
		StateManager.instance.UpdateFromStates();
		is_available = true;
		Debug.LogWarning("WHERERERERER");
//		Invoke("SetIsAvailable", 0.3f);
	}
//	void SetIsAvailable(){
//
//	}
	public void MakePause()
	{
		Time.timeScale = 0f;
	}
	public void MakeUnpause()
	{
		Time.timeScale = 1f;
	}
}
