using UnityEngine;
using System.Collections;

public class MoonMovement : MonoBehaviour {
    // Declare variables
    [SerializeField] float moveSpeed = 0.0001f;
    [SerializeField] float timeToNewMoon = 10.0f;
    private Vector3 movementVector;
    private Vector3 initialSpawnPos;

	// Use this for initialization
	void Start () {
        // Set the initial position of the moon
	    //this.guiTexture.pixelInset = new Rect(0.0f - Screen.width / 8.0f, 0.0f - Screen.height / 3.0f, Screen.width / 4.0f, Screen.width / 4.0f);
        initialSpawnPos = new Vector3(0.0f, -200.0f, 400.0f);
        this.transform.position = initialSpawnPos;
        this.transform.localScale = new Vector3(20.0f, 20.0f, 20.0f);

        // Initialise the movement of the moon
        movementVector = Vector3.up;

        // Move the moon in a slight Y direction
        //StartCoroutine(SetYValue());
	}
	
	// Update is called once per frame
	void Update () {
	    this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + movementVector, Time.deltaTime * moveSpeed);

        if (!this.renderer.isVisible && this.transform.position.y > 0.0f) {
            // When the moon has left the screen, reassign it's texture and reset it's position
            StartCoroutine(ResetMoonPosition());
        }
    }

    IEnumerator SetYValue() {
        movementVector = new Vector3(movementVector.x + moveSpeed, movementVector.y, movementVector.z);
        yield return new WaitForSeconds(4.0f);
        StartCoroutine(SetYValue());
    }

    IEnumerator ResetMoonPosition() {
        yield return new WaitForSeconds(timeToNewMoon);

        // Reset position
        this.transform.position = initialSpawnPos;

        // Reassign a different coloured texture;
    }
}
