using UnityEngine;
using System.Collections;

public class PlaneMovement : MonoBehaviour {
	// Public variables
	public int controlMethod = 1;
	public float forwardSpeed = 1.0f;
	public float rotationSpeed = 1.0f;
	public float momentumReduction = 1.0f;
	public float levelingForce = 1.0f;
	
	// Declare variables
	[SerializeField] float deadZone = 0.0f;
	private Vector3 slideTouchPos;
	private float momentum = 0.0f;
	private LevelLost gameState;
	
	// Use this for initialization
	void Start () {
		gameState = this.GetComponent<LevelLost>();	
	}
	
	void OnGUI () {

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
