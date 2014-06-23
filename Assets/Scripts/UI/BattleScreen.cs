
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleScreen : MonoBehaviour
{
	public static BattleScreen instance;
	public UISlider shield_slider;
	public UISlider boss_life_slider;
	public UILabel shield_label;
	public UILabel health_label;
	public Transform events_container;
	public UILabel event_template;

	void Awake()
	{
		instance = this;
	}

	public void SetupForBattle()
	{

	}

	void Update()
	{
		if (BattleManager.instance.IsBattleAlreadyLaunched ())
		{
			UpdateLife ();
			UpdateBossLife ();
			UpdateTimeline ();

		}
	}

	private void UpdateLife()
	{
		float max_shield = Player.instance.GetEffectiveMaxShield();
		float current_shield = PlayerBattle.instance.current_shield;
		shield_slider.sliderValue = current_shield / max_shield;
		shield_label.text = current_shield.ToString () + " / " + max_shield;
		health_label.text = Player.instance.current_life.ToString();
	}

	private void UpdateBossLife()
	{
		float max_health = BattleManager.instance.GetEnemyMaxLife();
		float current_health = BattleManager.instance.enemy_current_life;
		boss_life_slider.sliderValue = current_health / max_health;
	}

	private void UpdateTimeline()
	{
		foreach (Transform child in events_container)
		{
			GameObject.Destroy(child.gameObject);
		}
		List<TimelineEvent> events = EventsTimeline.instance.scheduled_events;
		foreach (TimelineEvent e in events)
		{
			GameObject n = (GameObject)(GameObject.Instantiate(event_template.gameObject));
			UILabel label = n.GetComponent<UILabel>();
			label.text = e.name;
			n.transform.SetParent(events_container);
			n.transform.localPosition = new Vector3(70, -140 + e.time_remaining * 50);
			n.transform.localScale = new Vector3(20, 20, 1);
		}
	}


}


