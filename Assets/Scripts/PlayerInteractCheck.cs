using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerInteractCheck : MonoBehaviour {

    private const int LAYER_INTERACTABLE = 9;

    public static PlayerInteractCheck Instance { get; private set; }

    private Vector3 floorPosition = Vector3.zero;

    public List<Interactable> interactables = new List<Interactable>();

    Interactable prevNearestInteractable = null;
    Interactable nearestInteractable = null;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

	void Start()
    {
        floorPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);
	}
	
	void Update()
    {
        floorPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);

        if(interactables.Count > 0)
        {
            Vector3 interactableFloorPosition = Vector3.zero;

            foreach(Interactable i in interactables)
            {
                interactableFloorPosition = new Vector3(i.transform.position.x, 0.0f, i.transform.position.z);
                i.DistanceToPlayer = (interactableFloorPosition - floorPosition).magnitude;
            }

            // Get nearest interactable
            nearestInteractable = interactables.OrderBy(o=>o.DistanceToPlayer).ToList()[0];

            // If player encountered a new nearest interactable
            if(nearestInteractable != prevNearestInteractable)
            {
                nearestInteractable.SetNearestInteractable();

                if(prevNearestInteractable != null)
                {
                    prevNearestInteractable.UnsetNearestInteractable();
                }
            }
        }
        else
        {
            nearestInteractable = null;

            if(nearestInteractable != prevNearestInteractable)
            {
                if(prevNearestInteractable != null)
                {
                    prevNearestInteractable.UnsetNearestInteractable();
                }
            }
        }

        if(nearestInteractable != null)
        {
            nearestInteractable.HandleUse();
        }

        prevNearestInteractable = nearestInteractable;

        #if UNITY_EDITOR

        if(nearestInteractable != null)
        {
            Debug.DrawLine(transform.position, nearestInteractable.gameObject.transform.position, Color.white);
        }

        #endif
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LAYER_INTERACTABLE)
        {
            if(other.gameObject.GetComponent<Interactable>() != null)
            {
                Interactable i = other.gameObject.GetComponent<Interactable>();

                if(!interactables.Contains(i))
                {
                    interactables.Add(i);
                }

                i.ActivateTrigger();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LAYER_INTERACTABLE)
        {
            if(other.gameObject.GetComponent<Interactable>() != null)
            {
                Interactable i = other.gameObject.GetComponent<Interactable>();

                if(interactables.Contains(i))
                {
                    i.DeactivateTrigger();
                    interactables.Remove(i);

                    if(nearestInteractable == i)
                    {
                        nearestInteractable = null;
                    }
                }
            }
        }
    }

    // Called by Interactable when it's done
    public void ActiveInteractableDone()
    {
        interactables.Remove(nearestInteractable);
        nearestInteractable = null;
    }
}
