using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Day {

	public enum DayStatus {INCOMPLETE, SUCCEEDED, FAILED};

	public List<Ritual> rituals;
	public int dayNumber;
	public DayStatus status;

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

	void UpdateStatus() {
		foreach (Ritual ritual in rituals) {
			if (ritual.status == Ritual.RitualStatus.FAILED) {
				status = DayStatus.FAILED;
				return;
			}
			if (ritual.status == Ritual.RitualStatus.INCOMPLETE) {
				status = DayStatus.INCOMPLETE;
				return;
			}
		}

		status = DayStatus.SUCCEEDED;
	}
}
