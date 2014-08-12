using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnGates : MonoBehaviour {
	[SerializeField] int numRotations = 10;
	[SerializeField] Transform gate;
	[SerializeField] float spawnTime = 6.0f;
	[SerializeField] float spawnDistance = 10.0f;
	private float timer = 0.0f;
	private Transform spawnedGate;
	private List<Transform> gatesList;
	private int score = 0;
	
	void Awake() {
		gatesList = new List<Transform>();
	}
	
	// Use this for initialization
	void Start () {
		timer = spawnTime;
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
		if (timer >= spawnTime) {
			Vector3 spawnPoint = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + spawnDistance);
			spawnedGate = (Transform)Instantiate(gate, spawnPoint, Quaternion.identity);
			
			gatesList.Add(spawnedGate);
			
			int randomNotch = (int)Random.Range(0, numRotations);
			spawnedGate.transform.eulerAngles = new Vector3(spawnedGate.transform.eulerAngles.x, spawnedGate.transform.eulerAngles.y, randomNotch * (360.0f / numRotations));
			
			timer = 0.0f;
		} else {
			timer += Time.deltaTime;
		}
		
		if (this.transform.position.z > gatesList[0].position.z) {
			score++;
			gatesList.RemoveAt(0);
		}
	}
}
