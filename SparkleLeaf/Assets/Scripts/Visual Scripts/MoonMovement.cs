using UnityEngine;
using System.Collections;

public class MoonMovement : MonoBehaviour {
    // Declare variables
    [SerializeField] float moveSpeed = 0.0001f;
    [SerializeField] float moonDuration = 20.0f;
    [SerializeField] float timeToNewMoon = 10.0f;
    [SerializeField] float timeBetweenXAdditions = 4.0f;
    private Vector3 movementVector;
    private Vector3 initialSpawnPos;
    private Rect initialSpawnRect;

	// Use this for initialization
	void Start () {
        // Set the initial position of the moon
        initialSpawnPos = this.transform.position;
        initialSpawnRect = new Rect(0.0f - Screen.width / 8.0f, 0.0f - 0.6f * Screen.height / 3.0f, Screen.width / 3.0f, Screen.width / 3.0f);
	    this.guiTexture.pixelInset = initialSpawnRect;

        //initialSpawnPos = new Vector3(0.0f, -200.0f, 400.0f);
        //this.transform.position = initialSpawnPos;
        //this.transform.localScale = new Vector3(20.0f, 20.0f, 20.0f);

        // Initialise the movement of the moon
        movementVector = Vector3.up;

        // Move the moon in a slight Y direction
        StartCoroutine(SetXValue());
        StartCoroutine(MoonCounter());
	}
	
	// Update is called once per frame
	void Update () {
	    this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + movementVector, Time.deltaTime * moveSpeed);
    }

    IEnumerator MoonCounter() {
        yield return new WaitForSeconds(moonDuration);
        StartCoroutine(ResetMoonPosition());
    }

    IEnumerator SetXValue() {
        movementVector = new Vector3(movementVector.x + moveSpeed, movementVector.y, movementVector.z);
        yield return new WaitForSeconds(timeBetweenXAdditions);
        StartCoroutine(SetXValue());
    }

    IEnumerator ResetMoonPosition() {
        yield return new WaitForSeconds(timeToNewMoon);

        // Reset position
        this.transform.position = initialSpawnPos;
        movementVector = Vector3.up;
        
        // Reassign a different coloured texture;

        // Start the moon moving again
        StartCoroutine(MoonCounter());
    }
}
