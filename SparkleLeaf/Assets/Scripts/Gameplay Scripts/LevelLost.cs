using UnityEngine;
using System.Collections;

public class LevelLost : MonoBehaviour {
	// Declare variables
	[SerializeField] int gamesBetweenAd = 1;
	public bool lost = false;

	private float timeUntilDeath;
	private GameAnalytics GAStuff;

	private int monsterKillType = 0; // 0 = Line, 1 = Cross

    private bool menuUp = false;

    private bool died = false;
    [SerializeField] GameObject planeModel;
    [SerializeField] GameObject brokenPlane;
    [SerializeField] ParticleSystem[] rippingEffects;

    private GameObject audioManager;

    private Animator killerAnim;

    private UnityAdsIntegration adCall;

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
        audioManager = GameObject.FindGameObjectWithTag ("FMOD_Manager");
        adCall = Camera.main.GetComponent<UnityAdsIntegration>();
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

        if (lost && !died && !killerAnim.GetCurrentAnimatorStateInfo(0).IsName("KillAnim")) {
            PlaneDeath();
            if (PlayerPrefs.GetInt("AdCounter") >= gamesBetweenAd) {
                adCall.AdDisplay();
                PlayerPrefs.SetInt("AdCounter", 0);
            }
        }

        if (!menuUp && lost) {
			audioManager.GetComponent<FMOD_Manager>().ForestSetDeath(true);
			audioManager.GetComponent<FMOD_Manager>().PlaneDeath(monsterKillType);

            PlayerPrefs.SetInt("AdCounter", PlayerPrefs.GetInt("AdCounter") + 1);

            Application.LoadLevelAdditive("MenuScreen");
            menuUp = true;

            /*
            if (Input.touchCount >= 3 || Input.GetKeyDown(KeyCode.Delete)) {
                Application.LoadLevel("MainMenu");
            }
            */
        }
	}

    private void PlaneDeath() {
        // Spawn the broken plane
        Instantiate(brokenPlane, planeModel.transform.position, planeModel.transform.rotation);

        // Disable the plane model
        planeModel.SetActive(false);

        // Play particle effects
        foreach (ParticleSystem effect in rippingEffects) {
            effect.Play();
        }

        // Show that the death effect has already been played
        died = true;
    }

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Obstacle") {
			lost = true;


            if (other.transform.parent.name.Contains("Gate")) {
                killerAnim = other.transform.parent.FindChild("lineMonster").GetComponent<Animator>();
                killerAnim.SetTrigger("TriggerKillAnim");
				monsterKillType = 0;
            } else if (other.transform.parent.name.Contains("Cross")) {
                killerAnim = other.transform.parent.FindChild("crossMonster").GetComponent<Animator>();
                killerAnim.SetTrigger("TriggerKillAnim");
				monsterKillType = 1;
            }

			string name = other.transform.parent.GetComponent<MovingGates>().gateName;
			GAStuff.SetObstacleDeath(name);
			GAStuff.SetTimeBeforeDeath(timeUntilDeath);
		}
	}
}
