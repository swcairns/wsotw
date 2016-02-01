using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Ritual {

	public string name;
	public int? priority;
	public string ritualType;
	public List<Task> tasks;

    public string Status {
        get {
            int completedCount = tasks.FindAll(item => item.status == "succeeded").Count;
            Debug.Log("Ritual Status Check. " + name + " " + completedCount + "/" + tasks.Count + " tasks completed");
            if (completedCount == tasks.Count)
            {
                return "succeeded";
            }
            else if (completedCount > 0 && completedCount < tasks.Count)
            {
                return "in_progress";
            }
            else
            {
                return "not_started";
            }
        }
    }

	public Ritual(string name, int? priority, string ritualType) {
		this.name = name;
		this.priority = priority;
		this.ritualType = ritualType;
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
					//EventManager.TriggerEvent("strike");
					return false;
				}
			}
		}

        // Looking good! Now let's actually perform the task.
        return task.Perform(succeeded);
	}
}
