using UnityEngine;
using System.Collections;

public class BaseExplorationInteraction : MonoBehaviour {
	public MosterData moster {get; set;}

	public virtual IEnumerator StartOnValidation()
	{
		yield return new WaitForSeconds(0.001f);
	}
	public virtual IEnumerator StartOnAgression()
	{
		BattleManager.instance.StartBattle(moster.moster_battle);
		yield return new WaitForSeconds(0.001f);
	}
}
