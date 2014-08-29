using UnityEngine;
using System.Collections;

public class menuButtons : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUIStyle skin = new GUIStyle();
		skin.fontSize = 60;
		skin.alignment = TextAnchor.MiddleCenter;
		if (GUI.Button(new Rect(0.0f, 0.0f, Screen.width, Screen.height), "Start Game!", skin)) {
			Application.LoadLevel(Application.loadedLevel + 1);
		}
	}
}
