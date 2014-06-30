
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
	public GameObject event_template;
	public UILabel player_damage;
	public UILabel boss_damage;
	public UIButton cancel_button;
	public UISprite timeline_preview;

	public GameObject disabled_skills;

	public UILabel boss_dark_label;
	public UILabel boss_light_label;
	public UILabel boss_rock_label;
	public UILabel boss_fire_label;

	void Awake()
	{
		instance = this;
	}

	public void SetupForBattle()
	{
		UICamera.selectedObject = cancel_button.gameObject;
	}

	void Update()
	{
		if (BattleManager.instance.IsBattleAlreadyLaunched ())
		{
			UpdateLife ();
			UpdateBossLife ();
			UpdateTimeline ();

			cancel_button.isEnabled = PlayerBattle.instance.is_casting_skill != true && PlayerBattle.instance.bonus_affinity_to_be_added_next != 0;
			disabled_skills.SetActive(PlayerBattle.instance.is_casting_skill == true);


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
		boss_dark_label.text = BattleManager.instance.enemy_moster.GetEffectiveAffinity(Element.DARK).ToString();
		boss_light_label.text = BattleManager.instance.enemy_moster.GetEffectiveAffinity(Element.LIGHT).ToString();
		boss_rock_label.text = BattleManager.instance.enemy_moster.GetEffectiveAffinity(Element.ROCK).ToString();
		boss_fire_label.text = BattleManager.instance.enemy_moster.GetEffectiveAffinity(Element.FIRE).ToString();

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
			GameObject n = (GameObject)(GameObject.Instantiate(event_template));
			n.SetActive(true);
			UILabel label = n.GetComponentInChildren<UILabel>();
			UISprite sprite = n.GetComponentInChildren<UISprite>();
			label.text = e.name;
			label.pivot = e.side == TimelineSide.PLAYER ? UIWidget.Pivot.Right : UIWidget.Pivot.Left;
			sprite.pivot = e.side == TimelineSide.PLAYER ? UIWidget.Pivot.Right : UIWidget.Pivot.Left;
			label.transform.localPosition = new Vector3(e.side == TimelineSide.PLAYER ? -10 : 10, 0);
			sprite.transform.localPosition = new Vector3(e.side == TimelineSide.PLAYER ? -5 : 5, 0);


			n.transform.SetParent(events_container);
			n.transform.localPosition = new Vector3(0, -275 + (e.time_remaining / EventsTimeline.instance.total_time) * 550);
			n.transform.localScale = new Vector3(1,1);



			if (e.event_type == TimelineEventType.ENEMY_SIMPLE_ATTACK || e.event_type == TimelineEventType.PLAYER_NORMAL_ATTACK)
			{
				label.transform.localScale = new Vector3(20, 20, 1);
			}
			else if (e.event_type == TimelineEventType.ENEMY_BURST_ATTACK || e.event_type == TimelineEventType.PLAYER_BURST_ATTACK)
			{
				label.transform.localScale = new Vector3(30, 30, 1);
			}
			else if (e.event_type == TimelineEventType.PLAYER_CANCEL_COMBOS)
			{
				label.transform.localScale = new Vector3(10, 10, 1);
			}
		}
	}

	public void DamageToPlayer(object to_display)
	{
		DisplayDamage (player_damage, to_display);
	}

	public void DamageToBoss(object to_display)
	{
		DisplayDamage (boss_damage, to_display);
	}

	private void DisplayDamage(UILabel lbl, object to_display)
	{
		lbl.gameObject.SetActive (true);
		lbl.text = to_display.ToString ();
		TweenPosition tp = TweenPosition.Begin(lbl.gameObject, 1, new Vector3(50,50, 0));
		tp.onFinished += OnAnimFinishedHandler;
		tp.method = UITweener.Method.BounceIn;
	}

	public void OnAnimFinishedHandler(UITweener tweener)
	{

		tweener.gameObject.SetActive (false);
		tweener.transform.localPosition = new Vector3 (0, 0, 0);
	}

	void Burst()
	{
		PlayerBattle.instance.BurstAffinities ();
	}

	void ResetAffinities()
	{
		PlayerBattle.instance.CancelCombos();
	}

	public void SkillTimelinePreview(Skill sk)
	{
		if (sk != null)
		{
			timeline_preview.gameObject.SetActive(true);
			timeline_preview.transform.localPosition = new Vector3(-75, -275 + (sk.cast_time / EventsTimeline.instance.total_time) * 550);
			timeline_preview.alpha = 0.5f;
		}
		else
		{
			timeline_preview.gameObject.SetActive(false);
		}
	}




}


