﻿using UnityEngine;
using System.Collections;

public class PlaneMovement : MonoBehaviour {
	// Public variables
	public int controlMethod = 1;
	public float forwardSpeed = 1.0f;
	public float rotationSpeed = 1.0f;
	public float momentumReduction = 1.0f;
	public float levelingForce = 1.0f;
	public float maxRotationSpeed = 4.0f;
	public float maxMomentum = 7.5f;
	public float levelingDampener = 1.5f;
	public float levelingDelay = 0.3f;
	public float oppositeDirectionPush = 2.0f;
	
    [SerializeField] Animator planeAnim;

    // Declare variables
	[SerializeField] float deadZone = 0.0f;
	[SerializeField] float tiltDeadZone = 0.1f;
	private Vector3 slideTouchPos;
	private float momentum = 0.0f;
	private float levelingTimer = 0.0f;
	private LevelLost gameState;
	private DebugControls pause;
	private float avgMomentum;
	
	// Variables to control speed increases
	[SerializeField] float speedIncreaseRate = 5.0f;
	[SerializeField] float speedIncrementor = 0.2f;
	[SerializeField] float speedGrowthPerStep = 1.01f;
	private float speedIncreaseTimer = 0.0f;

	// Audio variables
	private GameObject rotationSound;
	
	// Environment
	public Transform environmentCentre;

	private GameAnalytics GAStuff;

	private bool tutorialActivated = false;

    private GameObject audioManager;

    private bool gameInitliased = false;

	private bool flyIn = true;

	private bool initialiseAudio = false;

    // Firefly particle effects
   [SerializeField] ParticleSystem fireflyParticles;
   [SerializeField] float particleMinSpeed = 1.0f, particleMaxSpeed = 1.0f;

	private bool triggerRight, triggerLeft = false;

	void Awake() {
		Application.targetFrameRate = 60;
		this.GetComponent<Tutorial>().enabled = false;

		audioManager = GameObject.FindGameObjectWithTag ("FMOD_Manager");

        // Set audio levels to mute if the game is muted
        if (PlayerPrefs.GetInt("AudioEnabled") == 0) {
            foreach (AudioSource child in audioManager.transform.GetComponentsInChildren<AudioSource>()) {
                child.volume = 0.0f;
            }
        }
	}

