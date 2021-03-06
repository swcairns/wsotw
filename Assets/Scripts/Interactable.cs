﻿using UnityEngine;
using System.Collections;

public abstract class Interactable : MonoBehaviour {

    //private const int LAYER_PLAYER = 8;

    public string Name;

    [System.NonSerialized]
    public string Description;

    public bool TriggerActive { get; private set; }
    public bool IsNearest { get; private set; }
    public bool IsInUse { get; private set; }
    public bool IsDone { get; private set; }

    public float DistanceToPlayer { get; set; }

    public void ActivateTrigger()
    {
        TriggerActive = true;

        Activate();
    }

    public void DeactivateTrigger()
    {
        if(IsInUse)
        {
            EndUseInteractable();
        }

        DistanceToPlayer = -1.0f;
        TriggerActive = false;
    }

    public void SetNearestInteractable()
    {
        if(!IsNearest)
        {
            IsNearest = true;
            SetNearest();
        }
    }

    public void UnsetNearestInteractable()
    {
        if(IsInUse)
        {
            EndUseInteractable();
        }

        if(IsNearest)
        {
            IsNearest = false;
            UnsetNearest();
        }
    }

    public void UseInteractable()
    {
        if(!IsDone)
        {
            if(!IsInUse)
            {
                Ritual ritual = NarrativeManager.Instance.Today().FindRitualByTask(Name);
                EventManager.TriggerEvent("ritual_" + ritual.ritualType);
                IsInUse = true;
                Use();
            }
        }
    }

    public void EndUseInteractable(bool performTask = true)
    {
        if(IsInUse)
        {
            IsInUse = false;
            EndUse();

            if(performTask)
            {
                NarrativeManager.Instance.PerformTask(Name, false);
            }
        }
    }

    // Called by derived tasks
    public void DoneInteractable()
    {
        if(!IsDone)
        {
            if(NarrativeManager.Instance.PerformTask(Name, true))
            {
                IsDone = true;
                Done();
                EndUseInteractable(false);
                DeactivateTrigger();
                PlayerInteractCheck.Instance.ActiveInteractableDone();
                GetComponent<Collider>().enabled = false;
                enabled = false;
            }
            else
            {
                EndUseInteractable(false);
            }
        }
    }

    // Override if different way to use this Interactable
    public virtual void HandleUse()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UseInteractable();
        }
    }

    protected abstract void Use();
    protected abstract void EndUse();
    protected abstract void Activate();
    protected abstract void Deactivate();
    protected abstract void SetNearest();
    protected abstract void UnsetNearest();
    protected abstract void Done();

    protected virtual void Awake()
    {
        DistanceToPlayer = -1.0f;
        EventManager.StartListening("new_day", Initialize);
        EventManager.StartListening("new_day", Reset);

        gameObject.SetActive(false);
    }

    protected virtual void OnDestroy() {
        EventManager.StopListening("new_day", Initialize);
        EventManager.StopListening("new_day", Reset);
    }

    public virtual void Reset()
    {
    	//Debug.Log("Resetting tasks");
        TriggerActive = false;
        IsNearest = false;
        IsInUse = false;
        IsDone = false;
        DistanceToPlayer = -1.0f;
        GetComponent<Collider>().enabled = true;
    }

    protected abstract void Initialize();
}
