using UnityEngine;
using System.Collections;

public class MaterialLerp : MonoBehaviour {
	// Declare variables
	[SerializeField] Material[] materials;
	[SerializeField] int scoreTrigger = 30;
	[SerializeField] float lerpSpeed = 10.0f;

	private SpawnGates scoreValue;
	private int lerpCounter = 0;

	// Use this for initialization
	void Start () {
		this.renderer.material = materials[0];
		scoreValue = GameObject.FindGameObjectWithTag("Player").GetComponent<SpawnGates>();
	}
	
	// Update is called once per frame
	void Update () {
		if (scoreValue.score >= (lerpCounter + 1) * scoreTrigger) {
			this.renderer.material.Lerp(materials[lerpCounter], materials[lerpCounter + 1], Time.deltaTime * lerpSpeed);
			lerpCounter++;
		}
	}
}
