using UnityEngine;
using System.Collections;
using ChartboostSDK;

public class ChartboostAds : MonoBehaviour {

	void Awake() {
		CBExternal.init ();
	}

	// Use this for initialization
	void Start () {
		CBExternal.showInterstitial (CBLocation.Default);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
