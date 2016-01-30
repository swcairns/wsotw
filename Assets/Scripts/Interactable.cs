using UnityEngine;
using System.Collections;

public abstract class Interactable : MonoBehaviour {

    private const int LAYER_PLAYER = 8;

    public bool IsActive { get; private set; }

    public abstract void Activate();
    public abstract void Deactivate();
    public abstract void Use();

    public virtual void Update()
    {
        if(IsActive)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Use();
            }
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LAYER_PLAYER)
        {
            IsActive = true;
            Activate();
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LAYER_PLAYER)
        {
            IsActive = false;
            Deactivate();
        }
    }
}
