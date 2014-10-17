using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UnityAdsIntegration : MonoBehaviour {
    [SerializeField] string appID = "18381";
	// Use this for initialization
	void Start () {
	    Advertisement.Initialize(appID);
    }

    public void AdDisplay() {
        if (Advertisement.isSupported) {
            Advertisement.Show();
        }
    }
}
