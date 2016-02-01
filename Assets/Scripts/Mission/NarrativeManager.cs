using UnityEngine;
using LitJson;
using System.Collections;
using System.Collections.Generic;

public class NarrativeManager : Singleton<NarrativeManager> {
    protected NarrativeManager() { }

    public List<Day> days;
    public int currentDay;
    public int strikes;
    public int maxStrikes;
    public int numberOfDays = 10;

    public List<Ritual> todaysRituals;

    private UnityDataConnector udc;

    void OnEnable() {
        EventManager.StartListening("strike", HandleStrike);
        EventManager.StartListening("DataLoaded", OnDataLoaded);

    }

    void OnDisable() {
        EventManager.StopListening("strike", HandleStrike);
        EventManager.StopListening("DataLoaded", OnDataLoaded);

    }

    // Use this for initialization
    void Start() {
        udc = GetComponent<UnityDataConnector>();
        udc.Connect();
        currentDay = 0;
        strikes = 0;
        days = new List<Day>();
        todaysRituals = new List<Ritual>();
    }

    void OnDataLoaded() {
        Reset();
    }

    public bool PerformTask(string name, bool succeeded) {
        Debug.Log("Narrative Manager: Performing Task: " + name);
        Day day = Today();
        Task task = day.FindTaskByName(name);

        // If this task has a null priority, it's either a personal task
        // Or a narrative task.
        if (task.priority == null) {
            Ritual ritual = day.FindRitualByTask(name);

            //If this is a personal ritual, then we need to assign it a priority.
            if (ritual.ritualType == "personal") {
                int? maxPriority = 0;
                foreach (Task t in ritual.tasks) {
                    if (t.priority != null) {
                        if (t.priority > maxPriority) {
                            maxPriority = t.priority;
                        }
                    }
                }
                SetTaskPriority(name, (int)maxPriority + 1);
                Debug.Log("Task " + task.name + " had a null priority. Now it has priority " + task.priority);
            }

            // If this is a narrative task, then we don't need to do anything yet.
        }

        // Perform the task. If we're successful, return true.
        if (day.PerformTask(name, succeeded)) {

            int completedRituals = Today().rituals.FindAll(item => item.Status == "success").Count;
            Debug.Log("Narrative Manager: Task Succeeded!!");
            Debug.Log("Status:" + completedRituals + "/" + Today().rituals.Count + " rituals have been completed");
            return true;
        }
        else {
            EventManager.TriggerEvent("strike");
            Debug.Log("Narrative Manager: Task Failed :( You have " + strikes + " strikes");
            return false;
        };
    }

    // The first time you do your personal tasks, we need to remember the order that you performed them in.
    public void SetTaskPriority(string name, int priority) {
        foreach (Day day in days)
        {
            Task task = day.FindTaskByName(name);
            task.priority = priority;
        }
    }

    void HandleStrike() {
        strikes++;

        if (strikes >= maxStrikes) {
            Debug.Log("GAME OVER");
            EventManager.TriggerEvent("game_over");
        }
    }

    public void DoSomethingWithTheData(JsonData[] ssObjects)
    {
        // Make an entry for each day.
        for (int i = 0; i < numberOfDays; i++) {
            days.Add(new Day(i + 1));
        }

        // Each row is a task.
        for (int i = 0; i < ssObjects.Length; i++)
        {
            JsonData data = ssObjects[i];
            if (!data.Keys.Contains("day_number"))
            {
                continue;
            }
            int startDay = int.Parse(data["day_number"].ToString());

            if (data.Keys.Contains("ritual_frequency"))
            {
                int frequency = int.Parse(data["ritual_frequency"].ToString());
                for (int dayNumber = startDay; dayNumber <= numberOfDays; dayNumber += frequency)
                {
                    AddTask(dayNumber, data);
                }
            }
            else {
                AddTask(startDay, data);
            }
        }
        EventManager.TriggerEvent("DataLoaded");
    }

    void AddTask(int dayNumber, JsonData data) {
        Day day = days.Find(item => item.dayNumber == dayNumber);
        //Now check if this ritual has already been added to the day, or if we need to make a new one.
        string ritualName = data["ritual_name"].ToString();
        Ritual ritual = day.rituals.Find(item => item.name == ritualName);
        if (ritual == null) {
            int? priority = null;
            if (data.Keys.Contains("ritual_priority")) {
                priority = int.Parse(data["ritual_priority"].ToString());
            }

            string ritualType = data["ritual_type"].ToString();
            ritual = new Ritual(ritualName, priority, ritualType);
            day.rituals.Add(ritual);
        }

        // Make a new task.
        string taskName = data["task_name"].ToString();
        int? taskPriority = null;
        if (data.Keys.Contains("task_priority")) {
            taskPriority = int.Parse(data["task_priority"].ToString());
        }

        string taskDescription = "";
        if (data.Keys.Contains("task_description")) {
            taskDescription = data["task_description"].ToString();
        }

        string phraseToType = "";
        if (data.Keys.Contains("phrase_to_type")) {
            phraseToType = data["phrase_to_type"].ToString();
        }

        string loopSFX = "";
        if (data.Keys.Contains("loop_sfx")) {
            loopSFX = data["loop_sfx"].ToString();
        }

        string successSFX = "";

        if (data.Keys.Contains("success_sfx")) {
            successSFX = data["success_sfx"].ToString();
        }

        string failSFX = "";
        if (data.Keys.Contains("fail_sfx")) {
            failSFX = data["fail_sfx"].ToString();
        }

        Task task = new Task(taskName, taskPriority, taskDescription, phraseToType, loopSFX, successSFX, failSFX);

        // Add that task to the ritual
        ritual.tasks.Add(task);
    }

    // This can replace my copy/paste job above eventually.
    private string formatStringData(JsonData data, string key)
    {
        string ret = "";
        if (data.Keys.Contains(key))
        {
            ret = data[key].ToString();
        }
        return ret;
    }

	public Day Today() {
		return days.Find(item => item.dayNumber == currentDay);
	}

	List<Task> GetTodaysTasks() {
		Debug.Log("Here are today's Tasks:");
		List<Task> taskList = new List<Task>();

		foreach (Ritual ritual in Today().rituals) {
			foreach(Task task in ritual.tasks) {
				Debug.Log(task.name);
				taskList.Add(task);
			}
		}
		Debug.Log("That's the plan for the day!");
		return taskList;
	}

	public void NewDay() {
		StartCoroutine(FadeOut());
		StartCoroutine(FadeIn());
	}

	void Reset() {
		currentDay++;
		GetTodaysTasks();
		EventManager.TriggerEvent("new_day");
	}

	IEnumerator FadeOut() {
		StartCoroutine(FadeTo(1.0f, 2.0f));
		yield return new WaitForSeconds(1.0f);
	}

	IEnumerator FadeIn() {
		yield return new WaitForSeconds(3.0f);
		Reset();
		StartCoroutine(FadeTo(0.0f, 2.0f));
	}

	IEnumerator FadeTo(float aValue, float aTime)
	{
		GameObject screen = GameObject.Find("FadeOutScreen");
	    float alpha = screen.GetComponent<Renderer>().material.color.a;
	    for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
	    {
	        Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
			screen.GetComponent<Renderer>().material.color = newColor;
	        yield return null;
	    }
	}

	public Task FindTaskByNameToday(string name) {
		foreach (Ritual ritual in Today().rituals) {
			foreach(Task task in ritual.tasks) {
				if (task.name == name) {
					return task;
				}
			}
		}
		return null;
	}
}
