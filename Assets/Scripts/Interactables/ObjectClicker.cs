﻿using UnityEngine;
using System.Collections;

public class ObjectClicker : Interactable {

    private const float USE_DISTANCE = 1.0f;

    private const int LAYER_TASKOBJECT = 10;
    private const int LAYERMASK = (1 << LAYER_TASKOBJECT);

    public Collider targetTaskObject;

    [System.NonSerialized]
    public AudioClip UseSound;

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
        // Play use sound
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
        if(UseSound != null)
        {
            //TODO Play use sound
            //UseSound
        }

        useQueued = false;

        Debug.Log("Clicking done!", gameObject);
        DoneInteractable();
    }

    public void ResetUseQueued()
    {
        useQueued = false;
    }

    /*void Start()
    {
        targetTaskObject = transform.parent.gameObject.GetComponent<Collider>();
    }*/

    protected override void Initialize()
    {
        Task t = NarrativeManager.Instance.FindTaskByNameToday(Name);

        if(t != null)
        {
            Description = t.description;
            //UseSound = t.loopSFX;
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