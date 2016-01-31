using UnityEngine;
using System.Collections;

public class PlayMovie : MonoBehaviour {

	public bool PlaySound = false;

	void Start()
	{
		MovieTexture movie = (MovieTexture)GetComponent<Renderer> ().material.mainTexture;
		movie.loop = true;
		movie.Play ();

		if (PlaySound)
		{
			AudioSource a = gameObject.AddComponent<AudioSource> ();
			a.clip = movie.audioClip;
			a.spatialize = false;
			a.loop = true;
			a.Play ();
		}
	}
}
