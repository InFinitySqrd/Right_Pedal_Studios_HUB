using UnityEngine;
using System.Collections;

public class turnOffLight : MonoBehaviour {
	public GameObject light;
    [SerializeField] float fadeSpeed = 1.0f;
    private bool fadeLight = false;
	private LevelLost lostGame;
	private Transform player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		lostGame = player.gameObject.GetComponent<LevelLost>();
	}
	
	// Update is called once per frame
	void Update () {
        if (fadeLight) {
						light.light.intensity -= Time.deltaTime * fadeSpeed;
				} else {
						if (lostGame.lost) {
				DestroyLight();
						}
				}
	}

	public void DestroyLight () {
		//light.SetActive (false);
		fadeLight = true;
	}
}
