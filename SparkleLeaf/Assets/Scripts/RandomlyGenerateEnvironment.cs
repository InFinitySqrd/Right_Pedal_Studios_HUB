using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomlyGenerateEnvironment : MonoBehaviour {
	// Declare variables
	[SerializeField] GameObject plane;
	[SerializeField] int numPlanes = 20;
	[SerializeField] float minDist = 1.0f, maxDist = 2.0f;
	[SerializeField] float environmentChance = 0.5f;
	[SerializeField] int maxNumElements = 3;
	[SerializeField] Texture[] textures;
	[SerializeField] Transform[] environmentModels;
	
	private Transform centre;
	private Transform player;
	
	void Awake() {
		// Initialise variables
		player = GameObject.FindGameObjectWithTag("Player").transform;
		centre = this.transform;
	}
	
	// Use this for initialization
	void Start () {
		// Orientate the centre position accordingly
		centre.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - plane.transform.localScale.z * 4.5f, player.transform.position.z);
	
		for (int i = 0; i < numPlanes; i++) {
			// Rotate the centre around before spawning the next piece
			centre.Rotate(Vector3.left, Random.Range (minDist, maxDist));
			
			// Get a starting point
			Vector3 spawnPoint = new Vector3(centre.position.x, centre.position.y + plane.transform.localScale.z * 4.0f, centre.position.z);
			// Get the starting rotation
			Vector3 startingRotation = new Vector3(0,0,0);

			if (i % 2 == 0) {
			 startingRotation = new Vector3(-90.0f, -180.0f, 0.0f);
			}

			else {
				 startingRotation = new Vector3(-90.0f, 0, 0.0f);
			}
				// Spawn the object
			GameObject spawnedObj = (GameObject)GameObject.Instantiate(plane, spawnPoint, Quaternion.Euler(startingRotation)); 
			
			// Parent new object to the centre
			spawnedObj.transform.parent = centre;
			
			// Set the textures randomly for the object
			spawnedObj.renderer.material.SetTexture(0, textures[Random.Range(0, textures.Length)]);
			
			SpawnEnvironment(spawnedObj.transform);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	
	private void SpawnEnvironment(Transform planeSpawned) {
		int numPasses = Random.Range (1, maxNumElements);
	
		for (int i = 0; i <= numPasses; i++) {
			if (Random.value <= environmentChance) {
				Transform environment = (Transform)GameObject.Instantiate(environmentModels[Random.Range(0, environmentModels.Length)], planeSpawned.position, planeSpawned.rotation);

				//environment.eulerAngles = new Vector3(270.0f, 180.0f, Random.Range(0.0f, 360.0f));
				
				if (Random.value > 0.5f) {
					environment.position = new Vector3(Random.Range(-45.0f, -10.0f), Random.Range(-7.0f,-5.0f), 0.0f);
					environment.eulerAngles = new Vector3(270.0f, 180.0f, Random.Range (0, 180));
				} else {
					environment.position = new Vector3(Random.Range(10.0f, 45.0f), Random.Range(-7.0f,-5.0f), 0.0f);
					environment.eulerAngles = new Vector3(270.0f, 180.0f, Random.Range (180,360));
				}
				
				environment.localScale = new Vector3(7.0f, 7.0f, 7.0f);
				
				environment.parent = centre;
			}
		}
	}
}
