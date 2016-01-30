using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NarrativeManager : Singleton<NarrativeManager> {
	protected NarrativeManager () {}

	public List<Day> days;
	public int currentDay;
	public int strikes;

	void OnEnable() {
		EventManager.StartListening("strike", HandleStrike);
	}

	void OnDisable() {
		EventManager.StopListening("strike", HandleStrike);
	}

	// Use this for initialization
	void Start () {
		currentDay = 1;
		strikes = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TaskSuccess(string name) {
		Day day = days.Find(item => item.dayNumber == currentDay);
		day.PerformTask(name);
	}

	public void TaskFailed(string name) {
		EventManager.TriggerEvent("strike");
	}

	void HandleStrike() {
		strikes++;
	}
}
