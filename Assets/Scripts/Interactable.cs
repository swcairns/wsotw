﻿using UnityEngine;
using System.Collections;

public abstract class Interactable : MonoBehaviour {

    //private const int LAYER_PLAYER = 8;

    public string Name;

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
                IsInUse = true;
                Use();
            }
        }
    }

    public void EndUseInteractable()
    {
        if(IsInUse)
        {
            IsInUse = false;
            EndUse();

            if(IsDone)
            {
                //NarrativeManager.Instance.TaskSuccess(Name);
            }
            else
            {
                //NarrativeManager.Instance.TaskFailed(Name);
            }
        }
    }

    public void DoneInteractable()
    {
        if(!IsDone)
        {
            //TODO Check with manager if the ritual was successful
            //if(NarrativeManager.Instance.CheckTask(Name))
            //{
            IsDone = true;
            Done();
            EndUseInteractable();
            DeactivateTrigger();
            PlayerInteractCheck.Instance.ActiveInteractableDone();
            GetComponent<Collider>().enabled = false;
            enabled = false;
            //}
            /*else
            {
                NarrativeManager.Instance.TaskFailed(Name);   
            }*/
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

    void Awake()
    {
        DistanceToPlayer = -1.0f;
    }
}
