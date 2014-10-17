using UnityEngine;
using System.Collections;
using ChartboostSDK;

public class ChartboostAds : MonoBehaviour {

	void Awake() {
		CBExternal.init ();
	}

    public void ShowInterstitialAd() {
        CBExternal.showInterstitial(CBLocation.Default);
    }
}
