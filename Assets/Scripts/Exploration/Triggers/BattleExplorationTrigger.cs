using UnityEngine;
using System.Collections;

public class BattleExplorationTrigger : BaseExplorationTrigger {
	public bool agro_on_contact = false;
	public MosterData moster_data;
	protected override void OnPlayerEnter()
	{
		if (agro_on_contact == true)
		{
			if (moster_data.moster_battle == null)
				Debug.LogWarning("nullll");
			BattleManager.instance.StartBattle(moster_data.moster_battle);
		}
	}
}
