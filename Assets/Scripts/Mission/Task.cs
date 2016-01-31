﻿using UnityEngine;
using System.Collections;

public class Task {

	public string status;
	public int? priority;
	public string name;
	public string description;
	public string phraseToType;
	public string loopSFX;
	public string successSFX;
    public string failSFX;

	public Task(string name, int? priority, string description = "", string phraseToType = "", string loopSFX = "", string successSFX = "") {
		this.name = name;
		this.priority = priority;
		this.description = description;
        this.phraseToType = phraseToType;
		this.status = "incomplete";
		this.loopSFX = loopSFX;
		this.successSFX = successSFX;
        this.failSFX = failSFX;
	}

	public bool Perform(bool succeeded) {
		if (succeeded) {
			this.status = "succeeded";
			EventManager.TriggerEvent("task_succeeded");
		}
		else {
			this.status = "failed";
			EventManager.TriggerEvent("task_failed");
		}

        if (name == "bedtime") {
        	NarrativeManager.Instance.currentDay++;
        	EventManager.TriggerEvent("new_day");
        }

		return succeeded;
	}
}
