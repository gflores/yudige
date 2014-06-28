using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplainEnemyTookDamage : SequentialEventValidate {
	public Element player_element;
	public Element enemy_element;
	public int player_power;
	public int enemy_power;
	protected override void MakeStart()
	{
	}
	protected override IEnumerable ActionList()
	{
		OnStartEvent();
		//Start
		
		yield return StartCoroutine(_0());		
		yield return StartCoroutine(_1());		
		yield return StartCoroutine(_2());		
		yield return StartCoroutine(_3());		
		//End
		OnEndEvent();
		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0f));
		PopupSmall.instance.Hide();
	}
	IEnumerator _0(){
		CameraManager.instance.SetColorToFadePlane(new Color(0, 0, 0, 0.3f));
		ElementRelation element_relation = ElementManager.instance.GetRelationBetween(player_element, enemy_element);
		if (player_element == Element.DARK)
		{
			PopupSmall.instance.text_label.text = "Your attack was of type Void. The concept of nothingness\n" +
				"Void is the only thing which can negate absolute perfection, which is a conceptual reality.\n" +
					"But the void get crushed by fire and earth, which are the two elements which manifest concrete reality";
		}
		else if (player_element == Element.LIGHT)
		{
			PopupSmall.instance.text_label.text = "Your attack was of type Absolute. The conceptual ultimate reality\n" +
				"Conceptual perfection is always superior to concrete manifestation of reality like fire and earth .\n" +
					"But even something as powerful as conceptual perfection can be suppressed by its denial.";
		}
		else if (player_element == Element.FIRE)
		{
			PopupSmall.instance.text_label.text = "Your attack was of type Fire, one of the two natural elements in this world." +
				"Only a concrete manifestation of reality like fire can crush the concept of non-reality." +
					"But because fire is bound to reality, it will never be able transcend absolute perfection." +
					"Fire is also naturally weak against Rock";
		}
		else if (player_element == Element.ROCK)
		{
			PopupSmall.instance.text_label.text = "Your attack was of type Rock. One of the two natural elements in this world" +
				"Only a concrete manifestation of reality like earth can crush the concept of non-reality." +
					"But because earth is bound to reality, it will never be able transcend absolute perfection." +
					"Rock is also naturally strong against Fire";
		}
		PopupSmall.instance.Show(110,-41, 527, 300);
		yield return null;
	}
	IEnumerator _1(){
		if (player_element == Element.DARK)
		{
			PopupSmall.instance.text_label.text = "To recap:\n" +
				"Void is strong against Absolute\n" +
					"Void is weak against Fire and Rock";
		}
		else if (player_element == Element.LIGHT)
		{
			PopupSmall.instance.text_label.text = "To recap:\n" +
				"Absolute is strong against Fire and Rock\n" +
					"Absolute is weak against Void";
		}
		else if (player_element == Element.FIRE)
		{
			PopupSmall.instance.text_label.text = "To recap:\n" +
				"Fire is strong against Void\n" +
					"Fire is weak against Absolute and Rock";
		}
		else if (player_element == Element.ROCK)
		{
			PopupSmall.instance.text_label.text = "To recap:\n" +
				"Rock is strong against Void and Fire\n" +
					"Rock is weak against Absolute";
		}
		yield return null;
	}

	IEnumerator _2(){
		ElementRelation element_relation = ElementManager.instance.GetRelationBetween(player_element, enemy_element);
		if (element_relation == ElementRelation.NORMAL)
		{
			PopupSmall.instance.text_label.text = "When the attacking and defending types are the same, the damages are reduced by the value of the defending affinity\n";
		}
		else if (element_relation == ElementRelation.STRONG)
		{
			PopupSmall.instance.text_label.text = "When the attacking type is strong against the defending type, the damages are doubled and the defending affinity is taken as extra damages";
		}
		else if (element_relation == ElementRelation.WEAK)
		{
			PopupSmall.instance.text_label.text = "When the attacking type is weak against the defending type, the damages are halved and then reduced by the value of the defending affinity";
		}
		yield return null;
	}
	IEnumerator _3(){
		ElementRelation element_relation = ElementManager.instance.GetRelationBetween(player_element, enemy_element);
		string player_element_string = ElementManager.instance.ElementToString(player_element);
		string enemy_element_string = ElementManager.instance.ElementToString(enemy_element);
		bool is_burst = (EventsTimeline.instance.event_to_schedule.event_type == TimelineEventType.PLAYER_BURST_ATTACK);
		if (is_burst)
			player_power *= 2;
		if (element_relation == ElementRelation.NORMAL)
		{
			PopupSmall.instance.text_label.text = "You attacked using the element " + player_element_string + " with a power of " + player_power+"\n" +
				"against the enemy which defended with the element " + enemy_element_string + " with an affinity of: " + enemy_power + "\n" +
					"So the enemy took: "+player_power +" - "+enemy_power + " = " + Mathf.Max(0,player_power -enemy_power);

		}
		else if (element_relation == ElementRelation.STRONG)
		{
			PopupSmall.instance.text_label.text = "You have the advantage using the element " + player_element_string + " with a power of " + player_power+"\n" +
				"against the enemy which defended with the element " + enemy_element_string + " with an affinity of: " + enemy_power + "\n"+
			"So the enemy took: ("+player_power +" * 2) + " + enemy_power + " = " + Mathf.Max(0,player_power * 2 + enemy_power);
		}
		else if (element_relation == ElementRelation.WEAK)
		{
			PopupSmall.instance.text_label.text = "You have the disadvantage using the element " + player_element_string + " with a power of " + player_power+"\n" +
				"against the enemy which defended with the element " + enemy_element_string + " with an affinity of: " + enemy_power + "\n"+
					"So the enemy took: ("+player_power +" / 2) - " + enemy_power + " = " + Mathf.Max(0,player_power / 2 - enemy_power);
		}
		yield return null;
	}
}
