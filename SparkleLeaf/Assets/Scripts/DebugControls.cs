using UnityEngine;
using System.Collections;

public class DebugControls : MonoBehaviour {
	// Declare variables
	public bool paused = false;
	[SerializeField] float moveMax = 4.0f;
	[SerializeField] float rotateMax = 5.0f;
	[SerializeField] float momentumReduxMax = 10.0f;
	[SerializeField] float levelingForceMax = 10.0f;
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
			paused = false;
		
			if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height / 8.0f), "Open Debug Window")) {
				debugWindow = true;
			}
		} else {	
			paused = true;
						
			DrawControlOptions();
			DrawSliders(0, "MoveSpeed", ref planeVars.forwardSpeed, moveMax);
			DrawSliders(1, "RotateSpeed", ref planeVars.rotationSpeed, rotateMax);
			DrawSliders(2, "MomentumReduct", ref planeVars.momentumReduction, momentumReduxMax);
			DrawSliders(3, "LevelingForce", ref planeVars.levelingForce, levelingForceMax);
			
			
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
	
	private void DrawSliders(int lineNum, string labelName, ref float editedVar, float sliderMaxVal) {	
		GUI.Box(new Rect(0.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f , Screen.width / 6.0f, Screen.height / 12.0f), labelName);
		
		float sliderValue = editedVar;
		editedVar = GUI.HorizontalSlider(new Rect(Screen.width / 6.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f, Screen.width - Screen.width / 3.0f, Screen.height / 12.0f ), sliderValue, 0.0f, sliderMaxVal);
		
		GUI.Box(new Rect(Screen.width - Screen.width / 6.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f , Screen.width / 6.0f, Screen.height / 12.0f), editedVar.ToString());
	}
}
