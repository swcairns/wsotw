using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NarrativeManager : Singleton<NarrativeManager> {
	protected NarrativeManager () {}

	public List<Day> days;
	public int currentDay;

	// Use this for initialization
	void Start () {
		Setup();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetTaskStatus(string name, Task.TaskStatus status) {
		foreach (Ritual ritual in days[currentDay -1].rituals) {
			ritual.SetTaskStatus(name, status);
		}
	}

	void Setup() {

	// DAY 1
		Task task1 = new Task("task1", 1, 1, 1);
		Task task2 = new Task("task2", 1, 1, 1);
		Task task3 = new Task("task3", 1, 1, 1);
		List<Task> tasks = new List<Task> {task1, task2, task3};
		Ritual ritual1 = new Ritual("ritual1", tasks);

		task1 = new Task("task1", 1, 1, 1);
		task2 = new Task("task2", 1, 1, 1);
		task3 = new Task("task3", 1, 1, 1);
		tasks = new List<Task> {task1, task2, task3};
		Ritual ritual2 = new Ritual("ritual2", tasks);

		task1 = new Task("task1", 1, 1, 1);
		task2 = new Task("task2", 1, 1, 1);
		task3 = new Task("task3", 1, 1, 1);
		tasks = new List<Task> {task1, task2, task3};
		Ritual ritual3 = new Ritual("ritual3", tasks);

		List<Ritual> rituals = new List<Ritual> {ritual1, ritual2, ritual3};

		Day day = new Day(rituals);
		days.Add(day);


	// DAY 2
		task1 = new Task("task1", 1, 1, 1);
		task2 = new Task("task2", 1, 1, 1);
		task3 = new Task("task3", 1, 1, 1);
		tasks = new List<Task> {task1, task2, task3};
		ritual1 = new Ritual("ritual1", tasks);

		task1 = new Task("task1", 1, 1, 1);
		task2 = new Task("task2", 1, 1, 1);
		task3 = new Task("task3", 1, 1, 1);
		tasks = new List<Task> {task1, task2, task3};
		ritual2 = new Ritual("ritual2", tasks);

		task1 = new Task("task1", 1, 1, 1);
		task2 = new Task("task2", 1, 1, 1);
		task3 = new Task("task3", 1, 1, 1);
		tasks = new List<Task> {task1, task2, task3};
		ritual3 = new Ritual("ritual3", tasks);

		rituals = new List<Ritual> {ritual1, ritual2, ritual3};

		day = new Day(rituals);
		days.Add(day);

	}
}
