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
		if (GUI.Button(new Rect(Screen.width/3, Screen.height/3, Screen.width/3f, Screen.height/3f), "Start Game!", skin)) {
			Application.LoadLevel(Application.loadedLevel + 1);

		}
		}
}
