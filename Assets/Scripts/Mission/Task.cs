using UnityEngine;
using System.Collections;

public class Task {

	public string status;
	public int? priority;
	public string name;
	public string description;
	public string phaseToType;
	public string loopSFX;
	public string successSFX;

	public Task(string name, int? priority, string description = "", string phraseToType = "", string loopSFX = "", string successSFX = "") {
		this.name = name;
		this.priority = priority;
		this.description = description;
		this.status = "incomplete";
		this.loopSFX = loopSFX;
		this.successSFX = successSFX;
	}

	public void Perform() {
		this.status = "success";
		EventManager.TriggerEvent("task_completed");
	}

	public void Succeeded() {
		status = "succeeded";
	}

	public void Failed() {
		status = "failed";
	}

}
