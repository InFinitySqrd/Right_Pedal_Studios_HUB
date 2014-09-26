using UnityEngine;
using System.Collections;

public class FogScript : MonoBehaviour {
    // Declare variables
    [SerializeField] float chanceToAppear = 0.2f, moveSpeed = 1.0f, randomSpawnRange = 1.0f;
    private float randomNumber = 0.0f;
    private Vector3 position;
    private bool moveLeft = true;

	// Use this for initialization
	void Start () {
	    randomNumber = Random.value;

        if (randomNumber > chanceToAppear) {
            this.renderer.enabled = false;
        }

        position = this.transform.position;
        randomNumber = Random.value;

        if (randomNumber < 0.5f) {
            moveLeft = true;
        } else {
            moveLeft = false;
        }

        this.transform.position = new Vector3(this.transform.position.x + Random.Range(-randomSpawnRange, randomSpawnRange), this.transform.position.y, this.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        if (moveLeft) {
            this.transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);

            if (this.transform.position.x > position.x + randomSpawnRange) {
                moveLeft = false;
            }
        } else {
            this.transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);

            if (this.transform.position.x < position.x - randomSpawnRange) {
                moveLeft = true;
            }
        }
	}
}
