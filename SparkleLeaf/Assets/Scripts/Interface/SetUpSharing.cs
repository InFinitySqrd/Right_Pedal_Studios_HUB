using UnityEngine;
using System.Collections;

public class SetUpSharing : MonoBehaviour {
    // Declare variables
    [SerializeField] Transform twitter, facebook;
    private Transform share;

	// Use this for initialization
	void Start () {
        share = GameObject.Find("Share").transform;
        Vector3 sharePos = this.camera.WorldToScreenPoint(share.position);

	    twitter.transform.position = this.camera.ScreenToWorldPoint(new Vector3(sharePos.x + Screen.width / 10.0f, sharePos.y + Screen.height / 12.0f, 1.0f));
        facebook.transform.position = this.camera.ScreenToWorldPoint(new Vector3(sharePos.x - Screen.width / 10.0f, sharePos.y + Screen.height / 12.0f, 1.0f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
