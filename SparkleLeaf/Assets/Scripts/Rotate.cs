using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
    public float rotateSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(Vector3.forward * 50 * Time.deltaTime);
	}
}
