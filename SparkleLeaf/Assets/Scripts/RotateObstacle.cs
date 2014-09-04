using UnityEngine;
using System.Collections;

public class RotateObstacle : MonoBehaviour {
	// Declare variabes
	private float rotationSpeed;
	private bool clockwiseRot;

	private DebugControls pauseGame;
	
	// Use this for initialization
	void Start () {
		pauseGame = GameObject.FindGameObjectWithTag("Player").GetComponent<DebugControls>();
		
		if (Random.value >= 0.5f) {
			clockwiseRot = true;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!pauseGame.paused) {
			if (clockwiseRot) {
				this.transform.Rotate(Vector3.forward, rotationSpeed);
			} else {
				this.transform.Rotate(Vector3.back, rotationSpeed);
			}
		}
	}

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }
}
