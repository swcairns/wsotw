using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Day {

	public List<Ritual> rituals;
	public int dayNumber;
	public string status;

	public Day(int dayNumber) {
		this.dayNumber = dayNumber;
		rituals = new List<Ritual>();
	}

	public Task FindTaskByName(string name) {
		foreach (Ritual ritual in rituals) {
			Task task = ritual.FindTaskByName(name);
			if (task != null) {
				return task;
			}
		}

		return null;
	}

	public Ritual FindRitualByTask(string name) {
		foreach (Ritual ritual in rituals) {
			Task task = ritual.FindTaskByName(name);
			if (task != null) {
				return ritual;
			}
		}

		return null;
	}

	// Returns true if this task is performed successfully
	// Returns false if:
		// - There is a different ritual in progress that we haven't finished yet.
		// - You're doing this task in the wrong order.
		// - You fail doing the task for some reason.
	public bool PerformTask(string name) {
		// First find which ritual this task is for.
		Ritual ritual = FindRitualByTask(name);

		// If this task is a narrative task, then return true right away.
		if (ritual.ritualType == "narrative") {
			return true;
		}

		// Now find out if there are other rituals that are in progress that we haven't finished yet.
		foreach (Ritual r in rituals) {
			if (r.name != ritual.name) {
				if (r.status == "in_progress") {
					Debug.Log("A different ritual is still in progress! You dun goofed.");
					EventManager.TriggerEvent("strike");
					return false;
				}
			}
		}

		// You don't have another ritual in progress, so make sure you're doing this ritual in the right order.
		// Make sure we're not comparing against null priorities.
		foreach (Ritual r in rituals) {
			if (r.priority != null && r.priority < ritual.priority && r.status != "succeeded") {
				Debug.Log("You didn't perform the rituals in the correct order");
				EventManager.TriggerEvent("strike");
				return false;
			}			
		}

		//Forward this on to the Ritual to do some checking there.
		if (ritual.PerformTask(name)) {
			// If we're able to perform this task, then this ritual is now in progress.
			Debug.Log("You performed the task successfully.");
			return true;
		}
		else {
			return false;
		}
	}

	public int TaskCount() {
		int tasks = 0;

		foreach (Ritual ritual in rituals) {
			tasks += ritual.tasks.Count;
		}

		return tasks;
	}

	public void UpdateStatus() {
		// If all tasks have succeeded, then this ritual is complete.
		bool succeeded = true;
		foreach (Ritual r in rituals) {
			if (r.status != "succeeded") {
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
