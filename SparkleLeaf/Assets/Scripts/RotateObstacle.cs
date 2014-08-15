using UnityEngine;
using System.Collections;

public class RotateObstacle : MonoBehaviour {
	// Declare variabes
	private const float  minSpeed = 0.2f;
	private const  float maxSpeed = 1.4f;
	private float rotationSpeed;
	private bool clockwiseRot;
	
	// Use this for initialization
	void Start () {
		rotationSpeed = Random.Range(minSpeed, maxSpeed);
		
		if (Random.value >= 0.5f) {
			clockwiseRot = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (clockwiseRot) {
			this.transform.Rotate(Vector3.forward, rotationSpeed);
		} else {
			this.transform.Rotate(Vector3.back, rotationSpeed);
		}
	}
}
