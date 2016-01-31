using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Ritual {

	public string name;
	public int? priority;
	public string ritualType;
	public string status;
	public List<Task> tasks;

	public Ritual(string name, int? priority, string ritualType) {
		this.name = name;
		this.priority = priority;
		this.ritualType = ritualType;
		this.status = "incomplete";

		tasks = new List<Task>();
	}

	public Task FindTaskByName(string name) {
		return tasks.Find(item => item.name == name);
	}

	// Returns true if you successfully perform the task. Returns false otherwise.
	public bool PerformTask(string name, bool succeeded) {
		// Find the task by the name passed in.
		Task task = FindTaskByName(name);

		// Check if there are any tasks that have not been completed that are higher priority.
		foreach(Task t in tasks) {
			if (t.priority != null) {
				if (t.priority < task.priority && t.status != "succeeded") {
					Debug.Log("You didn't perform the tasks in the right order! STRIKE.");
					EventManager.TriggerEvent("strike");
					return false;
				}
			}
		}

		// Looking good! Now let's actually perform the task.
		bool result = task.Perform(succeeded);

		// Update the status of this ritual so we know if it's succeeded or just incomplete.
		UpdateStatus();

		return result;
	}

	public void UpdateStatus() {
		// If all tasks have succeeded, then this ritual is complete.
		bool succeeded = true;
		foreach (Task t in tasks) {
			if (t.status != "succeeded") {
				succeeded = false;
			}
		}
		if (succeeded) {
			status = "succeeded";
			return;
		}

		// If only some tasks have succeeded, then this ritual is in progress.
		// Since we're here checking the ritual, then at least one task must have been performed.
		status = "in_progress"; 
	}
}
