using UnityEngine;
using System.Collections;

public class SetUpMenu : MonoBehaviour {
    // Declare variables
    [SerializeField] Transform title, play, leaderboards, settings, backButton, information, muteSFX, share;
	[SerializeField] Font prevScoreFont, highScoreFont;

	private SpawnGates scoreVal;

    private FadeBetweenAudio audioFade; 

	void Awake () {
	    // Set up all UI elements to scale with screen size
        title.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f, 3.0f * Screen.height / 4.0f, 1.0f));
        play.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f, Screen.height / 5.2f, 1.0f));
        leaderboards.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f - Screen.width / 3.0f, Screen.height / 6.4f, 1.0f));
        settings.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f + Screen.width / 3.0f, Screen.height / 6.4f, 1.0f));
		backButton.transform.position = settings.transform.position;
		information.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f + Screen.width / 2.8f, Screen.height / 6.8f, 1.0f));
        muteSFX.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f + Screen.width / 2.8f, Screen.height / 6.8f, 1.0f));
        //muteBGM.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f + Screen.width / 2.8f, Screen.height / 5.6f, 1.0f));		
		share.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 1.14f, Screen.height / 2.14f, 1.0f));

        // Further set the positions of sub menus
        information.transform.position = new Vector3(information.transform.position.x, settings.transform.position.y + 1.6f * settings.transform.localScale.y, information.transform.position.z);
        muteSFX.transform.position = new Vector3(muteSFX.transform.position.x, settings.transform.position.y + 0.8f * settings.transform.localScale.y, muteSFX.transform.position.z);
        //muteBGM.transform.position = new Vector3(muteBGM.transform.position.x, settings.transform.position.y + 2.0f * settings.transform.localScale.y, muteBGM.transform.position.z);
    
        audioFade = GameObject.Find("AudioManager").GetComponent<FadeBetweenAudio>();
        audioFade.FadeAudio(false);
    }

	void Start() {
		scoreVal = GameObject.FindGameObjectWithTag("Player").GetComponent<SpawnGates>();
	}

	void Update() {
		if (scoreVal.score == 0) {	
			share.collider.enabled = false;
			share.renderer.enabled = false;
		}
	}

	float nativeWidth = 1920.0f;
	float nativeHeight = 1080.0f;
	void OnGUI() {
		// Get a scaling factor based off of the native resolution and preset resolution
		float rx = Screen.width / nativeWidth;
		float ry = Screen.height / nativeHeight;
			
		// Scale width the same as height - cut off edges to keep ratio the same
		GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(ry, ry, 1));
			
		// Get width taking into account edges being cut off or extended
		float adjustedWidth = nativeWidth * (rx / ry);

		GUIStyle skin = new GUIStyle();
		skin.font = prevScoreFont;
		skin.alignment = TextAnchor.MiddleLeft;
		skin.normal.textColor = Color.white;

		if (scoreVal.score > 0) {
			GUI.Box(new Rect(adjustedWidth / 14.0f, nativeHeight / 2.8f + nativeHeight / 12.0f, adjustedWidth / 2.0f, nativeHeight / 6.0f), "Score:" + scoreVal.score, skin);
		}

		skin.font = highScoreFont;
        skin.alignment = TextAnchor.MiddleCenter;
		GUI.Box(new Rect(adjustedWidth / 2.0f - adjustedWidth / 4.0f, nativeHeight / 2.4f + nativeHeight / 8.0f, adjustedWidth / 2.0f, nativeHeight / 6.0f), "High Score: " + PlayerPrefs.GetInt("Top Score"), skin);
	}
}
