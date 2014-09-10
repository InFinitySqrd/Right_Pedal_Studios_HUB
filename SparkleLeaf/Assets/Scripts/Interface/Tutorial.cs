using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {
    // Declare variables
    [SerializeField] Transform tutorialIcons;
    [SerializeField] Transform tutorialText;
    [SerializeField] float minHoldTime = 1.0f;
    [SerializeField] float delayToPlay = 1.0f;
    [SerializeField] float fadeRate = 0.2f;

    private float leftTimer = 0.0f, rightTimer = 0.0f, timer;
    private GameObject leftIcon, rightIcon, text;
    private bool leftAchieved, rightAchieved;

	private bool instantiated = false;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt("TutorialComplete") == 0 && !instantiated) {
			InstantiateTuteVars();
			instantiated = true;
		} else {
			this.enabled = false;
		}
	}
	// Update is called once per frame
	void Update () {
        // Determine when the player has completed the tutorial
        if (Input.GetMouseButton(0)) {
            if (Input.mousePosition.x > Screen.width / 2.0f) {
                rightTimer += Time.deltaTime;

                if (rightTimer > minHoldTime) {
                    rightAchieved = true;
                }
            } else {
                leftTimer += Time.deltaTime;

                if (leftTimer > minHoldTime) {
                    leftAchieved = true;
                }
            }
        }

        if (rightAchieved) {
            StartCoroutine(Fade(rightIcon.renderer.material));
        }

        if (leftAchieved) {
            StartCoroutine(Fade(leftIcon.renderer.material));
        }

        if (leftAchieved && rightAchieved) {
            if (timer >= delayToPlay) {
                StartCoroutine(Fade(text.renderer.material));
            } else {
                timer += Time.deltaTime;
            }

            if (text.renderer.material.color.a < 0.1f) {
                PlayerPrefs.SetInt("TutorialComplete", 1);
                Destroy(leftIcon);
                Destroy(rightIcon);
                Destroy(text);
                this.GetComponent<Tutorial>().enabled = false;
            }
        }
	}

	private void InstantiateTuteVars() {
		// Spawn hold points
		for (int i = -1; i < 2; i += 2) {
			// Set up an appropriate spawn point
			Vector3 spawnPoint = new Vector3(Screen.width / 2.0f + (i * Screen.width / 3.0f), Screen.height / 8.0f, -Camera.main.transform.position.z);
			spawnPoint = Camera.main.ScreenToWorldPoint(spawnPoint);
			
			// Instantiate the icon
			if (i == -1) {
				leftIcon = (GameObject)Instantiate(tutorialIcons.gameObject, spawnPoint, tutorialIcons.transform.rotation);
			} else if (i == 1) {
				rightIcon = (GameObject)Instantiate(tutorialIcons.gameObject, spawnPoint, tutorialIcons.transform.rotation);
			}
		}
		
		// Spawn tutorial text
		// Set up an appropriate spawn point
		Vector3 transform = new Vector3(Screen.width / 2.0f, Screen.height - Screen.height / 3.0f, -Camera.main.transform.position.z);
		transform = Camera.main.ScreenToWorldPoint(transform);
		
		// Instantiate the text
		text = (GameObject)Instantiate(tutorialText.gameObject, transform, tutorialIcons.transform.rotation);
	}

    IEnumerator Fade(Material transparentMat) {
        while (transparentMat.color.a > 0.0f) {
            transparentMat.color = new Color(transparentMat.color.r, transparentMat.color.g, transparentMat.color.b, transparentMat.color.a - Time.deltaTime * fadeRate);
            yield return null;
        }
    }
}
