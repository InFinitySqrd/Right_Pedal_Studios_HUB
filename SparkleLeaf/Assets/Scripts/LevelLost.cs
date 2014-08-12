using UnityEngine;
using System.Collections;

public class LevelLost : MonoBehaviour {
	// Declare variables
	[SerializeField] string loseText;
	public bool lost = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		if (lost) {
			// Display that the player has lost the game
			GUIStyle skin = new GUIStyle();
			skin.fontSize = 40;
			skin.alignment = TextAnchor.MiddleCenter;
			GUI.Box(new Rect(0.0f, Screen.height - Screen.height / 8.0f, Screen.width, Screen.height / 8.0f), "YOU LOST", skin);
			
			if (GUI.Button(new Rect(0.0f + Screen.width / 3.0f, 0.0f + 2.0f * Screen.height / 3.0f, Screen.width / 3.0f, Screen.height / 8.0f), "Restart?")) {
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Obstacle") {
			lost = true;
		}
	}
}
