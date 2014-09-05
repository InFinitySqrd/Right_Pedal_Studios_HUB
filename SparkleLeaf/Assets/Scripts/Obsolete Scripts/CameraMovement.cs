using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	// Declare variables
	private Transform planePos;
	private Vector3 cameraPosDifference;
	
	// Use this for initialization
	void Start () {
		planePos = GameObject.FindGameObjectWithTag("Player").transform;
		cameraPosDifference = this.transform.position - planePos.position;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = planePos.position + cameraPosDifference;
	}
}
