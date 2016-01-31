using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InterfaceManager : MonoBehaviour {

    public static InterfaceManager instance { get; private set; }

    // Scene references
    public Transform Canvas;
    public Transform Panel;
    public Singleton<NarrativeManager> narrativeManager;

    // Personal references
    public List<Task> taskList = new List<Task>();
    //public Text[] bulletArray = new Text[20];
    public List<Text> bulletList = new List<Text>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void OnEnable()
    {
        EventManager.StartListening("task_completed", HandleTaskCompleted);
    }

    void OnDisable()
    {
        EventManager.StopListening("task_completed", HandleTaskCompleted);
    }

    void Start()
    {
        // Get the tasks for today.
        CreateTaskList();

        // Create a list of all the bullet points.
        //bulletArray = Canvas.Find("UI_HUD").Find("Task Bullets").transform.GetComponentsInChildren<Text>();
        foreach (Text child in Canvas.Find("UI_HUD").Find("Task Bullets").transform.GetComponentsInChildren<Text>())
        {
            bulletList.Add(child);
        }

    }

    // Create a new list for the day.
    private void CreateTaskList()
    {
        taskList.Clear();
        //taskList = NarrativeManager.Instance.GetTodaysTasks();
    }

    // Thing to turn on bullet points for the day.
    public void SetUpBulletPoints()
    {
        for (int x = 0; x < bulletList.Count ; x++)
        {
            Text child = bulletList[x] as Text;
            child.enabled = true;
        }
    }

    // Thing to turn off all the bullet points.
    public void TurnOffBulletPoints()
    {
        for (int x = 0; x < bulletList.Count; x++)
        {
            Text child = bulletList[x] as Text;
            child.enabled = false;
        }
    }

    public void ResetBulletPoints()
    {
        TurnOffBulletPoints();
        SetUpBulletPoints();
    }

    // Thing to listen for Ritual Completion events.
    void HandleTaskCompleted()
    {

    }

    // Thing to turn on tasks on the ToDo List.
    public void TurnOnTask(int numTask)
    {
        //taskList[numTask]
    }


}
