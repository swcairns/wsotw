using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Ritual {

	public enum RitualStatus {INCOMPLETE, SUCCEEDED, FAILED};
	public string name;
	public string priority;
	public RitualStatus status;
	public List<Task> tasks;

	public Ritual(string name, List<Task> tasks) {
		this.name = name;
		this.tasks = tasks;
		this.status = RitualStatus.INCOMPLETE;
	}

	public Task FindTaskByName(string name) {
		return tasks.Find(item => item.name == name);
	}

	public void SetTaskStatus(string name, Task.TaskStatus status) {
		Task task = FindTaskByName(name);
		task.status = status;
	}

	public void UpdateStatus() {
		foreach (Task task in tasks) {
			if (task.status == Task.TaskStatus.FAILED) {
				status = RitualStatus.FAILED;
				return;
			}
			else if (task.status == Task.TaskStatus.INCOMPLETE) {
				status = RitualStatus.INCOMPLETE;
				return;
			}
		}

		status = RitualStatus.SUCCEEDED;
	}
}
