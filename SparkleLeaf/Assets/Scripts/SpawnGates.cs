using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnGates : MonoBehaviour {
	// Declare public variables
	public int score = 0;
    public float spawnTime = 6.0f;
    public float spawnDistance = 10.0f;
    public float percentageToRotate = 0.5f;
    public float minSpeed = 0.2f;
    public float maxSpeed = 1.4f;

	// Declare variables	
    [SerializeField] int numRotations = 360;
	[SerializeField] Transform[] obstacles;
	[SerializeField] float[] percentageSpawnChance; 
	
	[SerializeField] bool rotatable = true;

    // Colour change variables
    [SerializeField] float closeRotation = 5.0f;
    [SerializeField] float colourChangeRate = 4.0f;
	
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
		
		if (obstacles.Length != percentageSpawnChance.Length) {
			Debug.LogError("Not an equal number of objects and percentages");
		}
		
		float percentage = 0.0f;
		for (int i = 0; i < percentageSpawnChance.Length; i++) {
			percentage += percentageSpawnChance[i];
		}
		
		if (percentage != 1.0f) {
			Debug.LogError("Percentages do not add up to 1.0");
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
		int gateNumber = 0;

		if (gatesList.Count > 0) {
			//RotationColour();
		}
		
        // Spawn a gate if we are able to
		if (timer >= spawnTime && !pause.paused && !lostGame.lost) {
            // Grab a spawn point
			Vector3 spawnPoint = new Vector3(this.transform.position.x, planeVars.environmentCentre.position.y / 16.0f, this.transform.position.z + spawnDistance);
			
			float randomNumber = Random.value;
			float summedProbabilities = 0.0f;
			
			for (int i = 0; i < obstacles.Length; i++) {
				summedProbabilities += percentageSpawnChance[i];
				
                // Spawn a random gate
				if (randomNumber < summedProbabilities) {
					spawnedObstacle = (Transform)Instantiate(obstacles[i], spawnPoint, Quaternion.identity);
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
			if (rotatable) {
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
			if (RightSideUpScore) {
				if (playerRotation.z < 90 || playerRotation.z > 270) {
					score++;
					doubleScore++;
					GAStuff.SetDoubleScores(doubleScore);
				}
			}
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
}
