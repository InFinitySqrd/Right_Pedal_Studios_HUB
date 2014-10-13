using UnityEngine;
using System.Collections;

public class SetUpPlayerPrefs : MonoBehaviour {
	// Declare variables
	[SerializeField] PlaneMovement planeVars;
    [SerializeField] SpawnGates gateVars;

    [SerializeField] GameObject audioManager;

	// Use this for initialization
	void Start () {
        // Reset the tutorial values in player prefs
        //PlayerPrefs.SetInt("TutorialComplete", 0);
		PlayerPrefs.SetInt("FirstLaunch", 0);
		PlayerPrefs.SetInt("Control Method", 1);

        // Variables to track social integration
        PlayerPrefs.SetInt("GooglePlayActivated", 0);

        // Audio levels to be used in the game
//        PlayerPrefs.SetInt("AudioEnabled", 1);
//        for (int i = 0; i < audioManager.transform.childCount; i++) {
//            PlayerPrefs.SetFloat("AudioChild" + i, audioManager.transform.GetChild(i).audio.volume);
//        }

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
        PlayerPrefs.SetFloat("newObstacleTime", gateVars.newObstacleTime);
        PlayerPrefs.SetFloat("rotateNewObstacleTime", gateVars.rotateNewObstacleTime);
        PlayerPrefs.SetFloat("randomRotationSpeedIncrementor", gateVars.randomRotationSpeedIncrementor);
        PlayerPrefs.SetFloat("rotationSpeedIncreaseTime", gateVars.rotationSpeedIncreaseTime);
	}
}
