using UnityEngine;
using System.Collections;

public class SplashScreenTransitions : MonoBehaviour {
	// Declare variables
	private Texture2D screenFade;

	[SerializeField] GameObject[] splashScreens;
	[SerializeField] float fadeSpeed = 1.0f;
	[SerializeField] float splashDuration = 3.0f;

	private float timer = 0.0f;
	private int fadeNumber = 0;
	private bool fadingOut = false, fadingIn = false;
	private bool fadeDirection = true;

	// Use this for initialization
	void Start () {
		screenFade = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		screenFade.SetPixel(1, 1, Color.black);
		screenFade.Apply();
		splashScreens[fadeNumber].SetActive(true);
		StartCoroutine(FadeScreen(true));
		fadingOut = true;
	}

	void Update() {
		if (screenFade.GetPixel(1,1).a >= 0.95f) {
			fadingIn = false;
		}

		if (screenFade.GetPixel(1,1).a <= 0.05f) {
			fadingOut = false;
		}

		if (!fadingIn && !fadingOut) {
			if (!fadingIn && fadeDirection) {
				if (timer >= splashDuration) {
					screenFade.SetPixel(1,1, new Color(Color.black.r, Color.black.g, Color.black.b, 0.0f));
					screenFade.Apply();

					StartCoroutine(FadeScreen(false));
					fadeNumber++;

					fadeDirection = false;
					fadingIn = true;
					timer = 0.0f;
				} else {
					timer += Time.deltaTime;
				}
			} else if (!fadingOut && !fadeDirection) {
				if (fadeNumber == splashScreens.Length) {
					Application.LoadLevel(1);
				} else {
					if (fadeNumber > 0) {
						splashScreens[fadeNumber - 1].SetActive(false);
					}
					splashScreens[fadeNumber].SetActive(true);

					screenFade.SetPixel(1,1, new Color(Color.black.r, Color.black.g, Color.black.b, 1.0f));
					screenFade.Apply();

					StartCoroutine(FadeScreen(true));
					fadingOut = true;
					fadeDirection = true;
				}
			}
		}
	}

	void OnGUI () {
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), screenFade);
	}

	IEnumerator FadeScreen(bool fadeOut) {
		if (fadeOut) {
			while (screenFade.GetPixel(1,1).a >= 0.05f) {
				screenFade.SetPixel(1,1, new Color(Color.black.r, Color.black.g, Color.black.b, screenFade.GetPixel(1,1).a - Time.deltaTime * fadeSpeed));
				screenFade.Apply();

				yield return null;
			}
		} else {
			while (screenFade.GetPixel(1,1).a <= 0.95f) {
				screenFade.SetPixel(1,1, new Color(Color.black.r, Color.black.g, Color.black.b, screenFade.GetPixel(1,1).a + Time.deltaTime * fadeSpeed));
				screenFade.Apply();
				
				yield return null;
			}
		}
	}
}
