using UnityEngine;
using LitJson;
using System.Collections;
using System;
using System.Collections.Generic;

public class NarrativeManager : Singleton<NarrativeManager> {
	protected NarrativeManager () {}

	public List<Day> days;
	public int currentDay;
	public int strikes;
	public int maxStrikes;

	private UnityDataConnector udc;

	void OnEnable() {
		EventManager.Instance.StartListening("strike", HandleStrike);
	}

	void OnDisable() {
		EventManager.Instance.StopListening("strike", HandleStrike);
	}

	// Use this for initialization
	void Start () {
		udc = GetComponent<UnityDataConnector>();
		udc.Connect();

		currentDay = 1;
		strikes = 0;

		days = new List<Day>();
	}

	public void TaskSuccess(string name) {
		Day day = days.Find(item => item.dayNumber == currentDay);
		Task task = day.FindTaskByName(name);

		// If this task has a priority of null
		if (task.priority == null) {
			Ritual ritual = day.FindRitualByTask(name);
			int? maxPriority = 0;
			foreach (Task t in ritual.tasks) {
				if (t.priority > maxPriority) {
					maxPriority = t.priority;
				}
			}

			task.priority = maxPriority + 1;
		}

		day.PerformTask(name);
	}

	public void TaskFailed(string name) {
		EventManager.Instance.TriggerEvent("strike");
	}

	// The first time you do your personal tasks, we need to remember the order that you performed them in.
	public void SetTaskPriority(string name, int priority) {
		Day day = days.Find(item => item.dayNumber == currentDay);
		Task task = day.FindTaskByName(name);
		task.priority = priority;
	}

	void HandleStrike() {
		strikes++;

		if (strikes >= maxStrikes) {
			Debug.Log("GAME OVER");
			EventManager.Instance.TriggerEvent("game_over");
		}
	}

	public void DoSomethingWithTheData(JsonData[] ssObjects)
	{		
		for (int i = 0; i < ssObjects.Length; i++) 
		{	
			// Check if this day already exists. If not, make a new one.
			int dayNumber = int.Parse(ssObjects[i]["day_number"].ToString());
			Day day = days.Find(item => item.dayNumber == dayNumber);
			if (day == null) {
				day = new Day(dayNumber);
				days.Add(day);
			}

			// Now check if this ritual has already been added to the day, or if we need to make a new one.
			string ritualName = ssObjects[i]["ritual_name"].ToString();
			Ritual ritual = day.rituals.Find(item => item.name == ritualName);
			if (ritual == null) {
				string name = ssObjects[i]["ritual_name"].ToString();
				int? priority = null;
				if (ssObjects[i].Keys.Contains("ritual_priority")) {
					priority = int.Parse(ssObjects[i]["ritual_priority"].ToString());
				}

				string ritualType = ssObjects[i]["ritual_type"].ToString();
				ritual = new Ritual(name, priority, ritualType);
				day.rituals.Add(ritual);
			}

			// Make a new task.
			string taskName = ssObjects[i]["task_name"].ToString();
			int? taskPriority = null;
			if (ssObjects[i].Keys.Contains("task_priority")) {
				taskPriority = int.Parse(ssObjects[i]["task_priority"].ToString());
			}
			string taskDescription = ssObjects[i]["task_description"].ToString();
			Task task = new Task(taskName, taskPriority, taskDescription);

			// Add that task to the ritual
			ritual.tasks.Add(task);
		}	
	}

	Day Today() {
		return days.Find(item => item.dayNumber == currentDay);
	}

	List<Task> GetTodaysTasks() {
		List<Task> taskList = new List<Task>();

		foreach (Ritual ritual in Today().rituals) {
			foreach(Task task in ritual.tasks) {
				taskList.Add(task);
			}
		}

		return taskList;
	}

	void HandleTest(string value) {
		Debug.Log("Heard an event! " + value);
	}
}
