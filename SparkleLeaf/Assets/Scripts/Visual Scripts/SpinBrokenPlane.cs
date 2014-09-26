using UnityEngine;
using System.Collections;

public class SpinBrokenPlane : MonoBehaviour {
    // Declare variables
    [SerializeField] float maxTorque = 1.0f;

	// Use this for initialization
	void Start () {
        Vector3 direction = new Vector3(Random.Range(0.0f, maxTorque), Random.Range(0.0f, maxTorque), Random.Range(0.0f, maxTorque));
	    this.rigidbody.AddTorque(direction);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
