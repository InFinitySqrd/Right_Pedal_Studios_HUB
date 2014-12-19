using UnityEngine;
using System.Collections;

public class creditsMenu : MonoBehaviour {

	[SerializeField] Texture CreditsTexture, CreditsLogo, BackButton, Black;
	private GameObject audioManager;
	private float timer = 0.0f;

	// Use this for initialization
	void Start () {
		print ("Width" + Screen.width + " Height" + Screen.height);
		audioManager = GameObject.Find("AudioManager");
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > 1.0f) {
						if (Input.GetMouseButton (0)) {
								Application.LoadLevelAdditive ("MenuScreen");
								audioManager.GetComponent<FMOD_Manager> ().MenuTransition (false);
								Destroy (this.transform.root.gameObject);
						}
				}
	}

	void OnGUI() {
		float screenWidth = Screen.width;
		float screenHeight = Screen.height;
		float screenRatio = screenWidth / screenHeight;

		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), Black);

		// 9:16 config
		if (screenRatio < 0.7f) {
						GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), CreditsTexture);
						GUI.DrawTexture (new Rect (Screen.width * 0.1f, 0, Screen.width * 0.8f, Screen.width * 0.45f), CreditsLogo);
			print ("169" + "  " + screenRatio);
				}

		// 3:4 config
		if (screenRatio > 0.7f) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), CreditsTexture);
			GUI.DrawTexture (new Rect (Screen.width * 0.2f, 0, Screen.width * 0.6f, Screen.width * 0.35f), CreditsLogo);
			print ("34" + "   " + screenRatio);
				}
		GUI.DrawTexture(new Rect(Screen.width * 0.75f,Screen.height - Screen.width * 0.25f,Screen.width * 0.175f,Screen.width * 0.175f), BackButton);

	}
}
