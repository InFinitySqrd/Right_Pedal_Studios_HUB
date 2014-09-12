using UnityEngine;
using System.Collections;

public class SetUpMenu : MonoBehaviour {
    // Declare variables
    [SerializeField] Transform title, play, leaderboards, settings, backButton, information, muteSFX, muteBGM;
	[SerializeField] Font gameFont;

	private SpawnGates scoreVal;

	void Awake () {
	    // Set up all UI elements to scale with screen size
        title.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f, 3.0f * Screen.height / 4.0f, 1.0f));
        play.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f, Screen.height / 4.0f, 1.0f));
        leaderboards.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f - Screen.width / 3.0f, Screen.height / 5.6f, 1.0f));
        settings.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f + Screen.width / 3.0f, Screen.height / 5.6f, 1.0f));
		backButton.transform.position = settings.transform.position;
		information.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f + Screen.width / 2.8f, Screen.height / 5.6f, 1.0f));
        muteSFX.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f + Screen.width / 2.8f, Screen.height / 5.6f, 1.0f));
        muteBGM.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f + Screen.width / 2.8f, Screen.height / 5.6f, 1.0f));
	
        // Further set the positions of sub menus
        information.transform.position = new Vector3(information.transform.position.x, settings.transform.position.y + 3.0f * settings.transform.localScale.y, information.transform.position.z);
        muteSFX.transform.position = new Vector3(muteSFX.transform.position.x, settings.transform.position.y + 1.0f * settings.transform.localScale.y, muteSFX.transform.position.z);
        muteBGM.transform.position = new Vector3(muteBGM.transform.position.x, settings.transform.position.y + 2.0f * settings.transform.localScale.y, muteBGM.transform.position.z);
    }

	void Start() {
		scoreVal = GameObject.FindGameObjectWithTag("Player").GetComponent<SpawnGates>();
	}

	void OnGUI() {
		GUIStyle skin = new GUIStyle();
		skin.font = gameFont;
		skin.alignment = TextAnchor.MiddleCenter;
		skin.normal.textColor = Color.white;
		skin.fontSize = 72;

		if (scoreVal.score > 0) {
			GUI.Box(new Rect(Screen.width / 4.0f, Screen.height / 3.0f + Screen.height / 12.0f, Screen.width / 2.0f, Screen.height / 6.0f), "Score:" + scoreVal.score, skin);
		}

		skin.fontSize = 46;
		GUI.Box(new Rect(Screen.width / 4.0f, Screen.height / 3.0f + Screen.height / 5.0f, Screen.width / 2.0f, Screen.height / 6.0f), "High Score: " + PlayerPrefs.GetInt("Top Score"), skin);
	}
}
