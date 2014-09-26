using UnityEngine;
using System.Collections;

public class SelectiveGroundSpawning : MonoBehaviour {
    // Declare variables
    [SerializeField] GameObject[] groundObjects;
    [SerializeField] float[] percentages;

	// Use this for initialization
	void Start () {
	    // Check that the percentages array is fine
        if (percentages.Length != groundObjects.Length) {
            Debug.LogError("PROBLEM WITH GROUND SPAWN PERCENTAGES");
        }

        float summation = 0.0f;
        for (int i = 0; i < percentages.Length; i++) {
            summation += percentages[i];
        }

        if (summation != 1.0f) {
            Debug.LogError("PROBLEM WITH GROUND SPAWN PERCENTAGES");
        }

        SelectGroundToSpawn();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void SelectGroundToSpawn() {
        float randomNumber = Random.value;
        float sum = 0.0f;
        for (int i = 0; i < percentages.Length; i++) {
            sum += percentages[i];

            if (randomNumber <= sum) {
                groundObjects[i].SetActive(true);
                sum = -1.0f;
            } else {
                groundObjects[i].SetActive(false);
            }
        }
    }
}
