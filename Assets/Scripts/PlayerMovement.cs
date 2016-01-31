using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    //public const int LAYER_INTERACTABLE = 

	NavMeshAgent agent;



	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
         {
             RaycastHit hit;
             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             if (Physics.Raycast(ray, out hit))
             {
				agent.destination = hit.point;
             }
         }	
	}
}