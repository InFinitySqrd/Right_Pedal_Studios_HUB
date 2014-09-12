using UnityEngine;
using System.Collections;

public class SetUpCredits : MonoBehaviour {
    // Declare variables
    [SerializeField] Transform creditsList, backButton;
	[SerializeField] Font fontStyle;

	private float fadeSpeed = 0.0f;
	private Color textColour;

	void Awake () {
	    // Set up all UI elements to scale with screen size
		creditsList.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f, Screen.height / 2.2f, 1.0f));
		backButton.transform.position = this.camera.ScreenToWorldPoint(new Vector3(0.0f + Screen.width / 8.0f, Screen.height - Screen.height / 12.0f, 1.0f));
    
		fadeSpeed = this.transform.GetChild(0).GetComponent<MenuTween>().fadeSpeed;
		textColour = new Color(Color.white.r, Color.white.g, Color.white.b, 0.0f);
	}

	void Start() {
		StartCoroutine("FadeTextIn");
	}

	void OnGUI() {
		GUIStyle skin = new GUIStyle();
		skin.font = fontStyle;
		skin.fontSize = 42;
		skin.normal.textColor = textColour;
		skin.alignment = TextAnchor.MiddleCenter;

		GUI.Box(new Rect(0.0f, 0.0f, Screen.width, Screen.height), "\nDebra Polson\nNathaniel Holloway\nJames Finlayson\nWade Taylor\nRachel Grieveson\nNathan Corporal\n Nicky Watson", skin);
	}

	IEnumerator FadeTextIn() {
		while (textColour.a <= 1.0f) {
			textColour = new Color(Color.white.r, Color.white.g, Color.white.b, textColour.a + Time.deltaTime * fadeSpeed);
			yield return null;
		}
	}
}
