using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnGates : MonoBehaviour {
	// Declare public variables
	public int score = 0;
    public float spawnTime = 6.0f;
    public float spawnDistance = 10.0f;
    public float newObstacleTime = 20.0f;
    public float rotateNewObstacleTime = 10.0f;
    public float percentageToRotate = 0.5f;
    public float minSpeed = 0.2f;
    public float maxSpeed = 1.4f;    
    public float randomRotationSpeedIncrementor = 0.4f;
    public float rotationSpeedIncreaseTime = 10.0f;
	public int spawnSpacing = 5;

	// Declare variables	
	[SerializeField] int minRotation = 0, maxRotation = 360;
	[SerializeField] Transform[] obstacles;

    // Colour change variables
    [SerializeField] float closeRotation = 5.0f;
    [SerializeField] float colourChangeRate = 4.0f;
	
    private bool[] rotatable;

	private float timer = 0.0f;
	private Transform spawnedObstacle;
	private List<Transform> gatesList;
	private Color planeColor;
	//private Material planeBodyColor;
	
	private PlaneMovement planeVars;
	private DebugControls pause;
	private LevelLost lostGame;

	//[SerializeField] bool RightSideUpScore = false;
	//private bool crossAvailable = false;

	private GameAnalytics GAStuff;
	//private int doubleScore = 0;

    private int spawnedGateNum = 0;

    // Incremental difficulty variables
    private float randomRotationRangeTimer = 0.0f;
    private int gatesAbleToSpawn = 1;
    private float addNewGateTimer = 0.0f;
    private bool startRotationTimer = true;
    private float rotatableTimer = 0.0f;
	private float decSpawnSpacingTimer = 0.0f;
	[SerializeField] float decSpawnTime = 0.0f;
	private bool tutorialFinished = false;

	private RandomlyGenerateEnvironment enviroVars;
	public int startingSpawnPoint = 20;
	public int currentSpawnPoint = 0;

	[SerializeField] Font scoreFont;

    private int gateNumber = 0;

    // Fly through audio object
    private AudioSource flyThroughSound;

    private GameObject audioManager;

    // UI Things
    float nativeWidth = 1920.0f;
	float nativeHeight = 1080.0f;
    private int scoreTranslation = 0, scoreThreshold;
    private int translationSpeed = 5;
    private bool scoreUp = true;

	void Awake() {
		gatesList = new List<Transform>();
		planeVars = this.GetComponent<PlaneMovement>();
		pause = this.GetComponent<DebugControls>();
		lostGame = this.GetComponent<LevelLost>();
		enviroVars = GameObject.FindGameObjectWithTag("EnvironmentCentre").GetComponent<RandomlyGenerateEnvironment>();

		GAStuff = GameObject.FindGameObjectWithTag("GameAnalytics").GetComponent<GameAnalytics>();
		currentSpawnPoint = 0;

        flyThroughSound = GameObject.Find("FlyThrough Sound").GetComponent<AudioSource>();
	}
	
	// Use this for initialization
	void Start () {
		timer = 0.0f;
		planeColor = this.renderer.material.color;
		
        // Initialise the array of booleans to tell if an object can be rotated
        rotatable = new bool[obstacles.Length];
        for (int i = 0; i < rotatable.Length; i++) {
            rotatable[i] = false;
        }
		audioManager = GameObject.FindGameObjectWithTag ("FMOD_Manager");
        currentSpawnPoint = startingSpawnPoint;
	}

	void OnGUI() {
		// Draw the player's score in the top corner of the screen
		if (PlayerPrefs.GetInt("TutorialComplete") == 1 && !pause.paused) {
			// Get a scaling factor based off of the native resolution and preset resolution
			float rx = Screen.width / nativeWidth;
			float ry = Screen.height / nativeHeight;
			
			// Scale width the same as height - cut off edges to keep ratio the same
			GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(ry, ry, 1));
			
			// Get width taking into account edges being cut off or extended
			float adjustedWidth = nativeWidth * (rx / ry);

			GUIStyle skin = new GUIStyle();
			skin.font = scoreFont;
			skin.fontSize = 72;
			skin.alignment = TextAnchor.MiddleCenter;
	        skin.normal.textColor = Color.white;


			GUI.Box(new Rect(0.0f, scoreTranslation + 0.0f + nativeHeight / 7, adjustedWidth, nativeHeight / 8.0f), score.ToString(), skin);
		}
	}
    

	
	// Update is called once per frame
	void Update () {
        if (lostGame.lost) {
            if (score > PlayerPrefs.GetInt("Top Score")) {
                PlayerPrefs.SetInt("Top Score", score);
            }
        } else if (PlayerPrefs.GetInt("TutorialComplete") == 1){
            // Counters to increase the difficulty of the game over time
            IncreaseRandomRotationRange();
            IncreaseGatesToBeSpawned();
            SetNextObstacleToBeRotatable();
        }

		if (gatesList.Count > 0) {
			//RotationColour();
		}


        // Spawn a gate if we are able to
		if (PlayerPrefs.GetInt("TutorialComplete") == 1 && timer >= spawnTime && !pause.paused && !lostGame.lost) {
            SpawnNewGate();
	        SetSpawnPosition();
            AddNecessaryComponents();
			
			timer = 0.0f;

		

		} else if (!pause.paused){
			timer += Time.deltaTime * planeVars.forwardSpeed;

		}

        // Determine the score to be added to the player's total
		if ((gatesList.Count > 0) && this.transform.position.z > gatesList[0].position.z) {
			
			score++;

            scoreTranslation = 0;
            scoreThreshold = (int)(Screen.height / 80.0f);
            scoreUp = true;
            StartCoroutine(JiggleScoreNumber());
			
            GameObject currentGate = gatesList[0].gameObject;
			gatesList.RemoveAt(0);

			currentGate.GetComponent<turnOffLight>().DestroyLight();
            //flyThroughSound.pitch = Random.Range(0.9f, 1.1f);
            //flyThroughSound.Play();

			StartCoroutine(DestroyGate(currentGate));

            audioManager.GetComponent<FMOD_Manager>().SetGameplayTime(score);

			GAStuff.SetScore(score);
		}
	}

    private void SpawnNewGate() {
        //Grab a spawn point
	    Vector3 spawnPoint = new Vector3(50, 50, 50);
			
		float randomNumber = Random.value;
		float summedProbabilities = 0.0f;
		
		for (int i = 0; i < gatesAbleToSpawn; i++) {
			summedProbabilities += (1.0f / (float)gatesAbleToSpawn);
			
            // Spawn a random gate
			if (randomNumber < summedProbabilities) {
				spawnedObstacle = (Transform)Instantiate(obstacles[i], spawnPoint, obstacles[i].rotation);
				spawnedGateNum = i;
                gateNumber = i + 1;
				break;
			}
		}
			
        // Add the obstacle to the list of gates that are currently active
		gatesList.Add(spawnedObstacle);

        // Set the rotation of the gate
		int randomNotch = (int)Random.Range(minRotation, maxRotation);
		spawnedObstacle.transform.eulerAngles = new Vector3(spawnedObstacle.transform.eulerAngles.x, spawnedObstacle.transform.eulerAngles.y, spawnedObstacle.transform.eulerAngles.z + randomNotch);
			
    }

    private void SetSpawnPosition() {
        List<Transform> spawnPoints = enviroVars.spawnPoints;
		float distanceValue = 100.0f;
		int pointNumber = 0;

		foreach (Transform point in spawnPoints) {
			if (Vector3.Distance(point.position, this.transform.position) <= distanceValue) {
				distanceValue = Vector3.Distance(point.position, this.transform.position);
				pointNumber = spawnPoints.IndexOf (point);
			}
		}

		// Appropriately wrap the incrementor for the enemy spawning
		if (pointNumber + spawnSpacing < enviroVars.spawnPoints.Count) {
			currentSpawnPoint = pointNumber + spawnSpacing;
		} else {
			int difference = (pointNumber + spawnSpacing) - enviroVars.spawnPoints.Count;
			currentSpawnPoint = difference;
		}	
    }

    private void AddNecessaryComponents() {
        // Move the gate towards the player
        spawnedObstacle.gameObject.AddComponent<MovingGates>();
		MovingGates moveVars = spawnedObstacle.GetComponent<MovingGates>();
		moveVars.gateType = gateNumber;
		moveVars.followPoint = enviroVars.spawnPoints[currentSpawnPoint];
		
        // Determine if this object should be rotating or not
		if (rotatable[spawnedGateNum]) {
			float randomNum = Random.value;
			
			if (randomNum <= percentageToRotate) {
				spawnedObstacle.gameObject.AddComponent<RotateObstacle>();
                spawnedObstacle.GetComponent<RotateObstacle>().SetRotationSpeed(Random.Range(minSpeed, maxSpeed));
			}
		}
    }

	private void RotationColour() {
        // Colour the plane if the plane matches the rotation of the obstacle
		if (Quaternion.Angle(this.transform.rotation, gatesList[0].transform.rotation) <= closeRotation ||
		    Quaternion.Angle(this.transform.rotation, gatesList[0].transform.rotation) >= 180.0f - closeRotation) {
			this.renderer.material.color = Color.Lerp(this.renderer.material.color, Color.green, Time.deltaTime * colourChangeRate);
			//planeBodyColor.color = this.renderer.material.color;
		} else {
			this.renderer.material.color = Color.Lerp(this.renderer.material.color, planeColor, Time.deltaTime * colourChangeRate * 4.0f);
			//planeBodyColor.color = this.renderer.material.color;
		}
	}

    private void IncreaseRandomRotationRange() {
        if (randomRotationRangeTimer >= rotationSpeedIncreaseTime) {
            minSpeed += randomRotationSpeedIncrementor;
            maxSpeed += randomRotationSpeedIncrementor;

            randomRotationRangeTimer = 0.0f;
        } else {
            randomRotationRangeTimer += Time.deltaTime;
        }
    }

    private void IncreaseGatesToBeSpawned() {
        if (addNewGateTimer >= newObstacleTime) {
            gatesAbleToSpawn++;

            addNewGateTimer = 0.0f;
            startRotationTimer = true;
        } else {
            addNewGateTimer += Time.deltaTime;
        }
        
        if (gatesAbleToSpawn > obstacles.Length) {
            gatesAbleToSpawn = obstacles.Length;
        }
    }

    private void SetNextObstacleToBeRotatable() {
        if (startRotationTimer) {
            if (rotatableTimer >= rotateNewObstacleTime) {
                rotatable[gatesAbleToSpawn - 1] = true;

                rotatableTimer = 0.0f;
                startRotationTimer = false;
            } else {
                rotatableTimer += Time.deltaTime;
            }
        }
    }

    IEnumerator JiggleScoreNumber() {
        while (scoreThreshold >= 1) {            
            if (!scoreUp) {
                if (scoreTranslation < scoreThreshold) {
                    scoreTranslation += translationSpeed;
                } else {
                    scoreUp = true;
                    scoreThreshold /= 2;
                }
            } else {
                if (scoreTranslation > -scoreThreshold) {
                    scoreTranslation -= translationSpeed;
                } else {
                    scoreUp = false;
                    scoreThreshold /= 2;
                }
            }
            yield return null;
        }
    }

    IEnumerator DestroyGate(GameObject gate) {
        yield return new WaitForSeconds(1.0f);
        Destroy(gate.gameObject);
    }
}
