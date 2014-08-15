using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnGates : MonoBehaviour {
	[SerializeField] int numRotations = 10;
	[SerializeField] Transform gate;
	[SerializeField] float spawnTime = 6.0f;
	[SerializeField] float spawnDistance = 10.0f;
	[SerializeField] float closeRotation = 5.0f;
	[SerializeField] float colourChangeRate = 4.0f;
	private float timer = 0.0f;
	private Transform spawnedGate;
	private List<Transform> gatesList;
	private int score = 0;
	private Color planeColor;
	private Material planeBodyColor;
	
	void Awake() {
		gatesList = new List<Transform>();
	}
	
	// Use this for initialization
	void Start () {
		timer = spawnTime;
		planeColor = this.renderer.material.color;
		planeBodyColor = this.transform.GetChild(0).renderer.material;
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
		if (gatesList.Count > 0) {
			RotationColour();
		}
	
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
		
		if ((gatesList.Count > 0) && this.transform.position.z > gatesList[0].position.z) {
			score++;
			gatesList.RemoveAt(0);
		}
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
