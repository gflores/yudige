using UnityEngine;
using System.Collections;

public enum DeathType {
	NATURAL,
	NO_HEALTH
}

public class DeathSceneScript : SequentialEventValidate {
	public DeathType death_type {get; set;}
	protected override IEnumerable ActionList()
	{
		OnStartEvent();
		//Start

		if (death_type == DeathType.NATURAL)
		{
			yield return StartCoroutine(_0_natural());		
		}
		else if (death_type == DeathType.NO_HEALTH)
		{
			yield return StartCoroutine(_0_no_health());
		}		
		//End
		PopupText.instance.Hide();
		OnEndEvent();
	}
	public IEnumerator Coroutine_NaturalDeath()
	{
		yield return StartCoroutine(_0_natural());
		yield return StartCoroutine(WaitForValidation());
		yield return StartCoroutine(_1_natural());
		yield return StartCoroutine(WaitForValidation());
		PopupText.instance.Hide();
	}
	public IEnumerator Coroutine_NoHealthDeath()
	{
		yield return StartCoroutine(_0_no_health());
		yield return StartCoroutine(WaitForValidation());
		yield return StartCoroutine(_1_no_health());
		yield return StartCoroutine(WaitForValidation());
		PopupText.instance.Hide();
	}
	IEnumerator WaitForValidation()
	{
		while(true)
		{
			if (Input.GetButtonDown("Validate"))
				yield break;
			yield return new WaitForSeconds(0.01f);
		}
	}
	IEnumerator _0_natural(){
		PopupText.instance.Show(0, 200, 500, 300);
		PopupText.instance.text_label.text = "";
		yield return StartCoroutine(StringUtils.LaunchProgressiveLabel(
			"Dead from old age\n",
			PopupText.instance.text_label
			));
		yield return null;
	}
	IEnumerator _1_natural(){
		PopupText.instance.text_label.text = "";
		yield return StartCoroutine(StringUtils.LaunchProgressiveLabel(
			"Dead from old age2\n",
			PopupText.instance.text_label
			));
		yield return null;
	}
	IEnumerator _0_no_health(){
		PopupText.instance.Show(0, 200, 500, 300);
		PopupText.instance.text_label.text = "";
		yield return StartCoroutine(StringUtils.LaunchProgressiveLabel(
			"How ironic it is for me to be taken out of existence.\n",
			PopupText.instance.text_label
			));
		yield return null;
	}
	IEnumerator _1_no_health(){
		PopupText.instance.text_label.text = "";
		yield return StartCoroutine(StringUtils.LaunchProgressiveLabel(
			"I shall come back in the most simple form.\n",
			PopupText.instance.text_label
			));
		yield return null;
	}

}
