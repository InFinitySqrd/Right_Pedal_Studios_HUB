using UnityEngine;
using System.Collections;

public class CallToFacebook : MonoBehaviour {
	    // Declare variables
    private SpawnGates getScore;

	void Awake() {
		FB.Init(this.FacebookInitCallback, OnHideUnity);

        getScore = GameObject.FindGameObjectWithTag("Player").GetComponent<SpawnGates>();
	}

    public void ShareClick() {
        StartCoroutine(ShareToFacebook());
    }

	// added a wait, however this does not prevent additional touches before loading
	IEnumerator ShareToFacebook() {
		if (FB.IsLoggedIn) {
			this.PostToFacebookFeed();
		} else {
			FB.Login("email,publish_actions", LoginCallback);
		}
        yield return null;
	}
	
	private void FacebookInitCallback() {
		// stub
	}

	/// <summary>
	/// The delegate function used in response the Facebook SDK assuming control over the application.
	/// </summary>
	/// <param name="isGameShown">If set to <c>true</c> if game is shown.</param>
	/// <remarks>This is used to pause and resume the underlying application during Facebook operations.</remarks>
	private void OnHideUnity(bool isGameShown) {                                                              
		if (!isGameShown) {                                
			Time.timeScale = 0;                                                                  
		} else {                                                                                 
			Time.timeScale = 1;                                                                  
		}                                                                                        
	}

	/// <summary>
	/// The callback delegate function used in response to a user logging into Facebook via the Facebook SDK.
	/// </summary>
	/// <param name="result">The Result of the log in.</param>
	void LoginCallback(FBResult result) {
		if (FB.IsLoggedIn) {                                                                                      
			Debug.Log("Logged in. ID: " + FB.UserId);
			this.PostToFacebookFeed();
		}                                                                                      
	}                                                                                          

	/// <summary>
	/// Post a status to the user's Facebook feed.
	/// </summary>
	void PostToFacebookFeed() {
		FB.Feed(
			link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest"),
			linkName: "Silent Grove",
			linkDescription: "I just scored " + getScore.score.ToString() + " points in Silent Grove!",
			picture: "http://www.friendsmash.com/images/logo_large.jpg"
			//callback: FeedPostCallback
		);
	}

    /*
	void FeedPostCallback(FBResult response) {
		const string ID_KEY = "id";

		// parse response data
		Dictionary<string, string> responseData = null;
		if (response.Text != null) {
			responseData = Json.Deserialize(response.Text) as Dictionary<string, string>;
		}

		// handle response
		if (responseData != null && responseData.ContainsKey(ID_KEY) && responseData[ID_KEY] == null) {
			
			// TODO: Replace with error message in-game.
			Debug.Log("********* Network Error! *********"); 
		}
	}*/
}
