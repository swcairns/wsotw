using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    private float fixedXPosition;
    private float fixedYPosition;

    void Start()
    {
        fixedXPosition = transform.position.x;
        fixedYPosition = transform.position.y;
    }

	void LateUpdate()
    {
        Vector3 newPosition = new Vector3(fixedXPosition, fixedYPosition, target.position.z);
        transform.position = newPosition;
	}
}