	// Use this for initialization
	void Start () {
		gameState = this.GetComponent<LevelLost>();	

		environmentCentre = GameObject.FindGameObjectWithTag("EnvironmentCentre").transform;
		rotationSound = GameObject.Find("Rotation Sound");

		GAStuff = GameObject.FindGameObjectWithTag("GameAnalytics").GetComponent<GameAnalytics>();

        if (PlayerPrefs.GetInt("TutorialComplete") == 0) {
            momentum = -6.8f;
        } else {
            momentum = -6.8f;
        }

		controlMethod = PlayerPrefs.GetInt ("Control Method");
		
		pause = this.GetComponent<DebugControls>();

		// Start the game with the menu when the game launches
		if (PlayerPrefs.GetInt("FirstLaunch") == 0) {
			Application.LoadLevelAdditive("MenuScreen");
            PlayerPrefs.SetInt("FacebookInitialised", 0);
			PlayerPrefs.SetInt("FirstLaunch", 1);

			pause.paused = true;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {	
				// Update the speed of the firefly particles
				fireflyParticles.startSpeed = Random.Range (particleMinSpeed * forwardSpeed, particleMaxSpeed * forwardSpeed);

				if (PlayerPrefs.GetInt ("TutorialComplete") == 0 && !pause.paused && !tutorialActivated) {
						this.GetComponent<Tutorial> ().enabled = true;
						tutorialActivated = true;
						//PlayerPrefs.SetInt("TutorialComplete", 1);
				}

				if (!gameState.lost && !pause.paused) {

						if (!flyIn) {
								transform.Translate (0, 0, 0);
								// Move the plane forward
								//this.transform.Translate(Vector3.forward * forwardSpeed);
								// Rotate the environment around the player
								environmentCentre.transform.Rotate (Vector3.left * forwardSpeed / 4.0f);
								//audioManager.GetComponent<FMOD_Manager>().WindRotation(momentum);
								PlaneRotation ();
								if (!gameInitliased) {
										gameInitliased = true;
										
								}
						} else {
								if (!initialiseAudio) {
										audioManager.GetComponent<FMOD_Manager> ().ResumeGame ();
										audioManager.GetComponent<FMOD_Manager> ().ForestSetDeath (false);
										initialiseAudio = true;
										//momentum = maxMomentum * 0.75f;
								}
								PlaneRotation ();
								transform.position = Vector3.Lerp (transform.position, new Vector3 (0, 0, 0), 0.05f);
								environmentCentre.transform.eulerAngles = Vector3.Lerp (environmentCentre.transform.rotation.eulerAngles, 
				                                                       new Vector3 (environmentCentre.transform.rotation.eulerAngles.x - 2, 
				                                                                   environmentCentre.transform.rotation.eulerAngles.y, 
				            														environmentCentre.transform.rotation.eulerAngles.z), 0.05f);
								if (transform.position.x < 0.5f && transform.position.z > -0.5f) {
										//transform.position = new Vector3(0,0,0);
										flyIn = false;
										transform.GetComponent<SpawnGates> ().flyIn = false;
								}
						}
				}

				if (pause.paused) {
						momentum = 0.0f;
				} else {
						Controls ();
		
						if (speedIncreaseTimer >= speedIncreaseRate) {
								IncreaseMovement (ref speedIncrementor);
								speedIncreaseTimer = 0.0f;
						} else {
								speedIncreaseTimer += Time.deltaTime;
						}

						if (Mathf.Abs (momentum) > maxMomentum) {
								momentum = maxMomentum * Mathf.Clamp (momentum, -1, 1);
						}

						// Calculate the player's average momentum
						avgMomentum += momentum;
						avgMomentum /= Time.time;

						if (gameState.lost) {
								GAStuff.SetAverageMomentum (avgMomentum);
						}
				}
		}
	
	private void PlaneRotation() {	
		if (!pause.paused) {
			this.transform.Rotate(Vector3.forward * momentum);

			// Play audio for rotation relative to the momentum of the plane
			//rotationSound.GetComponent<AudioSource>().volume = Mathf.Clamp(Mathf.Abs(momentum), 0.0f, 1.0f);
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
//				print (momentum);
				if (Input.mousePosition.x < Screen.width / 2.0f && Input.mousePosition.x > 0) {
				if (triggerLeft) {
						audioManager.GetComponent<FMOD_Manager>().rotateUltraExtreme();
						//print ("trigger");

					}
					triggerLeft = false;

					if (momentum >= 6) {
						triggerRight = true;

					}
					else {
						triggerRight = false;
					}

					if (momentum >= 0.0f) {
						momentum += Time.deltaTime * rotationSpeed;
					} else {
						momentum += Time.deltaTime * rotationSpeed * oppositeDirectionPush;
					}

                    planeAnim.SetBool("RotatingRight", false);
                    planeAnim.SetBool("RotatingLeft", true);
					
				} else if (Input.mousePosition.x > Screen.width/2.0f && Input.mousePosition.x < Screen.width) {
					if (triggerRight) {
						audioManager.GetComponent<FMOD_Manager>().rotateUltraExtreme();
						//print ("trigger");

					}
					triggerRight = false;
					if (momentum <= -6) {
							triggerLeft = true;
						//audioManager.GetComponent<FMOD_Manager>().rotateUltraExtreme();
					}
						else {
							triggerLeft = false;
						}
					if (momentum <= 0.0f) {
						momentum -= Time.deltaTime * rotationSpeed;
					} else {
						momentum -= Time.deltaTime * rotationSpeed * oppositeDirectionPush;
					}
                    


                    planeAnim.SetBool("RotatingLeft", false);
                    planeAnim.SetBool("RotatingRight", true);
				}
				
				levelingTimer = 0.0f;
				
			} else {
				if (triggerLeft) {
					if (momentum > -6.5f) {
						triggerLeft = false;
					}
				}

				if (triggerRight) {
					if (momentum < 6.5f) {
						triggerRight = false;
					}
				}
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

                planeAnim.SetBool("RotatingLeft", false);
                planeAnim.SetBool("RotatingRight", false);

			}
			
			break;
		case 2:
			// Rotate the plane by sliding on the screen
			if (Input.touches.Length > 0) {
				if (Input.touches[0].phase == TouchPhase.Began) {
					// Store the starting touch position
					slideTouchPos = Input.touches[0].position;
					levelingTimer = 0.0f;
				} else if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Stationary) {
					// Rotate the plane based off of the new touch position
					if (Input.touches[0].position.x < slideTouchPos.x + deadZone) {
						if (momentum >= 0.0f) {
							momentum += Time.deltaTime * rotationSpeed;
						} else {
							momentum += Time.deltaTime * rotationSpeed;// * oppositeDirectionPush;
						}
					} else if (Input.touches[0].position.x > slideTouchPos.x - deadZone){
						if (momentum <= 0.0f) {
							momentum -= Time.deltaTime * rotationSpeed;
						} else {
							momentum -= Time.deltaTime * rotationSpeed;// * oppositeDirectionPush;
						}
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
			}
			break;
		case 3:
			// Rotate the plane using tilt controls
			if (Input.acceleration.x > tiltDeadZone) {
				if (momentum <= 0.0f) {
					momentum -= Time.deltaTime * rotationSpeed;
				} else {
					momentum -= Time.deltaTime * rotationSpeed * oppositeDirectionPush;
				}

				levelingTimer = 0.0f;
			} else if (Input.acceleration.z < -tiltDeadZone){
				if (momentum >= 0.0f) {
					momentum += Time.deltaTime * rotationSpeed;
				} else {
					momentum += Time.deltaTime * rotationSpeed * oppositeDirectionPush;
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
		default:
			Debug.LogError("Invalid Control Scheme");
			break;
		}
	}
	
	public void IncreaseMovement(ref float incrementor) {
		if (PlayerPrefs.GetInt("TutorialComplete") == 1) {
			forwardSpeed += incrementor;
			incrementor *= speedGrowthPerStep;
			
			if (rotationSpeed < maxRotationSpeed) {
				rotationSpeed += incrementor;
			} else {
				rotationSpeed = maxRotationSpeed;
			}
		}
	}
}
