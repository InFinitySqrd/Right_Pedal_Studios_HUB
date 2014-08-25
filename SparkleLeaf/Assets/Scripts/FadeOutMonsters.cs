using UnityEngine;
using System.Collections;

public class FadeOutMonsters : MonoBehaviour {
	// Declare variables
	[SerializeField] float fadeSpeed = 1.0f;
	private Transform playerPos;
	private bool fading = false;
	
	// Use this for initialization
	void Start () {
		playerPos = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.z <= playerPos.position.z) {
			fading = true;
		}
		
		if (fading) {
			this.renderer.material.color = new Color(this.renderer.material.color.r, this.renderer.material.color.g, this.renderer.material.color.b, this.renderer.material.color.a - fadeSpeed * Time.deltaTime);
		}
	}
}
