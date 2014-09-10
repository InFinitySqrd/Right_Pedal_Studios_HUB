using UnityEngine;
using System.Collections;

public class LevelLost : MonoBehaviour {
	// Declare variables
	[SerializeField] string loseText;
	public bool lost = false;

	private float timeUntilDeath;
	private GameAnalytics GAStuff;

    private bool menuUp = false;

	// Use this for initialization
	void Start () {
		GAStuff = GameObject.FindGameObjectWithTag("GameAnalytics").GetComponent<GameAnalytics>();

		timeUntilDeath = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		timeUntilDeath += Time.deltaTime;

        if (!menuUp && lost) {
            Application.LoadLevelAdditive("MenuScreen");
            menuUp = true;

            if (Input.touchCount >= 3 || Input.GetKeyDown(KeyCode.Delete)) {
                Application.LoadLevel("MainMenu");
            }
        }
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Obstacle") {
			lost = true;

			string name = other.transform.parent.GetComponent<MovingGates>().gateName;
			GAStuff.SetObstacleDeath(name);
			GAStuff.SetTimeBeforeDeath(timeUntilDeath);
		}
	}
}
