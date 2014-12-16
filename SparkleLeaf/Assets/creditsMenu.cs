using UnityEngine;
using System.Collections;

public class creditsMenu : MonoBehaviour {

	[SerializeField] Texture CreditsTexture, CreditsLogo, BackButton;
	private GameObject audioManager;
	private float timer = 0.0f;

	// Use this for initialization
	void Start () {
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
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), CreditsTexture);
		GUI.DrawTexture(new Rect(Screen.width * 0.1f,0,Screen.width * 0.8f,Screen.width * 0.45f), CreditsLogo);
		GUI.DrawTexture(new Rect(Screen.width * 0.75f,Screen.height - Screen.width * 0.25f,Screen.width * 0.175f,Screen.width * 0.175f), BackButton);

	}
}
