using UnityEngine;
using System.Collections;

public class SetToScreenSize : MonoBehaviour {
	// Declare variables
	[SerializeField] bool mainMenu = false;
    [SerializeField] bool isVignette = false;

	// Use this for initialization
	void Start () {
		if (!mainMenu) {
			this.transform.guiTexture.pixelInset = new Rect(-Screen.width / 2, -Screen.height/2, Screen.width, Screen.height);
		} else {
			this.transform.guiTexture.pixelInset = new Rect(-Screen.width * 1.1f / 2, -Screen.height * 1.2f / 2, Screen.width * 1.1f, Screen.height * 1.2f);
		}

        if (isVignette) {
            this.transform.guiTexture.pixelInset = new Rect(0.0f, 0.0f, Screen.width * 1.1f, Screen.height * 1.2f);
        }



		//this.transform.guiTexture.pixelInset.y = Screen.height / 2;
		//this.transform.guiTexture.pixelInset.width = Screen.width;
		//this.transform.guiTexture.pixelInset.height = Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
