using UnityEngine;
using System.Collections;

public class Task {

	public string status;
	public int? priority;
	public string name;
	public string description;

	public Task(string name, int? priority, string description = "") {
		this.name = name;
		this.priority = priority;
		this.description = description;
		this.status = "incomplete";
	}

	public void Perform() {
		this.status = "success";
		EventManager.Instance.TriggerEvent("task_completed");
	}

	public void Succeeded() {
		status = "succeeded";
	}

	public void Failed() {
		status = "failed";
	}

}
