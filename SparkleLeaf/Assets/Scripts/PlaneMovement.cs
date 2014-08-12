using UnityEngine;
using System.Collections;

public class PlaneMovement : MonoBehaviour {
	// Declare variables
	//public int numRotations = 10;
	[SerializeField] float forwardSpeed = 1.0f;
	[SerializeField] float rotationSpeed = 1.0f;
	[SerializeField] float momentumReduction = 1.0f;
	[SerializeField] float levelingForce = 1.0f;
	[SerializeField] float deadZone = 0.0f;
	private bool debugWindow = false;
	private int controlMethod = 1;
	private Vector3 slideTouchPos;
	private float momentum = 0.0f;
	private LevelLost gameState;
	
	// Use this for initialization
	void Start () {
		gameState = this.GetComponent<LevelLost>();	
	}
	
	void OnGUI () {
		if (!debugWindow) {
			if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height / 8.0f), "Open Debug Window")) {
				debugWindow = true;
			}
		} else {			
			if (GUI.Button(new Rect(0, 0, Screen.width / 2.0f, Screen.height / 8.0f), "Hold Rot.")) {
				controlMethod = 1;
			}
			
			if (GUI.Button(new Rect(Screen.width / 2.0f, 0, Screen.width / 2.0f, Screen.height / 8.0f), "Slide Rot.")) {
				controlMethod = 2;
			}
			
			if (GUI.Button(new Rect(Screen.width - Screen.width / 4.0f, 0.0f + Screen.height / 8.0f, Screen.width / 4.0f, Screen.height / 8.0f), "Close Window")) {
				debugWindow = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameState.lost) {
			this.transform.Translate(Vector3.forward * forwardSpeed);
			PlaneRotation();
		}
		
		switch (controlMethod) {
			case 1:
				// Rotate the plane around a point
				//if (Input.touches.Length > 0) {
				if (Input.GetMouseButton(0)) {
					// if (Input.touches[0].position.x < Screen.width / 2.0f) {
					if (Input.mousePosition.x < Screen.width / 2.0f) {
						momentum += Time.deltaTime * rotationSpeed;
					} else {
						momentum -= Time.deltaTime * rotationSpeed;
					}
				} else {
					if (momentum > 0.0f) {
						momentum -= Time.deltaTime * momentumReduction;
					} else if (momentum < 0.0f) {
						momentum += Time.deltaTime * momentumReduction;
					}
					
					LevelPlane();
				}
				
				break;
			case 2:
				// Rotate the plane by sliding on the screen
				if (Input.touches.Length > 0) {
					if (Input.touches[0].phase == TouchPhase.Began) {
						// Store the starting touch position
						slideTouchPos = Input.touches[0].position;
					} else if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Stationary) {
						// Rotate the plane based off of the new touch position
						if (Input.touches[0].position.x < slideTouchPos.x + deadZone) {
							momentum += Time.deltaTime;
						} else if (Input.touches[0].position.x > slideTouchPos.x - deadZone){
							momentum -= Time.deltaTime;
						}
					}
				}
				break;
			default:
				Debug.LogError("Invalid Control Scheme");
				break;
		}
	}
	
	private void PlaneRotation() {	
		this.transform.Rotate(Vector3.forward * momentum);
	}
	
	private void LevelPlane() {
		if (this.transform.eulerAngles.z <= 90.0f) {
			momentum -= Time.deltaTime * levelingForce;
		} else if (this.transform.eulerAngles.z <= 180.0f) {
			momentum -= Time.deltaTime * levelingForce;
		} else if (this.transform.eulerAngles.z <= 270.0f) {
			momentum += Time.deltaTime * levelingForce;
		} else {
			momentum += Time.deltaTime * levelingForce;
		}
	}
}
