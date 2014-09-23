using UnityEngine;
using System.Collections;

public class FadeBetweenAudio : MonoBehaviour {
    // Declare variables
    private AudioSource bgmBackground, bgmTitle;

    void Awake() {
        bgmBackground = GameObject.Find("Background Track").GetComponent<AudioSource>();
        bgmTitle = GameObject.Find("Title Track").GetComponent<AudioSource>();
    }

	// Use this for initialization
	void Start () {
	
	}

    public void FadeAudio(bool titleOn) {
        StartCoroutine(FadeMusicFiles(titleOn));
    }
	
    IEnumerator FadeMusicFiles(bool title) {
        if (title) {
            while (bgmTitle.volume > 0.0f) {
                bgmTitle.volume -= Time.deltaTime;
                bgmBackground.volume += Time.deltaTime;
                yield return null;
            }
        } else {
            while (bgmBackground.volume > 0.0f) {
                bgmTitle.volume += Time.deltaTime;
                bgmBackground.volume -= Time.deltaTime;
                yield return null;
            }
        }
    }
}
