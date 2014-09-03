using UnityEngine;
using System.Collections;

public class DebugControls : MonoBehaviour {
	// Declare variables
	public bool paused = false;
	[SerializeField] float moveMax = 10.0f;
	[SerializeField] float rotateMax = 10.0f;
	[SerializeField] float maxRotateMax = 10.0f;
	[SerializeField] float momentumReduxMax = 10.0f;
	[SerializeField] float maxMomentumMax = 10.0f;	
	[SerializeField] float levelingForceMax = 10.0f;
	[SerializeField] float levelingDampMax = 10.0f;
	[SerializeField] float levelingDelayMax = 10.0f;
	[SerializeField] float oppositeDirMax = 10.0f;
	private bool debugWindow = false;
	private PlaneMovement planeVars;
	
	void Awake() {
		planeVars = this.GetComponent<PlaneMovement>();
	}
	
	// Use this for initialization
	void Start () {
		planeVars.forwardSpeed = PlayerPrefs.GetFloat("movement");
		planeVars.rotationSpeed = PlayerPrefs.GetFloat("rotation");
		planeVars.maxRotationSpeed = PlayerPrefs.GetFloat("maxRotate");
		planeVars.momentumReduction = PlayerPrefs.GetFloat("momentumRedux");
		planeVars.maxMomentum = PlayerPrefs.GetFloat("maxMomentum");
		planeVars.levelingForce = PlayerPrefs.GetFloat("levelingForce");
		planeVars.levelingDampener = PlayerPrefs.GetFloat("levelingDamp");
		planeVars.levelingDelay = PlayerPrefs.GetFloat("levelingDelay");
		planeVars.oppositeDirectionPush = PlayerPrefs.GetFloat("oppositeDirPush");
	}
	
	void OnGUI() {
		if (!debugWindow) {
			paused = false;

			if (Input.touchCount >= 3 || Input.GetKeyDown(KeyCode.Delete)) {
				debugWindow = true;
			}
		} else {	
			paused = true;
						
			DrawControlOptions();
			DrawSliders(0, "MoveSpeed", ref planeVars.forwardSpeed, moveMax);
			DrawSliders(1, "RotateSpeed", ref planeVars.rotationSpeed, rotateMax);
			DrawSliders(2, "MaxRotation", ref planeVars.maxRotationSpeed, maxRotateMax);
			DrawSliders(3, "MomentumReduct", ref planeVars.momentumReduction, momentumReduxMax);
			DrawSliders(4, "MaxMomentum", ref planeVars.maxMomentum, maxMomentumMax);
			DrawSliders(5, "LevelingForce", ref planeVars.levelingForce, levelingForceMax);
			DrawSliders(6, "LevelingDamp", ref planeVars.levelingDampener, levelingDampMax);
			DrawSliders(7, "LevelingDelay", ref planeVars.levelingDelay, levelingDelayMax);
			DrawSliders(8, "OppositeDirPush", ref planeVars.oppositeDirectionPush, oppositeDirMax);
			
			if (GUI.Button(new Rect(Screen.width - Screen.width / 4.0f, Screen.height - Screen.height / 8.0f, Screen.width / 4.0f, Screen.height / 8.0f), "Close Window")) {
				debugWindow = false;

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
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void DrawControlOptions() {
		if (GUI.Button(new Rect(0, 0, Screen.width / 3.0f, Screen.height / 8.0f), "Hold Rot.")) {
			planeVars.controlMethod = 1;
		}
		
		if (GUI.Button(new Rect(Screen.width / 3.0f, 0, Screen.width / 3.0f, Screen.height / 8.0f), "Slide Rot.")) {
			planeVars.controlMethod = 2;
		}

		if (GUI.Button(new Rect(2.0f * Screen.width / 3.0f, 0, Screen.width / 3.0f, Screen.height / 8.0f), "Tilt Rot.")) {
			planeVars.controlMethod = 3;
		}
	}
	
	private void DrawSliders(int lineNum, string labelName, ref float editedVar, float sliderMaxVal) {	
		GUI.Box(new Rect(0.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f , Screen.width / 6.0f, Screen.height / 12.0f), labelName);
		
		float sliderValue = editedVar;
		editedVar = GUI.HorizontalSlider(new Rect(Screen.width / 6.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f, Screen.width - Screen.width / 3.0f, Screen.height / 12.0f ), sliderValue, 0.0f, sliderMaxVal);
		
		GUI.Box(new Rect(Screen.width - Screen.width / 6.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f , Screen.width / 6.0f, Screen.height / 12.0f), editedVar.ToString());
	}
}
