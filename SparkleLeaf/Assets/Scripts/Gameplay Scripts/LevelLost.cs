using UnityEngine;
using System.Collections;

public class LevelLost : MonoBehaviour {
	// Declare variables
	[SerializeField] string loseText;
	public bool lost = false;

	private float timeUntilDeath;
	private GameAnalytics GAStuff;

    private bool menuUp = false;

	// FMOD relating to death and BGM
    /*
	private FMOD.Studio.EventInstance FMOD_Music;
	private FMOD.Studio.ParameterInstance FMOD_Death;
	private float deathMusicTime = 0.0f;

	// FMOD relating to ambience
	private FMOD.Studio.EventInstance FMOD_Ambience;
	private FMOD.Studio.ParameterInstance FMOD_AmbienceTime;
    */

	// Use this for initialization
	void Start () {
		GAStuff = GameObject.FindGameObjectWithTag("GameAnalytics").GetComponent<GameAnalytics>();

        /*
		FMOD_Music = FMOD_StudioSystem.instance.GetEvent ("event:Music/Gameplay");
		FMOD_Ambience = FMOD_StudioSystem.instance.GetEvent ("event:/Ambience/Forest");

		if (FMOD_Music.getParameter ("Death", out FMOD_Death) != FMOD.RESULT.OK) {
			Debug.LogError ("death paramter is not working dickhead");
			return;

		}

		if (FMOD_Ambience.getParameter("Time", out FMOD_AmbienceTime) != FMOD.RESULT.OK) {
			Debug.LogError ("ambience paramter is not working dickhead");
			return;

		}

		this.FMOD_Music.start ();
		this.FMOD_Ambience.start ();
        */

		timeUntilDeath = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		timeUntilDeath += Time.deltaTime;

        if (!menuUp && lost) {
            Application.LoadLevelAdditive("MenuScreen");
            menuUp = true;

            if (Input.touchCount >= 3 || Input.GetKeyDown(KeyCode.Delete)) {
                Application.LoadLevel("MainMenu");
            }
        }
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Obstacle") {
			lost = true;

			string name = other.transform.parent.GetComponent<MovingGates>().gateName;
			GAStuff.SetObstacleDeath(name);
			GAStuff.SetTimeBeforeDeath(timeUntilDeath);
		}
	}
}
