using UnityEngine;
using System.Collections;

public class TataExploration : BaseExplorationInteraction {

	public override IEnumerator StartOnAgression()
	{
		Debug.LogWarning("FUCK YOU");
		yield return new WaitForSeconds(1f);
		BattleManager.instance.StartBattle(moster.moster_battle);//tti
	}
}
