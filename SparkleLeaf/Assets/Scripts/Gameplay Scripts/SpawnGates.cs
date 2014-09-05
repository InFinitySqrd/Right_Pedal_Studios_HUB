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

	// Declare variables	
    [SerializeField] int numRotations = 360;
	[SerializeField] Transform[] obstacles;

    // Colour change variables
    [SerializeField] float closeRotation = 5.0f;
    [SerializeField] float colourChangeRate = 4.0f;
	
    private bool[] rotatable;

	private float timer = 0.0f;
	private Transform spawnedObstacle;
	private List<Transform> gatesList;
	private Color planeColor;
	private Material planeBodyColor;
	
	private PlaneMovement planeVars;
	private DebugControls pause;
	private LevelLost lostGame;

	[SerializeField] bool RightSideUpScore = false;
	private bool crossAvailable = false;

	private GameAnalytics GAStuff;
	private int doubleScore = 0;

    private int spawnedGateNum = 0;

    // Incremental difficulty variables
    private float randomRotationRangeTimer = 0.0f;
    private int gatesAbleToSpawn = 1;
    private float addNewGateTimer = 0.0f;
    private bool startRotationTimer = true;
    private float rotatableTimer = 0.0f;
	
	void Awake() {
		gatesList = new List<Transform>();
		planeVars = this.GetComponent<PlaneMovement>();
		pause = this.GetComponent<DebugControls>();
		lostGame = this.GetComponent<LevelLost>();

		GAStuff = GameObject.FindGameObjectWithTag("GameAnalytics").GetComponent<GameAnalytics>();
	}
	
	// Use this for initialization
	void Start () {
		timer = spawnTime;
		planeColor = this.renderer.material.color;
		
        // Initialise the array of booleans to tell if an object can be rotated
        rotatable = new bool[obstacles.Length];
        for (int i = 0; i < rotatable.Length; i++) {
            rotatable[i] = false;
        }
	}
	
	void OnGUI() {
		// Draw the player's score in the top corner of the screen
		GUIStyle skin = new GUIStyle();
		skin.fontSize = 40;
		skin.alignment = TextAnchor.MiddleCenter;
        skin.normal.textColor = Color.white;
		GUI.Box(new Rect(0.0f, 0.0f + Screen.height / 8.0f, Screen.width / 4.0f, Screen.height / 8.0f), score.ToString(), skin);
	}
	
	// Update is called once per frame
	void Update () {
        if (lostGame.lost) {
            if (score > PlayerPrefs.GetInt("Top Score")) {
                PlayerPrefs.SetInt("Top Score", score);
            }
        } else if (PlayerPrefs.GetInt("Tutorial Complete") == 1){
            // Counters to increase the difficulty of the game over time
            IncreaseRandomRotationRange();
            IncreaseGatesToBeSpawned();
            SetNextObstacleToBeRotatable();
        }

		int gateNumber = 0;

		if (gatesList.Count > 0) {
			//RotationColour();
		}

        // Spawn a gate if we are able to
		if (PlayerPrefs.GetInt("Tutorial Completed") == 1 && timer >= spawnTime && !pause.paused && !lostGame.lost) {
            // Grab a spawn point
			Vector3 spawnPoint = new Vector3(this.transform.position.x, planeVars.environmentCentre.position.y / 16.0f, this.transform.position.z + spawnDistance);
			
			float randomNumber = Random.value;
			float summedProbabilities = 0.0f;
			
			for (int i = 0; i < gatesAbleToSpawn; i++) {
				summedProbabilities += (1.0f / (float)gatesAbleToSpawn);
				
                // Spawn a random gate
				if (randomNumber < summedProbabilities) {
					spawnedObstacle = (Transform)Instantiate(obstacles[i], spawnPoint, Quaternion.identity);
					spawnedGateNum = i;
                    gateNumber = i + 1;
					break;
				}
			}
			
            // Add the obstacle to the list of gates that are currently active
			gatesList.Add(spawnedObstacle);
			
            // Set the rotation of the gate
			int randomNotch = (int)Random.Range(0, numRotations);
			spawnedObstacle.transform.eulerAngles = new Vector3(spawnedObstacle.transform.eulerAngles.x, spawnedObstacle.transform.eulerAngles.y, randomNotch * (360.0f / numRotations));
			
            // Move the gate towards the player
            spawnedObstacle.gameObject.AddComponent<MovingGates>();
			spawnedObstacle.GetComponent<MovingGates>().gateType = gateNumber;
			
            // Determine if this object should be rotating or not
			if (rotatable[spawnedGateNum]) {
				float randomNum = Random.value;
				
				if (randomNum <= percentageToRotate) {
					spawnedObstacle.gameObject.AddComponent<RotateObstacle>();
                    spawnedObstacle.GetComponent<RotateObstacle>().SetRotationSpeed(Random.Range(minSpeed, maxSpeed));
				}
			}
			
			timer = 0.0f;
		} else if (!pause.paused){
			timer += Time.deltaTime * planeVars.forwardSpeed;
		}
		
        // Determine the score to be added to the player's total
		if ((gatesList.Count > 0) && this.transform.position.z > gatesList[0].position.z) {
			Vector3 playerRotation = this.transform.rotation.eulerAngles;
			score++;
			GAStuff.SetScore(score);
			gatesList.RemoveAt(0);
		}
	}
	
	private void RotationColour() {
        // Colour the plane if the plane matches the rotation of the obstacle
		if (Quaternion.Angle(this.transform.rotation, gatesList[0].transform.rotation) <= closeRotation ||
		    Quaternion.Angle(this.transform.rotation, gatesList[0].transform.rotation) >= 180.0f - closeRotation) {
			this.renderer.material.color = Color.Lerp(this.renderer.material.color, Color.green, Time.deltaTime * colourChangeRate);
			planeBodyColor.color = this.renderer.material.color;
		} else {
			this.renderer.material.color = Color.Lerp(this.renderer.material.color, planeColor, Time.deltaTime * colourChangeRate * 4.0f);
			planeBodyColor.color = this.renderer.material.color;
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
            if (rotatableTimer >= rotationSpeedIncreaseTime) {
                rotatable[gatesAbleToSpawn - 1] = true;

                rotatableTimer = 0.0f;
                startRotationTimer = false;
            } else {
                rotatableTimer += Time.deltaTime;
            }
        }
    }
}
