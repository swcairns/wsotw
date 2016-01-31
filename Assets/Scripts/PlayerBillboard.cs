using UnityEngine;
using System.Collections;

public class PlayerBillboard : MonoBehaviour {

    public Camera dasCamera;
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(dasCamera.transform);
	}
}
