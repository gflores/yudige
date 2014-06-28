using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SequentialAction : MonoBehaviour
{
	public bool allow_accelerate = false;
	public float acceleration_ratio = 10f;
	
	bool _is_accelerated = false;
	IEnumerator _actions_enumerator;
	protected bool _can_do_next_action;

	public virtual void StartSequence()
	{
		WantDoNextAction();
	}
	void Awake()
	{
		Reinit();
		_can_do_next_action = true;
		MakeStart();
		AfterMakeStart();
	}
	public void Reinit()
	{
		_actions_enumerator = ActionList().GetEnumerator();
	}
	virtual protected void MakeStart()
	{
	}
	virtual protected void AfterMakeStart()
	{
	}
	
	virtual protected IEnumerable ActionList()
	{
		yield return new WaitForSeconds(0.001f);
	}

	public void WantDoNextAction()
	{
		if (_can_do_next_action)
			ForceDoNextAction();
		else if (allow_accelerate)
		{
			Time.timeScale = acceleration_ratio;
			if (_is_accelerated == false)
			{
				_is_accelerated = true;
				OnStartAcceleration();
			}
		}
	}
	
	public void ForceDoNextAction()
	{
		if (_actions_enumerator.MoveNext())
			StartCoroutine(ForceDoNextActionCoroutine());
		else
			Debug.LogWarning("plus rien a faire");
		
	}
	
	IEnumerator ForceDoNextActionCoroutine()
	{
		_can_do_next_action = false;
		yield return (Coroutine) _actions_enumerator.Current;
		_can_do_next_action = true;
		if (_is_accelerated == true)
		{
			_is_accelerated = false;
			Time.timeScale = 1f;
			OnEndAcceleration();
		}
	}
	
	virtual protected void OnStartAcceleration()
	{}
	virtual protected void OnEndAcceleration()
	{}
	
	public void AuthorizeNextAction()
	{
		_can_do_next_action = true;
	}
}