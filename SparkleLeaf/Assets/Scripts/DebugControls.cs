using UnityEngine;
using System.Collections;

public class DebugControls : MonoBehaviour {
	// Declare variables
	[SerializeField] float sliderMaxVal = 10.0f;
	private bool debugWindow = false;
	private PlaneMovement planeVars;
	
	void Awake() {
		planeVars = this.GetComponent<PlaneMovement>();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	void OnGUI() {
		if (!debugWindow) {
			if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height / 8.0f), "Open Debug Window")) {
				debugWindow = true;
			}
		} else {			
			DrawControlOptions();
			DrawSliders(0, "MoveSpeed", ref planeVars.forwardSpeed);
			DrawSliders(1, "RotateSpeed", ref planeVars.rotationSpeed);
			DrawSliders(2, "MomentumReduct", ref planeVars.momentumReduction);
			DrawSliders(3, "LevelingForce", ref planeVars.levelingForce);
			
			
			if (GUI.Button(new Rect(Screen.width - Screen.width / 4.0f, 4.0f * Screen.height / 12.0f + Screen.height / 8.0f, Screen.width / 4.0f, Screen.height / 8.0f), "Close Window")) {
				debugWindow = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void DrawControlOptions() {
		if (GUI.Button(new Rect(0, 0, Screen.width / 2.0f, Screen.height / 8.0f), "Hold Rot.")) {
			planeVars.controlMethod = 1;
		}
		
		if (GUI.Button(new Rect(Screen.width / 2.0f, 0, Screen.width / 2.0f, Screen.height / 8.0f), "Slide Rot.")) {
			planeVars.controlMethod = 2;
		}
	}
	
	private void DrawSliders(int lineNum, string labelName, ref float editedVar) {	
		GUI.Box(new Rect(0.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f , Screen.width / 6.0f, Screen.height / 12.0f), labelName);
		
		float sliderValue = editedVar;
		editedVar = GUI.HorizontalSlider(new Rect(Screen.width / 6.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f, Screen.width - Screen.width / 3.0f, Screen.height / 12.0f ), sliderValue, 0.0f, sliderMaxVal);
		
		GUI.Box(new Rect(Screen.width - Screen.width / 6.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f , Screen.width / 6.0f, Screen.height / 12.0f), editedVar.ToString());
	}
}
