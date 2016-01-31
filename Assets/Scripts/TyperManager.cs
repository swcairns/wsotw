using UnityEngine;
using System.Collections;

public class TyperManager : MonoBehaviour {

    public static TyperManager Instance { get; private set; }

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
	
	void Update()
    {
	    
	}
}
