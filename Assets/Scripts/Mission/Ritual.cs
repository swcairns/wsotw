using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Ritual {

	public string name;
	public int priority;
	public string status;
	public List<Task> tasks;

	public Ritual(string name, List<Task> tasks) {
		this.name = name;
		this.tasks = tasks;
		this.status = "incomplete";
	}

	public Task FindTaskByName(string name) {
		return tasks.Find(item => item.name == name);
	}

	// Returns true if you successfully perform the task. Returns false otherwise.
	public bool PerformTask(string name) {
		// Find the task by the name passed in.
		Task task = FindTaskByName(name);

		// Check if there are any tasks that have not been completed that are higher priority.
		foreach(Task t in tasks) {
			if (t.priority < task.priority && t.status != "succeeded") {
				EventManager.TriggerEvent("strike");
				return false;
			}
		}

		// Looking good! Now let's actually perform the task.
		task.Perform();

		// Update the status of this ritual so we know if it's succeeded or just incomplete.
		UpdateStatus();

		return true;
	}

	public void UpdateStatus() {
		foreach (Task t in tasks) {
			if (t.status != "succeeded") {
				status = "incomplete";
				return;
			}
		}

		status = "succeeded";
		return;
	}
}
