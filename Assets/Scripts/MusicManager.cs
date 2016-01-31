using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MusicManager : MonoBehaviour {

    // AMBIENT
    public AudioSource amb_engineNoise;
    public AudioSource amb_telemetry;

    // NEUTRAL
    public AudioSource neut_personal_1st;
    public AudioSource neut_personal_2nd;
    public AudioSource neut_ship_1st;
    public AudioSource neut_ship_2nd;
    public AudioSource neut_narrative_1st;
    public AudioSource neut_narrative_2nd;

    // BAD
    public AudioSource neg_personal_1st;
    public AudioSource neg_personal_2nd;
    public AudioSource neg_ship_1st;
    public AudioSource neg_ship_2nd;
    public AudioSource neg_narrative_1st;
    public AudioSource neg_narrative_2nd;

    // List for all the audio.
    public List<AudioSource> audioList = new List<AudioSource>();

    // Current Audio Tracks
    AudioSource cur_personal;
    AudioSource cur_ship;
    AudioSource cur_narrative;

    // For changing the track to loop.
    bool personalIsFirst = true;
    bool shipIsFirst = true;
    bool narrativeIsFirst = true;

    int fadeTime = 1;


    // Use this for initialization
    void Start() {

        // Create lists of all the tracks.
        foreach (AudioSource track in transform.GetComponents<AudioSource>())
        //for (int x = 2; x < transform.GetComponents<AudioSource>().Length; x++)
        {
            audioList.Add(track);
            track.loop = true;
            track.playOnAwake = false;
            track.volume = 0;
        }
        // Remove the ambient sound tracks.
        audioList.Remove(audioList[1]);
        audioList.Remove(audioList[0]);

        // Set the "first" tracks to not loop.
        neut_personal_1st.loop = false;
        neut_ship_1st.loop = false;
        neut_narrative_1st.loop = false;

        // Set and stop the current tracks.
        ResetTracks();

    }

    void ResetTracks()
    {
        // Set the "current" track for each ritual type.
        cur_personal = neut_narrative_1st;
        cur_ship = neut_ship_1st;
        cur_narrative = neut_narrative_1st;

    }

    void HandleDayStart()
    {
        // Fade in the ambient sounds.
        StartCoroutine("FadeInTrack", amb_engineNoise);
        StartCoroutine("FadeInTrack", amb_telemetry);
    }

    void HandleRitualStart()
    {
        // Start all the tracks.
        cur_personal.Play();
        cur_ship.Play();
        cur_narrative.Play();

        // Set the triggering ritual's track's volume to 1.
        // TODO
    }

    void Update()
    {
        // Swap the first track for the second track for each ritual type when the first finishes playing.
        // Manage the Personal track.
        if (cur_personal.isPlaying && personalIsFirst)
        {
            if (cur_personal.timeSamples >= cur_personal.GetComponent<AudioClip>().samples)
            {
                cur_personal.Stop();
                cur_personal = (cur_personal == neut_personal_1st) ? neut_personal_2nd : neg_personal_2nd;
                cur_personal.Play();
            }
            personalIsFirst = false;
        }
        // Manage the Ship track.
        if (cur_ship.isPlaying && shipIsFirst)
        {
            if (cur_ship.timeSamples >= cur_ship.GetComponent<AudioClip>().samples)
            {
                cur_ship.Stop();
                cur_ship = (cur_ship == neut_ship_1st) ? neut_ship_2nd : neg_ship_2nd;
                cur_ship.Play();
            }
            shipIsFirst = false;
        }
        // Manage the Narrative track.
        if (cur_narrative.isPlaying && narrativeIsFirst)
        {
            if (cur_narrative.timeSamples >= cur_narrative.GetComponent<AudioClip>().samples)
            {
                cur_narrative.Stop();
                cur_narrative = (cur_narrative == neut_narrative_1st) ? neut_narrative_2nd : neg_narrative_2nd;
                cur_narrative.Play();
            }
            narrativeIsFirst = false;
        }

    }

    void HandleRitualStrike()
    {
        // Set the ritual's current track to its negative version.
        // Fade out the neutral track, fade in the negative track.
    }

    void HandleDayEnd ()
    {
        foreach (AudioSource track in audioList)
        {
            if (track.isPlaying) StartCoroutine("FadeOutAndEndTrack", track);
        }

        // Fade out the ambient sounds.
        StartCoroutine("FadeOutTrack", amb_engineNoise);
        StartCoroutine("FadeOutTrack", amb_telemetry);
    }
    // Fade out all tracks' volume to 0.
    // foreach (AudioSource track in audioTrack) fade to black
	
	// Fade between two tracks.
    void FadeTwoTracks (AudioSource cur, AudioSource target)
    {
        StartCoroutine("FadeOutTrack", cur);
        StartCoroutine("FadeInTrack", target);
    }

    IEnumerator FadeOutTrack(AudioSource track)
    {
        float max = track.volume;
        while (track.volume > 0)
        {
            track.volume = Mathf.Lerp(max, 0, Time.time * fadeTime);
            yield break;
        }
    }

    IEnumerator FadeOutAndEndTrack(AudioSource track)
    {
        float max = track.volume;
        while (track.volume > 0)
        {
            track.volume = Mathf.Lerp(max, 0, Time.time * fadeTime);
            yield break;
        }
        track.Stop();
    }

    IEnumerator FadeInTrack (AudioSource track)
    {
        float min = track.volume;
        while (track.volume < 1)
        {
            track.volume = Mathf.Lerp(min, 1, Time.time * fadeTime);
            yield break;
        }
    }
}
