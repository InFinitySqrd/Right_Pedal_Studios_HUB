using UnityEngine;
using System.Collections;

public class menuButtons : MonoBehaviour {
	// Declare variables
	[SerializeField] PlaneMovement planeVars;

	// Use this for initialization
	void Start () {
		// Load default variables into player prefs for the optimum planeVars configuration
		PlayerPrefs.SetFloat("movement", planeVars.forwardSpeed);
		PlayerPrefs.SetFloat("rotation", planeVars.rotationSpeed);
		PlayerPrefs.SetFloat("maxRotate", planeVars.maxRotationSpeed);
		PlayerPrefs.SetFloat("momentumRedux", planeVars.momentumReduction);
		PlayerPrefs.SetFloat("maxMomentum", planeVars.maxMomentum);
		PlayerPrefs.SetFloat("levelingForce", planeVars.levelingForce);
		PlayerPrefs.SetFloat("levelingDamp", planeVars.levelingDampener);
		PlayerPrefs.SetFloat("levelingDelay", planeVars.levelingDelay);
		PlayerPrefs.SetFloat("oppositeDirPush", planeVars.oppositeDirectionPush);
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
