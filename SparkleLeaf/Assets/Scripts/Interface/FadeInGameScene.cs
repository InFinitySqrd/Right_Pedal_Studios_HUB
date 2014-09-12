using UnityEngine;
using System.Collections;

public class FadeInGameScene : MonoBehaviour {
	// Declare variables
	private Texture2D screenFade;
	[SerializeField] float fadeSpeed = 1.0f;

	void Awake() {
		screenFade = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		screenFade.SetPixel(1, 1, Color.black);
		screenFade.Apply();
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(FadeScreen());
	}
	
	// Update is called once per frame
	void Update () {
		print ("A");
		if (screenFade.GetPixel(1,1).a <= 0.05f) {
			this.GetComponent<FadeInGameScene>().enabled = false;
		}
	}

	void OnGUI () {
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), screenFade);
	}

	IEnumerator FadeScreen() {
		while (screenFade.GetPixel(1,1).a >= 0.05f) {
			screenFade.SetPixel(1,1, new Color(Color.black.r, Color.black.g, Color.black.b, screenFade.GetPixel(1,1).a - Time.deltaTime * fadeSpeed));
			screenFade.Apply();
				
			yield return null;
		}
	}
}
