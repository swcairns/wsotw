using UnityEngine;
using System.Collections;

public class Task {

	public string status;
	public int? priority;
	public string name;
	public string description;
	public string phraseToType;
	public string loopSFX;
	public string successSFX;

	public Task(string name, int? priority, string description = "", string phraseToType = "", string loopSFX = "", string successSFX = "") {
		this.name = name;
		this.priority = priority;
		this.description = description;
        this.phraseToType = phraseToType;
		this.status = "incomplete";
		this.loopSFX = loopSFX;
		this.successSFX = successSFX;
	}

	public void Perform() {
		this.status = "succeeded";
		EventManager.TriggerEvent("task_completed");
	}
}
