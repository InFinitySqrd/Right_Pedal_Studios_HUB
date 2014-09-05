using UnityEngine;
using System.Collections;

public class MenuButtons : MonoBehaviour {
	// Declare variables
	[SerializeField] PlaneMovement planeVars;
    [SerializeField] SpawnGates gateVars;

	// Use this for initialization
	void Start () {
        // Reset the tutorial values in player prefs
        PlayerPrefs.SetInt("Tutorial Completed", 0);

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

        // Load optimum values for gateVars
        PlayerPrefs.SetFloat("spawnTime", gateVars.spawnTime);
        PlayerPrefs.SetFloat("spawnDistance", gateVars.spawnDistance);
        PlayerPrefs.SetFloat("rotationPercentage", gateVars.percentageToRotate);
        PlayerPrefs.SetFloat("minRotationSpeed", gateVars.minSpeed);
        PlayerPrefs.SetFloat("maxRotationSpeed", gateVars.maxSpeed);
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
