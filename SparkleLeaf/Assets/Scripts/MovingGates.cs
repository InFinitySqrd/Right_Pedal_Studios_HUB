﻿using UnityEngine;
using System.Collections;

public class MovingGates : MonoBehaviour {
	// Declare public variables
	public int gateType = 0;	// 1 = Gate, 2 = Cross
	public string gateName;

	// Declare variables
	private PlaneMovement planeVars;
	private LevelLost lostGame;
	private Transform player;
	private float movementSpeed;
	private bool atPlayer = false;
	
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		planeVars = player.gameObject.GetComponent<PlaneMovement>();
		lostGame = player.gameObject.GetComponent<LevelLost>();
		movementSpeed = planeVars.forwardSpeed;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (gateType != 0) {
			switch (gateType) {
				case 1:
					gateName = "Gate";
					break;
				case 2:
					gateName = "Cross";
					break;
				default:
					Debug.LogError("Incorrect Gate Type");
					break;
			}
		}

		if (this.transform.position == player.transform.position) {
			atPlayer = true;
		}

		if (!lostGame.lost) {
			if (!atPlayer ) {	
				this.transform.position = Vector3.MoveTowards(this.transform.position, player.position, movementSpeed);
			} else {
				this.transform.Translate(Vector3.back * movementSpeed);	
			}
		}
		
		if (this.transform.eulerAngles.x > 0.05f) {
			this.transform.Rotate(Vector3.forward * movementSpeed);
		}
	}
}
