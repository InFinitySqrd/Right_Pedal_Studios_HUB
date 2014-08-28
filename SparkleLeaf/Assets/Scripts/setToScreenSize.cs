using UnityEngine;
using System.Collections;

public class setToScreenSize : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.transform.guiTexture.pixelInset = new Rect(-Screen.width / 2, -Screen.height/2, Screen.width, Screen.height);
		//this.transform.guiTexture.pixelInset.y = Screen.height / 2;
		//this.transform.guiTexture.pixelInset.width = Screen.width;
		//this.transform.guiTexture.pixelInset.height = Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
