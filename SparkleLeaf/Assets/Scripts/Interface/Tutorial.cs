﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour {
    // Declare variables
    [SerializeField] Transform tutorialIcons;
    [SerializeField] Transform tutorialText;
    [SerializeField] float minHoldTime = 1.0f;
    [SerializeField] float delayToPlay = 1.0f;
    [SerializeField] float fadeRate = 0.2f;

    private float leftTimer = 0.0f, rightTimer = 0.0f, timer;
    private GameObject leftIcon, rightIcon, text;
    private bool leftAchieved, rightAchieved;

	private RandomlyGenerateEnvironment enviroVars;
	private SpawnGates setStartingPoint;

	private bool instantiated = false;

	private float tutorialTimer = 0.0f;
	private bool lerpUp = true;
	private Vector3 tutorialLowerBound = new Vector3(1.5f,1.5f,1.5f);
	private Vector3 tutorialUpperBound = new Vector3(2f,2f,2f);
	private Vector3 tutorialHeldUpperBound = new Vector3 (4f, 4f, 4f);
	private Vector3 tutorialFinishedUpperBound = new Vector3(8f,8f,8f);

	private bool rightHeld, leftHeld;

    private GooglePlayIntegration googlePlay;

	// Use this for initialization
	void Start () {
		enviroVars = GameObject.FindGameObjectWithTag("EnvironmentCentre").GetComponent<RandomlyGenerateEnvironment>();
		setStartingPoint = this.GetComponent<SpawnGates>();
    
        googlePlay = Camera.main.GetComponent<GooglePlayIntegration>();

		if (PlayerPrefs.GetInt("TutorialComplete") == 0 && !instantiated) {
			InstantiateTuteVars();
			instantiated = true;
		} else {
			this.enabled = false;
		}

		rightIcon.transform.localScale = tutorialLowerBound;
		leftIcon.transform.localScale = tutorialLowerBound;
	}
	// Update is called once per frame
	void Update () {

		if (lerpUp) {
			if (!leftHeld && !leftAchieved) {
				// Pulse circles up
				leftIcon.transform.localScale = Vector3.Lerp(leftIcon.transform.localScale, tutorialUpperBound, 0.05f);
			}
			
			if (!rightHeld && !rightAchieved) {
				rightIcon.transform.localScale = Vector3.Lerp(rightIcon.transform.localScale, tutorialUpperBound, 0.05f);
			}
		}
		
		else {
			if (!leftHeld && !leftAchieved) {
				leftIcon.transform.localScale = Vector3.Lerp(leftIcon.transform.localScale, tutorialLowerBound, 0.1f);
			}
			
			if (!rightHeld && !rightAchieved) {
				rightIcon.transform.localScale = Vector3.Lerp(rightIcon.transform.localScale, tutorialLowerBound, 0.1f);
			}
		}
		
		tutorialTimer += Time.deltaTime;
		if (tutorialTimer >= 1.5f && lerpUp) {
			lerpUp= false;
			tutorialTimer = 0;
		}
		
		if (tutorialTimer >= 0.75f && !lerpUp) {
			lerpUp = true;
			tutorialTimer = 0;
		}


        // Determine when the player has completed the tutorial
        if (Input.GetMouseButton (0)) {
			if (Input.mousePosition.x > Screen.width / 2.0f) {
				rightTimer += Time.deltaTime;
				rightHeld = true;
				rightIcon.transform.localScale = Vector3.Lerp(rightIcon.transform.localScale, tutorialHeldUpperBound, 0.015f);

				if (rightTimer > minHoldTime) {
					rightAchieved = true;
				}

				leftHeld = false;
			} 

			else {
				leftTimer += Time.deltaTime;
				leftHeld = true;
				leftIcon.transform.localScale = Vector3.Lerp(leftIcon.transform.localScale, tutorialHeldUpperBound, 0.015f);

				if (leftTimer > minHoldTime) {
					leftAchieved = true;
				}

				rightHeld = false;
			}
		} 
		else {
			rightHeld = false;
			leftHeld = false;
			leftTimer = 0.0f;
			rightTimer = 0.0f;
			}

        if (rightAchieved) {
            StartCoroutine(Fade(rightIcon.renderer.material));
			rightIcon.transform.localScale = Vector3.Lerp(rightIcon.transform.localScale, tutorialFinishedUpperBound, 0.085f);
        }

        if (leftAchieved) {
            StartCoroutine(Fade(leftIcon.renderer.material));
			leftIcon.transform.localScale = Vector3.Lerp(leftIcon.transform.localScale, tutorialFinishedUpperBound, 0.085f);
        }

        if (leftAchieved && rightAchieved) {
            if (timer >= delayToPlay) {
                StartCoroutine(Fade(text.renderer.material));
            } else {
                timer += Time.deltaTime;
            }

            if (text.renderer.material.color.a < 0.1f) {
                PlayerPrefs.SetInt("TutorialComplete", 1);

                googlePlay.UnlockAchievement("TutorialComplete");

                Destroy(leftIcon);
                Destroy(rightIcon);
                Destroy(text);

				// Find the nearest monster spawn point to the player
				List<Transform> spawnPoints = enviroVars.spawnPoints;
				float distanceValue = 100.0f;
				int pointNumber = 0;

				foreach (Transform point in spawnPoints) {
					if (Vector3.Distance(point.position, this.transform.position) <= distanceValue) {
						distanceValue = Vector3.Distance(point.position, this.transform.position);
						pointNumber = spawnPoints.IndexOf (point);
					}
				}

				setStartingPoint.currentSpawnPoint = pointNumber + setStartingPoint.startingSpawnPoint;

                this.GetComponent<Tutorial>().enabled = false;
			}
        }
	}

	private void InstantiateTuteVars() {
		// Spawn hold points
		for (int i = -1; i < 2; i += 2) {
			// Set up an appropriate spawn point
			Vector3 spawnPoint = new Vector3(Screen.width / 2.0f + (i * Screen.width / 3.0f), Screen.height / 8.0f, -Camera.main.transform.position.z);
			spawnPoint = Camera.main.ScreenToWorldPoint(spawnPoint);
			
			// Instantiate the icon
			if (i == -1) {
				leftIcon = (GameObject)Instantiate(tutorialIcons.gameObject, spawnPoint, tutorialIcons.transform.rotation);
			} else if (i == 1) {
				rightIcon = (GameObject)Instantiate(tutorialIcons.gameObject, spawnPoint, tutorialIcons.transform.rotation);
			}
		}
		
		// Spawn tutorial text
		// Set up an appropriate spawn point
		Vector3 transform = new Vector3(Screen.width / 2.0f, Screen.height - Screen.height / 3.0f, -Camera.main.transform.position.z);
		transform = Camera.main.ScreenToWorldPoint(transform);

		/// No tutorial text in this game!
		// Instantiate the text
		text = (GameObject)Instantiate(tutorialText.gameObject, transform, tutorialIcons.transform.rotation);
	}

    IEnumerator Fade(Material transparentMat) {
        while (transparentMat.color.a > 0.0f) {
            transparentMat.color = new Color(transparentMat.color.r, transparentMat.color.g, transparentMat.color.b, transparentMat.color.a - Time.deltaTime * fadeRate);
            yield return null;
        }
    }
}
