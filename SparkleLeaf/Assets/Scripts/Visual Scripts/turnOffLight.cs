using UnityEngine;
using System.Collections;

public class turnOffLight : MonoBehaviour {
	public GameObject light;
    [SerializeField] float fadeSpeed = 1.0f;
    private bool fadeLight = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (fadeLight) {
            light.light.intensity -= Time.deltaTime * fadeSpeed;
        }
	}

	public void DestroyLight () {
		//light.SetActive (false);
		fadeLight = true;
	}
}
