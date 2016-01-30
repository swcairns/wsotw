using UnityEngine;
using System.Collections;

public class Task {

	public enum TaskStatus {INCOMPLETE, SUCCEEDED, FAILED};
	public int priority;
	public int startDay;
	public int frequency;
	public string name;
	public string description;
	public TaskStatus status;

	public Task(string name, int startDay, int frequency, int priority, string description = "") {
		this.name = name;
		this.startDay = startDay;
		this.frequency = frequency;
		this.priority = priority;
		this.description = description;
		this.status = TaskStatus.INCOMPLETE;
	}

	public void Succeeded() {
		status = TaskStatus.SUCCEEDED;
	}

	public void Failed() {
		status = TaskStatus.FAILED;
	}

	//TODO: Trigger event when the status changes. That will let us update Rituals and Days
}
