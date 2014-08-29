using UnityEngine;
using System.Collections;

public class LevelLost : MonoBehaviour {
	// Declare variables
	[SerializeField] string loseText;
	public bool lost = false;

	private float timeUntilDeath;

	// Use this for initialization
	void Start () {
		timeUntilDeath = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		timeUntilDeath += Time.deltaTime;
	}
	
	void OnGUI() {
		if (lost) {
			// Display that the player has lost the game
			GUIStyle skin = new GUIStyle();
			skin.fontSize = 40;
			skin.alignment = TextAnchor.MiddleCenter;			
			skin.normal.textColor = Color.white;
			
			GUI.Box(new Rect(0.0f, Screen.height - Screen.height / 8.0f, Screen.width, Screen.height / 8.0f), "Game Over", skin);

			skin.fontSize = 34;
			if (GUI.Button(new Rect(0.0f, 0.0f + Screen.height / 3.0f, Screen.width, Screen.height / 8.0f), "Restart?", skin)) {
				Application.LoadLevel(Application.loadedLevel);
			}

			if (GUI.Button(new Rect(0.0f + Screen.width / 3.0f, 0.0f + 1.6f * Screen.height / 3.0f, Screen.width / 3.0f, Screen.height / 8.0f), "Menu", skin)) {
				Application.LoadLevel(Application.loadedLevel-1);
			}
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Obstacle") {
			lost = true;

			string name = other.transform.parent.GetComponent<MovingGates>().gateName;

			GA.API.Design.NewEvent("Player Died To " + name, timeUntilDeath);
		}
	}
}
