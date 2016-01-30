using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Day {

	public List<Ritual> rituals;
	public int dayNumber;
	public string status;

	public Day(List<Ritual> rituals) {
		this.rituals = rituals;
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

	public void PerformTask(string name) {
		// First find which ritual this task is for.
		Ritual ritual = FindRitualByTask(name);

		// Now find out if there are other rituals that are in progress that we haven't finished yet.
		foreach (Ritual r in rituals) {
			if (r.name != ritual.name) {
				if (r.status == "in_progress") {
					// A different ritual is still in progress! You dun goofed.
					EventManager.TriggerEvent("strike");
					return;
				}
			}
		}

		// You don't have another ritual in progress, so make sure you're doing this ritual in the right order.
		foreach (Ritual r in rituals) {
			if (r.priority < ritual.priority && r.status != "succeeded") {
				EventManager.TriggerEvent("strike");
				return;
			}			
		}

		//Forward this on to the Ritual to do some checking there.
		if (ritual.PerformTask(name)) {
			// If we're able to perform this task, then this ritual is now in progress.
			ritual.status = "in_progress";
		};
	}
}
