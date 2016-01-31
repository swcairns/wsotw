using UnityEngine;
using System.Collections;

public class ObjectClicker : Interactable {

    private const float USE_DISTANCE = 1.0f;

    private const int LAYER_TASKOBJECT = 10;
    private const int LAYERMASK = (1 << LAYER_TASKOBJECT);

    public Collider targetTaskObject;

    //[System.NonSerialized]
    public AudioClip UseSound;
    //[System.NonSerialized]
    public AudioClip FailSound;

    private AudioSource source;

    private bool useQueued = false;

    protected override void Activate()
    {
        // Player enters trigger
    }

    protected override void Deactivate()
    {
        // Player leaves trigger
    }

    protected override void SetNearest()
    {
    }

    protected override void UnsetNearest()
    {
    }

    protected override void Use()
    {
        useQueued = true;
    }

    protected override void EndUse()
    {
    }

    protected override void Done()
    {
    }

    void Update()
    {
        if(!IsDone)
        {
            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo = new RaycastHit();

                if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000.0f, LAYERMASK))
                {
                    Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction, Color.white);

                    if(hitInfo.collider == targetTaskObject)
                    {
                        UseInteractable();
                        EndUseInteractable();
                    }
                }
            }

            if(useQueued)
            {
                if(DistanceToPlayer <= USE_DISTANCE && DistanceToPlayer >= 0.0f)
                {
                    Click();
                }
            }
        }
    }

    void Click()
    {
        useQueued = false;

        // Check with NarrativeManager if this Interactable is Done
        if(NarrativeManager.Instance.PerformTask(Name, true))
        {
            if(UseSound != null)
            {
                source.PlayOneShot(UseSound);
            }

            DoneInteractable();
        }
        else
        {
            if(FailSound != null)
            {
                source.PlayOneShot(FailSound);
            }

            EndUseInteractable();
        }
    }

    public void ResetUseQueued()
    {
        useQueued = false;
    }

    protected override void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.loop = false;
        source.playOnAwake = false;
        source.spatialize = false;

        base.Awake();
    }

    protected override void Initialize()
    {
        Task t = NarrativeManager.Instance.FindTaskByNameToday(Name);

        if(t != null)
        {
            Description = t.description;

            if(t.successSFX != "")
            {
                UseSound = (AudioClip)Resources.Load("Sfx/Ship/" + t.successSFX);
            }
            if(t.failSFX != "")
            {
                FailSound = (AudioClip)Resources.Load("Sfx/Ship/" + t.failSFX);
            }
            gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Task " + Name + " does not exist!", gameObject);
            gameObject.SetActive(false);
        }
    }

    public override void Reset()
    {
        base.Reset();

        useQueued = false;
    }
}