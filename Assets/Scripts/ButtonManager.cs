using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour {

    // Variables
    int brightest = 10;
    int dimmest = 1;

    //public Material glowMat;
    //public struct HSBColor _targetColorHSB = stuff.

    void Start()
    {

    }

    // Start a pulsing glow.


    // However bright it is, dim the brightness down to the dimmest value.
    void DimDown()
    {
        StartCoroutine("Dimmer");
    }

    IEnumerator Dimmer()
    {
        //GetComponent<Renderer>().material.SetColor("_EmissionColor", HSBColor.ToColor(HSBColor.Lerp(HSBColor.FromColor(GetComponent<Renderer>().material.color), _targetColorHSB, Time.deltaTime / timeLeft)));
        yield break;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
