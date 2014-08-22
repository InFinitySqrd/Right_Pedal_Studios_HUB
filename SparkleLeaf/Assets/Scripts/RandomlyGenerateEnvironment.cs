using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomlyGenerateEnvironment : MonoBehaviour {
	// Declare variables
	[SerializeField] GameObject plane;
	[SerializeField] int numPlanes = 20;
	[SerializeField] float minDist = 1.0f, maxDist = 2.0f;
	[SerializeField] Texture[] textures;
	
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
			Vector3 startingRotation = new Vector3(-90.0f, -180.0f, 0.0f);
			// Spawn the object
			GameObject spawnedObj = (GameObject)GameObject.Instantiate(plane, spawnPoint, Quaternion.Euler(startingRotation)); 
			
			// Parent new object to the centre
			spawnedObj.transform.parent = centre;
			
			// Set the textures randomly for the object
			spawnedObj.renderer.material.SetTexture(0, textures[Random.Range(0, textures.Length)]);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
