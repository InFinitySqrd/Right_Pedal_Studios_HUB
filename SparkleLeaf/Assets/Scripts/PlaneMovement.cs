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
	[SerializeField] float maxRotationSpeed = 4.0f;
	[SerializeField] float levelingDampener = 1.5f;
	[SerializeField] float levelingDelay = 0.3f;
	[SerializeField] float deadZone = 0.0f;
	private Vector3 slideTouchPos;
	private float momentum = 0.0f;
	private float levelingTimer = 0.0f;
	private LevelLost gameState;
	private DebugControls pause;
	
	// Variables to control speed increases
	[SerializeField] float speedIncreaseRate = 5.0f;
	[SerializeField] float speedIncrementor = 0.2f;
	private float speedIncreaseTimer = 0.0f;
	
	// Environment
	public Transform environmentCentre;

	void Awake() {
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start () {
		gameState = this.GetComponent<LevelLost>();	
		pause = this.GetComponent<DebugControls>();
		environmentCentre = GameObject.FindGameObjectWithTag("EnvironmentCentre").transform;
		
		momentum = 0.0f;
	}
	
	void OnGUI () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {		
		if (!gameState.lost && !pause.paused) {
			// Move the plane forward
			//this.transform.Translate(Vector3.forward * forwardSpeed);
			// Rotate the environment around the player
			environmentCentre.transform.Rotate(Vector3.left * forwardSpeed / 4.0f);
					
			PlaneRotation();
		}
		
		Controls();
		
		if (speedIncreaseTimer >= speedIncreaseRate) {
			IncreaseMovement(speedIncrementor);
			speedIncreaseTimer = 0.0f;
		} else {
			speedIncreaseTimer += Time.deltaTime;
		}
	}
	
	private void PlaneRotation() {	
		if (!pause.paused) {
			this.transform.Rotate(Vector3.forward * momentum);
		}
	}
	
	private void LevelPlane() {
		if (this.transform.eulerAngles.z <= 90.0f) {
			momentum -= Time.deltaTime * levelingForce;
		} else if (this.transform.eulerAngles.z <= 180.0f) {
			momentum += Time.deltaTime * levelingForce;
		} else if (this.transform.eulerAngles.z <= 270.0f) {
			momentum -= Time.deltaTime * levelingForce;
		} else if (this.transform.eulerAngles.z <= 360.0f){
			momentum += Time.deltaTime * levelingForce;
		}
		
		if (this.transform.eulerAngles.z > 175.0f || this.transform.eulerAngles.z < 195.0f) {
			if (momentum > 0.0f) {				
				momentum -= Time.deltaTime * (levelingForce / levelingDampener);
			} if (momentum < 0.0f) {				
				momentum += Time.deltaTime * (levelingForce / levelingDampener);
			}
		} else if (this.transform.eulerAngles.z > 345.0f || this.transform.eulerAngles.z < 15.0f) {
			if (momentum > 0.0f) {				
				momentum -= Time.deltaTime * (levelingForce / levelingDampener);
			} if (momentum < 0.0f) {				
				momentum += Time.deltaTime * (levelingForce / levelingDampener);
			}

		}
	}
	
	private void Controls() {
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
				
				levelingTimer = 0.0f;
				
			} else {
				if (momentum > 0.0f) {
					momentum -= Time.deltaTime * momentumReduction;
				} else if (momentum < 0.0f) {
					momentum += Time.deltaTime * momentumReduction;
				}
				
				if (levelingTimer >= levelingDelay) {
					LevelPlane();
				} else {
					levelingTimer += Time.deltaTime;
				}
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
	
	public void IncreaseMovement(float incrementor) {
		forwardSpeed += incrementor;
		
		if (rotationSpeed < maxRotationSpeed) {
			rotationSpeed += incrementor;
		} else {
			rotationSpeed = maxRotationSpeed;
		}
	}
}
