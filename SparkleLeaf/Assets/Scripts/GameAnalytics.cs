using UnityEngine;
using System.Collections;

public class GameAnalytics : MonoBehaviour {
	// Declare variables
	private int score;
	private int doubleCount;
	private float avgMomentum;
	private float timeBeforeDeath;
	private string obstacleName;

	private bool sentData = false;

	private LevelLost gameState;

	// Create methods to set values to be used by the GameAnalytics manager

	public void SetScore(int value) {
		score = value;
	}

	public void SetDoubleScores(int value) {
		doubleCount = value;
	}

	public void SetAverageMomentum(float value) {
		avgMomentum = value;
	}

	public void SetTimeBeforeDeath(float time) {
		timeBeforeDeath = time;
	}

	public void SetObstacleDeath(string name) {
		obstacleName = name;
	}

	// Use this for initialization
	void Start () {
		gameState = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelLost>();
	}

	// Update is called once per frame
	void Update () {
		if (gameState.lost && !sentData) {
			StartCoroutine("SendData");

			sentData = true;
		}
	}

	IEnumerator SendData() {
		GA.API.Design.NewEvent("Score", score);
		GA.API.Design.NewEvent("Double Score", doubleCount);
		GA.API.Design.NewEvent("Average Momentum", avgMomentum);
		GA.API.Design.NewEvent("Time Before Death", timeBeforeDeath);
		GA.API.Design.NewEvent("Player Died To " + obstacleName, 1);

		Debug.Log("SENDING");

		yield return null;
	}
}
