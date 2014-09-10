using UnityEngine;
using System.Collections;

public class SetUpMenu : MonoBehaviour {
    // Declare variables
    [SerializeField] Transform title, play, leaderboards, settings, backButton, information, muteSFX, muteBGM;

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

    // Use this for initialization
    void Start() {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
