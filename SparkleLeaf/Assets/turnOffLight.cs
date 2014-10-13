using UnityEngine;
using System.Collections;

public class turnOffLight : MonoBehaviour {
	public GameObject light;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void destroyLight () {
		//light.SetActive (false);
		light.GetComponent<Light> ().color = new Vector4 (0, 0, 0, 255);
	}
}
