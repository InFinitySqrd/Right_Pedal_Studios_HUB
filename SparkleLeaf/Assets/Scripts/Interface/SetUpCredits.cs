using UnityEngine;
using System.Collections;

public class SetUpCredits : MonoBehaviour {
    // Declare variables
    [SerializeField] Transform title, creditsList, backButton;
	[SerializeField] Font creditsFont;

	private float fadeSpeed = 0.0f;
	private Color textColour;

	void Awake () {
	    // Set up all UI elements to scale with screen size
		title.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f, Screen.height - Screen.height / 7.0f, 0.5f));
		creditsList.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f, Screen.height / 2.8f, 1.0f));
		backButton.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width - Screen.width / 8.0f, 0.0f + Screen.height / 12.0f, 0.5f));
    
		fadeSpeed = this.transform.GetChild(0).GetComponent<MenuTween>().fadeSpeed;
		textColour = new Color(Color.white.r, Color.white.g, Color.white.b, 0.0f);
	}

	void Start() {
		StartCoroutine("FadeTextIn");
	}

    void Update() {
        // Switch back to the main menu if the user hits the back button
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.LoadLevelAdditive("MenuScreen");
            Destroy(this.gameObject);
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
		skin.font = creditsFont;
		skin.normal.textColor = textColour;
		skin.alignment = TextAnchor.MiddleCenter;
		
		GUI.Box(new Rect(0.0f, 0.0f, adjustedWidth, nativeHeight), "\n\n\n\nDebra Polson\nNathaniel Holloway\nJames Finlayson\nWade Taylor\nRachel Grieveson\nNathan Corporal\n Nicky Watson", skin);
	}
	
	IEnumerator FadeTextIn() {
		while (textColour.a <= 1.0f) {
			textColour = new Color(Color.white.r, Color.white.g, Color.white.b, textColour.a + Time.deltaTime * fadeSpeed);
			yield return null;
		}
	}
}
