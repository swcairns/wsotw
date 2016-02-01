using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MusicManager : MonoBehaviour {

    // DAY TRANSITION MUSIC
    //public AudioSource tran_evening;
    //public AudioSource tran_morning;

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

    float fadeTime = .5f;
    float enviroVol = .5f;

    // Listen for shit.
    void OnEnable()
    {
        EventManager.StartListening("new_day", HandleDayEnd);
        //EventManager.StartListening("end_day", HandleDayStart);

        // Rituals
        EventManager.StartListening("ritual_personal", HandleRitualStart_Personal);
        EventManager.StartListening("ritual_ship", HandleRitualStart_Ship);
        EventManager.StartListening("ritual_narrative", HandleRitualStart_Narrative);

        //EventManager.StartListening("ritual_strike_personal", HandleRitualStart_Narrative);
        //EventManager.StartListening("ritual_strike_ship", HandleRitualStart_Narrative);
        //EventManager.StartListening("ritual_strike_narrative", HandleRitualStart_Narrative);
    }

    void OnDisable()
    {
        EventManager.StopListening("new_day", HandleDayEnd);
        //EventManager.StopListening("end_day", HandleDayStart);

        // Rituals
        EventManager.StopListening("ritual_personal", HandleRitualStart_Personal);
        EventManager.StopListening("ritual_ship", HandleRitualStart_Ship);
        EventManager.StopListening("ritual_narrative", HandleRitualStart_Narrative);

        //EventManager.StopListening("ritual_strike_personal", HandleRitualStart_Narrative);
        //EventManager.StopListening("ritual_strike_ship", HandleRitualStart_Narrative);
        //EventManager.StopListening("ritual_strike_narrative", HandleRitualStart_Narrative);
    }

    // Use this for initialization
    void Start() {

        // Create lists of all the tracks.
        AudioSource[] tempAudioList = transform.GetComponents<AudioSource>();
        foreach (AudioSource track in tempAudioList)
        //for (int x = 2; x < transform.GetComponents<AudioSource>().Length; x++)
        {
            //Debug.Log("track is: " + track);
            //Debug.Log("clip is: " + track.clip);
            audioList.Add(track);
            track.loop = true;
            track.playOnAwake = false;
            track.volume = 0;
        }
        // Remove the ambient and transition sound tracks.
        //audioList.Remove(audioList[3]);
        //audioList.Remove(audioList[2]);
        audioList.Remove(audioList[1]);
        audioList.Remove(audioList[0]);

        // Set the "first" tracks to not loop.
        neut_personal_1st.loop = false;
        neut_ship_1st.loop = false;
        neut_narrative_1st.loop = false;

        // Set and stop the current tracks.
        ResetTracks();

        // Play the ambient sounds.
        amb_engineNoise.Play();
        amb_telemetry.Play();

        // Fade in the ambient sounds.
        StartCoroutine(FadeInTrack(amb_engineNoise, enviroVol));
        StartCoroutine(FadeInTrack(amb_telemetry, enviroVol));

    }

    void ResetTracks()
    {
        // Set the "current" track for each ritual type.
        cur_personal = neut_personal_1st;
        cur_ship = neut_ship_1st;
        cur_narrative = neut_narrative_1st;

        //Debug.Log("current personal: " + cur_personal);
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
            //Debug.Log("cur_personal timeSamples = " + cur_personal.timeSamples + ", cur_personal clip samples = " + cur_personal.clip.samples);
            if (cur_personal.timeSamples >= cur_personal.clip.samples) //cur_personal.GetComponent<AudioClip>().samples)
            {
                Debug.Log("Swapping personal track!");
                cur_personal.Stop();
                Debug.Log("old track: " + cur_personal.clip);
                float tempVol = cur_personal.volume;
                cur_personal = (cur_personal.clip == neut_personal_1st.clip) ? neut_personal_2nd : neg_personal_2nd;
                Debug.Log("new track: " + cur_personal.clip);
                cur_personal.volume = tempVol;
                cur_personal.Play();
                personalIsFirst = false;
            }
        }
        // Manage the Ship track.
        if (cur_ship.isPlaying && shipIsFirst)
        {
            if (cur_ship.timeSamples >= cur_ship.clip.samples) //cur_ship.GetComponent<AudioClip>().samples)
            {
                cur_ship.Stop();
                cur_ship = (cur_ship == neut_ship_1st) ? neut_ship_2nd : neg_ship_2nd;
                cur_ship.Play();
                shipIsFirst = false;
            }
        }
        // Manage the Narrative track.
        if (cur_narrative.isPlaying && narrativeIsFirst)
        {
            if (cur_narrative.timeSamples >= cur_narrative.clip.samples) //cur_narrative.GetComponent<AudioClip>().samples)
            {
                cur_narrative.Stop();
                cur_narrative = (cur_narrative == neut_narrative_1st) ? neut_narrative_2nd : neg_narrative_2nd;
                cur_narrative.Play();
                narrativeIsFirst = false;
            }
        }

    }

    void HandleRitualStart_Personal()
    {
        Debug.Log("Started Personal Ritual!");
        StartCoroutine(FadeInTrack(cur_personal));
    }

    void HandleRitualStart_Ship()
    {
        StartCoroutine("FadeInTrack", cur_ship);
    }

    void HandleRitualStart_Narrative()
    {
        StartCoroutine("FadeInTrack", cur_narrative);
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
        //StartCoroutine("FadeOutTrack", amb_engineNoise);
        //StartCoroutine("FadeOutTrack", amb_telemetry);
    }
	
	// Fade between two tracks.
    void FadeTwoTracks (AudioSource cur, AudioSource target, float vol)
    {
        StartCoroutine("FadeOutTrack", cur);
        StartCoroutine(FadeInTrack(target, vol));
    }

    IEnumerator FadeOutTrack(AudioSource track)
    {
        float max = track.volume;
        float timeToChange = 0;
        while (track.volume > 0)
        {
            timeToChange += Time.deltaTime * fadeTime;
            track.volume = Mathf.Lerp(max, 0, timeToChange);
            yield break;
        }
    }

    IEnumerator FadeOutAndEndTrack(AudioSource track)
    {
        float max = track.volume;
        float timeToChange = 0;
        while (track.volume > 0)
        {
            timeToChange += Time.deltaTime * fadeTime;
            track.volume = Mathf.Lerp(max, 0, timeToChange);
            yield return null;
        }
        track.Stop();
    }

    IEnumerator FadeInTrack (AudioSource track, float vol = 1)
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
}
