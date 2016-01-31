using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectTyper : Interactable {

    [System.NonSerialized]
    public string StringToType;

    //[System.NonSerialized]
    public AudioClip UseSound;
    //[System.NonSerialized]
    public AudioClip SuccessSound;
    //[System.NonSerialized]
    public AudioClip FailSound;

    private AudioSource source;

    private Color DimColor = Color.white;
    private Color BrightColor = Color.blue;

    private string dimColorHex;
    private string brightColorHex;

    private Text uiText;

    private int typeProgress = 0;
    private string nextLetter;

    private bool showText = false;

    private string prevInput = "";

    protected override void Activate()
    {
        // Player enters trigger
    }

    protected override void Deactivate()
    {
        // Player leaves trigger
    }

    protected override void SetNearest()
    {
        ShowText();
    }

    protected override void UnsetNearest()
    {
        HideText();
    }

    public override void HandleUse()
    {
        if(Input.inputString != "")
        {
            UseInteractable();
        }
    }

    protected override void Use()
    {
        typeProgress = 0;
        UpdateLabelColor();

        GetNextLetter();

        if(UseSound != null)
        {
            // Play looping use sound
            source.loop = true;
            source.clip = UseSound;
            source.Play();
        }
    }

    protected override void EndUse()
    {
        typeProgress = 0;
        GetNextLetter();
        UpdateLabelColor();

        if(UseSound != null)
        {
            if(source.isPlaying)
            {
                source.Stop();
            }
        }

        if(IsDone)
        {
            // Play success sound
            if(SuccessSound != null)
            {
                source.PlayOneShot(SuccessSound);
            }
        }
        else
        {
            // Play fail sound
            if(GameManager.instance.PersonalTaskFailSound != null)
            {
                source.PlayOneShot(GameManager.instance.PersonalTaskFailSound);
            }
        }

        prevInput = "";
    }

    protected override void Done()
    {
        prevInput = "";
    }

	protected virtual void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.loop = false;
        source.playOnAwake = false;
        source.spatialize = false;

        // Get hex values for colors
        dimColorHex = ColorToHex(DimColor);
        brightColorHex = ColorToHex(BrightColor);

	    // Instantiate label
        GameObject textObject = (GameObject)Instantiate(GameManager.instance.TyperTextPrefab);
        textObject.transform.SetParent(GameManager.instance.Canvas, false);
        uiText = textObject.GetComponent<Text>();

        // Disable label by default
        textObject.SetActive(false);

        base.Awake();

        enabled = false;
	}
	
    void Update()
    {
        if(IsInUse)
        {
            string input = Input.inputString;

            //TODO GameManager change state to Interactable in use

            // Typing out sheeeit
            if(typeProgress < StringToType.Length)
            {
                if(input != "" && prevInput == "")
                {
                    if(input == nextLetter)
                    {
                        typeProgress++;

                        if(typeProgress < StringToType.Length)
                        {
                            GetNextLetter();
                            UpdateLabelColor();
                        }
                        else
                        {
                            UpdateLabelColor();

                            DoneInteractable();
                        }
                    }
                    else
                    {
                        EndUseInteractable();
                    }
                }
            }

            prevInput = input;
        }
	}

    private void GetNextLetter()
    {
    	if (StringToType != null) {
			if(StringToType.Length > 0)
	        {
	            nextLetter = StringToType.Substring(typeProgress, 1);
	        }
	        else
	        {
	            nextLetter = "";
	        }
    	}

    }

    private void UpdateLabelColor()
    {
        // Update label color
        uiText.text = "<color=#" + brightColorHex + ">" + StringToType.Substring(0, typeProgress) + "</color><color=#" + dimColorHex + ">" + StringToType.Substring(typeProgress, (StringToType.Length) - typeProgress) + "</color>";
    }

    void LateUpdate()
    {
        // Update text position based on camera position
        uiText.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    private void ShowText()
    {
        // Activate text object
        uiText.gameObject.SetActive(true);
        enabled = true;

        typeProgress = 0;
        UpdateLabelColor();
        GetNextLetter();

        showText = true;
    }

    private void HideText()
    {
        // Deactive text object
        uiText.gameObject.SetActive(false);
        enabled = false;

        showText = false;
        prevInput = "";
    }

    // Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
    public static string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    public static Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r,g,b, 255);
    }

    protected override void Initialize()
    {
        Task t = NarrativeManager.Instance.FindTaskByNameToday(Name);

        if(t != null)
        {
            gameObject.SetActive(true);
            Description = t.description;
            StringToType = t.phraseToType;

            Debug.Log(t.loopSFX, gameObject);
            Debug.Log(t.successSFX, gameObject);
            Debug.Log(t.failSFX, gameObject);

            if(t.loopSFX != "")
            {
                UseSound = (AudioClip)Resources.Load("Sfx/Personal/" + t.loopSFX);
            }
            if(t.successSFX != "")
            {
                SuccessSound = (AudioClip)Resources.Load("Sfx/Personal/" + t.successSFX);
            }
            if(t.failSFX != "")
            {
                FailSound = (AudioClip)Resources.Load("Sfx/Personal/" + t.failSFX);
            }

            gameObject.SetActive(true);
            UpdateLabelColor();

            if(IsNearest)
            {
                ShowText();
            }
            else
            {
                HideText();
            }
        }
        else
        {
            Debug.LogWarning("Task " + Name + " does not exist on day " + NarrativeManager.Instance.Today().dayNumber + "!", gameObject);
            gameObject.SetActive(false);
        }
    }

    public override void Reset()
    {
    	Debug.Log("Typer Object Reset");
        base.Reset();

        prevInput = "";
        showText = false;
        typeProgress = 0;
        Initialize();
        GetNextLetter();
    }
}
