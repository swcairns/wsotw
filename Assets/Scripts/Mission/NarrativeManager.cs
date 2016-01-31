﻿using UnityEngine;
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

	public bool PerformTask(string name) {
		Debug.Log("Narrative Manager: Performing Task: " + name);
		Day day = Today();
		Task task = day.FindTaskByName(name);

		// If this task has a null priority, it's either a personal task
		// Or a narrative task.
		if (task.priority == null) {
			Ritual ritual = day.FindRitualByTask(name);

			//If this is a personal ritual, then we need to assign it a priority.
			if (ritual.ritualType == "personal") {
				int? maxPriority = 0;
				foreach (Task t in ritual.tasks) {
					if (t.priority > maxPriority) {
						maxPriority = t.priority;
					}
				}

				task.priority = maxPriority + 1;
			}

			// If this is a narrative task, then we don't need to do anything yet.
		}

		// Perform the task. If we're successful, return true.
		if (day.PerformTask(name)) {
			Debug.Log("Narrative Manager: Task Succeeded!!");
			return true;
		}
		else {
			strikes++;
			Debug.Log("Narrative Manager: Task Failed :( You have " + strikes + " strikes");
			return false;
		};
	}

	// The first time you do your personal tasks, we need to remember the order that you performed them in.
	public void SetTaskPriority(string name, int priority) {
		Day day = days.Find(item => item.dayNumber == currentDay);
		Task task = day.FindTaskByName(name);
		task.priority = priority;
	}

	void HandleStrike() {
		//strikes++;

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

		string taskDescription = "";
		if (data.Keys.Contains["task_description"]) {
			taskDescription = data["task_description"].ToString();
		}
	
		string phraseToType = "";
		if (data.Keys.Contains["phrase_to_type"]) {
			taskDescription = data["phrase_to_type"].ToString();
		}

		string loopSFX = "";
		if (data.Keys.Contains["loop_sfx"]) {
			taskDescription = data["loop_sfx"].ToString();
		}

		string successSFX = "";
		if (data.Keys.Contains["success_sfx"]) {
			taskDescription = data["success_sfx"].ToString();
		}

		Task task = new Task(taskName, taskPriority, taskDescription, phraseToType, loopSFX, successSFX);

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

	Task FindTaskByName(string name) {
		foreach (Ritual ritual in Today().rituals) {
			foreach(Task task in ritual.tasks) {
				if (task.name == name) {
					return task;
				}
			}
		}
		return null;
	}

	void HandleTest(string value) {
		Debug.Log("Heard an event! " + value);
	}
}
