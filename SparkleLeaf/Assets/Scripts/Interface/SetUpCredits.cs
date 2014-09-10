using UnityEngine;
using System.Collections;

public class SetUpCredits : MonoBehaviour {
    // Declare variables
    [SerializeField] Transform title, creditsList, backButton;

	void Awake () {
	    // Set up all UI elements to scale with screen size
        title.transform.position = this.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f, 3.0f * Screen.height / 4.0f, 1.0f));
    }

    // Use this for initialization
    void Start() {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
