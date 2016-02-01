using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Day {

	public List<Ritual> rituals;
	public int dayNumber;
	public string status;

    public string Status
    {
        get
        {
            int completedCount = rituals.FindAll(item => item.Status == "succeeded").Count;
            if (completedCount == rituals.Count)
            {
                return "succeeded";
            }
            else if (completedCount > 0 && completedCount < rituals.Count)
            {
                return "in_progress";
            }
            else
            {
                return "not_started";
            }
        }
    }

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
	public bool PerformTask(string name, bool succeeded) {
		// First find which ritual this task is for.
		Ritual ritual = FindRitualByTask(name);

		// If this task is a narrative task, then return true right away.
		if (ritual.ritualType == "narrative") {
			return true;
		}

		// Now find out if there are other rituals that are in progress that we haven't finished yet.
		foreach (Ritual r in rituals) {
			if (r.name != ritual.name) {
				if (r.Status == "in_progress") {
					Debug.Log("A different ritual is still in progress! You dun goofed. That ritual was: " + ritual.name);
					//EventManager.TriggerEvent("strike");
					return false;
				}
			}
		}

		// You don't have another ritual in progress, so make sure you're doing this ritual in the right order.
		// Make sure we're not comparing against null priorities.
		foreach (Ritual r in rituals) {
			if (r.priority != null && r.priority < ritual.priority && r.Status != "succeeded") {
				Debug.Log("You didn't perform the rituals in the correct order");
				return false;
			}			
		}

		//Forward this on to the Ritual to do some checking there.
		return ritual.PerformTask(name, succeeded);
	}

	public int TaskCount() {
		int tasks = 0;

		foreach (Ritual ritual in rituals) {
			tasks += ritual.tasks.Count;
		}

		return tasks;
	}
}
