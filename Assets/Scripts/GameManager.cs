using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    
    public static GameManager instance { get; private set; }

    // Scene references
    public Transform Canvas;

    // Assets
    public GameObject TyperTextPrefab;
    public AudioClip TaskSuccessSound;
    public AudioClip TaskFailSound;

	void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
	}
}
