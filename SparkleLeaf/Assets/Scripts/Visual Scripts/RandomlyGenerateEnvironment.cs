﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomlyGenerateEnvironment : MonoBehaviour {
	// Declare variables
	[SerializeField] GameObject plane;
	[SerializeField] int numPlanes = 20;
	[SerializeField] float rotationVariation = 0.5f;
	[SerializeField] float environmentChance = 0.5f;
	[SerializeField] int maxNumElements = 3;
	[SerializeField] Texture[] textures;
	[SerializeField] Transform[] environmentModels;

	// Variables to control spawning of environmental objects
	[SerializeField] float minXRange = 10.0f, maxXRange = 45.0f;
	[SerializeField] float minYRange = 5.0f, maxYRange = 7.0f;
	
	private Transform centre;
	private Transform player;
	public List<Transform> spawnPoints;
	
	void Awake() {
		// Initialise variables
		player = GameObject.FindGameObjectWithTag("Player").transform;
		centre = this.transform;

		spawnPoints = new List<Transform>();
	}
	
	// Use this for initialization
	void Start () {
		// Orientate the centre position accordingly
		centre.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - plane.transform.localScale.z * 4.5f, player.transform.position.z);
	
		for (int i = 0; i < numPlanes; i++) {
			// Rotate the centre around before spawning the next piece
			centre.Rotate(Vector3.left, (360.0f / numPlanes) + Random.Range(-rotationVariation, rotationVariation));
			
			// Get a starting point
			Vector3 spawnPoint = new Vector3(centre.position.x, centre.position.y + plane.transform.localScale.z * 4.0f, centre.position.z);
			// Get the starting rotation
			Vector3 startingRotation = new Vector3(0,0,0);

			if (i % 2 == 0) {
				startingRotation = new Vector3(-90.0f, -180.0f, 0.0f);
			} else {
				startingRotation = new Vector3(-90.0f, 0, 0.0f);
			}
				// Spawn the object
			GameObject spawnedObj = (GameObject)GameObject.Instantiate(plane, spawnPoint, Quaternion.Euler(startingRotation)); 
			
			// Parent new object to the centre
			spawnedObj.transform.parent = centre;
			
			// Set the textures randomly for the object
			spawnedObj.renderer.material.SetTexture(0, textures[Random.Range(0, textures.Length)]);
			
			SpawnEnvironment(spawnedObj.transform);

			GameObject point = new GameObject();
			point.name = "Monster Spawn Point";
			point.transform.position = player.transform.position;

			point.transform.parent = spawnedObj.transform;

			spawnPoints.Add(point.transform);
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
					environment.position = new Vector3(Random.Range(-maxXRange, -minXRange), Random.Range(minYRange, maxYRange), 0.0f);
					environment.eulerAngles = new Vector3(270.0f, 180.0f, Random.Range (0, 180));
				} else {
					environment.position = new Vector3(Random.Range(minXRange, maxXRange), Random.Range(minYRange, minYRange), 0.0f);
					environment.eulerAngles = new Vector3(270.0f, 180.0f, Random.Range (180,360));
				}
				
				environment.localScale = new Vector3(7.0f, 7.0f, 7.0f);
				
				environment.parent = centre;
			}
		}
	}
}
