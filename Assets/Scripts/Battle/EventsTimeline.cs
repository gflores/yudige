using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TimelineEvent{
	public float time_remaining;
	public string name;
	public IEnumerator on_complete_routine;
}

public class EventsTimeline : MonoBehaviour {
	public static EventsTimeline instance{get; set;}
	public float total_time = 20f;
	public TimelineEvent event_to_schedule {get; set;}
	public List<TimelineEvent> scheduled_events {get; set;}

	void Awake()
	{
		instance = this;
		scheduled_events = new List<TimelineEvent>();
	}
	public void Schedule(TimelineEvent n_event_to_schedule)
	{
		event_to_schedule = n_event_to_schedule;
		LaunchSchedule();
	}
	public void LaunchSchedule()
	{
		scheduled_events.Add(event_to_schedule);
		StartCoroutine(Coroutine_EventProgression(event_to_schedule));
	}

	IEnumerator Coroutine_EventProgression(TimelineEvent timeline_event)
	{
		while (timeline_event.time_remaining > 0)
		{
			timeline_event.time_remaining -= Time.deltaTime;
			yield return new WaitForSeconds(0.001f);
		}
		Debug.LogWarning("timeline_event " + timeline_event.name + " FINISHED WAITING !");
		StartCoroutine(timeline_event.on_complete_routine);
	}
}