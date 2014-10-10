using UnityEngine;
using System.Collections;

public class PlaneTrail : MonoBehaviour {
    // Declare variables
    [SerializeField] Transform followObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    this.transform.position = Vector3.MoveTowards(this.transform.position, followObject.position, Time.deltaTime);
	}
}
