using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour {

    // Variables
    float brightest = 8f;
    float dimmest = 0f;

    public Material mat;

    float dur = 2f;
    float speed = 10f;

    private bool _isPulsing;

    public bool IsPulsing
    {
        get { return _isPulsing; }
        set
        {
            _isPulsing = value;
            if (value) StartCoroutine("Pulsing");
            else
            {
                StopCoroutine("Pulsing");
                SetToDim();
            }
        }
    }

    void Start()
    {
        mat = transform.GetComponent<Renderer>().material;
        this.IsPulsing = true;
    }

    // Pulse the glow!
    IEnumerator Pulsing()
    {
        Debug.Log("Pulsing!!");
        Color emissionColor = mat.GetColor("_EmissionColor");
        while(true)
        {
            float lerp = Mathf.PingPong(Time.time * speed, brightest - dimmest) + dimmest;
            Debug.Log(lerp);
            //Color emissionColor = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a) * Mathf.LinearToGammaSpace(lerp);
            mat.SetColor("_EmissionColor", emissionColor * Mathf.LinearToGammaSpace(lerp));
            yield return null;
        }
    }

    // However bright it is, dim the brightness down to the dimmest value.
    void SetToDim ()
    {
        Color emissionColor = mat.GetColor("_EmissionColor");
        mat.SetColor("_EmissionColor", emissionColor * Mathf.LinearToGammaSpace(dimmest));
    }

    /*
    IEnumerator FadeInTrack(AudioSource track, float vol = 1)
    {
        //Debug.Log("track volume: " + track.volume);
        float min = track.volume;
        float timeToChange = 0;
        while (track.volume < vol)
        {
            timeToChange += Time.deltaTime * fadeTime;
            track.volume = Mathf.Lerp(min, vol, timeToChange);
            // Debug.Log("track volume after lerp: " + track.volume);
            yield return null;
        }
    }
    */

    // Update is called once per frame
    void Update () {
	
	}
}
