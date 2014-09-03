using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnGates : MonoBehaviour {
	// Declare public variables
	public int score = 0;

	// Declare variables
	[SerializeField] int numRotations = 10;
	[SerializeField] float spawnTime = 6.0f;
	[SerializeField] float spawnDistance = 10.0f;
	[SerializeField] float closeRotation = 5.0f;
	[SerializeField] float colourChangeRate = 4.0f;
	
	[SerializeField] Transform[] obstacles;
	[SerializeField] float[] percentageSpawnChance; 
	
	[SerializeField] bool rotatable = true;
	[SerializeField] float percentageToRotate = 0.5f;
	
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
		// Display that the player has lost the game
		GUIStyle skin = new GUIStyle();
		skin.fontSize = 40;
		skin.alignment = TextAnchor.MiddleCenter;
		GUI.Box(new Rect(0.0f, 0.0f + Screen.height / 8.0f, Screen.width / 4.0f, Screen.height / 8.0f), score.ToString(), skin);
	}
	
	// Update is called once per frame
	void Update () {
		int gateNumber = 0;

		if (gatesList.Count > 0) {
			//RotationColour();
		}
		
		if (timer >= spawnTime && !pause.paused && !lostGame.lost) {
			Vector3 spawnPoint = new Vector3(this.transform.position.x, planeVars.environmentCentre.position.y / 16.0f, this.transform.position.z + spawnDistance);
			
			float randomNumber = Random.value;
			float summedProbabilities = 0.0f;
			
			for (int i = 0; i < obstacles.Length; i++) {
				summedProbabilities += percentageSpawnChance[i];
				
				if (randomNumber < summedProbabilities) {
					spawnedObstacle = (Transform)Instantiate(obstacles[i], spawnPoint, Quaternion.identity);
					gateNumber = i + 1;
					break;
				}
			}
			
			gatesList.Add(spawnedObstacle);
			
			int randomNotch = (int)Random.Range(0, numRotations);
			spawnedObstacle.transform.eulerAngles = new Vector3(spawnedObstacle.transform.eulerAngles.x, spawnedObstacle.transform.eulerAngles.y, randomNotch * (360.0f / numRotations));
			spawnedObstacle.gameObject.AddComponent<MovingGates>();
			spawnedObstacle.GetComponent<MovingGates>().gateType = gateNumber;
			
			if (rotatable) {
				float randomNum = Random.value;
				
				if (randomNum <= percentageToRotate) {
					spawnedObstacle.gameObject.AddComponent<RotateObstacle>();
				}
			}
			
			timer = 0.0f;
		} else {
			timer += Time.deltaTime * planeVars.forwardSpeed;
		}
		
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

		/*if (RightSideUpScore) {
			if (playerRotation.z < 90 || playerRotation.z > 270) {

			}
		}*/
	}
	
	private void RotationColour() {
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
