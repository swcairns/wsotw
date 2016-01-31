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
	public int numberOfDays = 10;

	private UnityDataConnector udc;

	void OnEnable() {
		EventManager.StartListening("strike", HandleStrike);
	}

	void OnDisable() {
		EventManager.StopListening("strike", HandleStrike);
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
		EventManager.TriggerEvent("strike");
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
			EventManager.TriggerEvent("game_over");
		}
	}

	public void DoSomethingWithTheData(JsonData[] ssObjects)
	{
		// Make an entry for each day.
		for(int i = 0; i < numberOfDays; i++) {
			days.Add(new Day(i+1));
		}		

		// Each row is a task.
		for (int i = 0; i < ssObjects.Length; i++) 
		{	
			JsonData data = ssObjects[i];
			if (!data.Keys.Contains("day_number")) 
			{
				continue;
			}
			int startDay = int.Parse(data["day_number"].ToString());

			if (data.Keys.Contains("ritual_frequency")) 
			{
				int frequency = int.Parse(data["ritual_frequency"].ToString());
				for (int dayNumber = startDay; dayNumber <= numberOfDays; dayNumber += frequency) 
				{
					AddTask(dayNumber, data);
				}	
			}
			else {
				AddTask(startDay, data);
			}
		}	
	}

	void AddTask(int dayNumber, JsonData data) {
		Day day = days.Find(item => item.dayNumber == dayNumber);
		//Now check if this ritual has already been added to the day, or if we need to make a new one.
		string ritualName = data["ritual_name"].ToString();
		Ritual ritual = day.rituals.Find(item => item.name == ritualName);
		if (ritual == null) {
			int? priority = null;
			if (data.Keys.Contains("ritual_priority")) {
				priority = int.Parse(data["ritual_priority"].ToString());
			}

			string ritualType = data["ritual_type"].ToString();
			ritual = new Ritual(ritualName, priority, ritualType);
			day.rituals.Add(ritual);
		}

		// Make a new task.
		string taskName = data["task_name"].ToString();
		int? taskPriority = null;
		if (data.Keys.Contains("task_priority")) {
			taskPriority = int.Parse(data["task_priority"].ToString());
		}
		string taskDescription = data["task_description"].ToString();
		Task task = new Task(taskName, taskPriority, taskDescription);

		// Add that task to the ritual
		ritual.tasks.Add(task);
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
