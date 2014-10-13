using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System.Collections;

public class GooglePlayIntegration : MonoBehaviour {
    // Declare variables
    [SerializeField]  string leaderboardName = "CgkIq63S6YYMEAIQBg";
    private DebugControls pause;
    private bool authenticationSuccess = false;

	// Use this for initialization
	void Start () {
        if (Application.platform != RuntimePlatform.Android) {
            this.enabled = false;
        }

        // Initialise variables
        pause = GameObject.FindGameObjectWithTag("Player").GetComponent<DebugControls>();

        // Initialise Google Play Games
        PlayGamesPlatform.DebugLogEnabled = true;

        if (PlayerPrefs.GetInt("GooglePlayActivated") == 0) {
            PlayGamesPlatform.Activate();
            PlayerPrefs.SetInt("GooglePlayActivated", 1);
        }

        // Authenticate the user
        Social.localUser.Authenticate(ProcessAuthentication);
	}
	
    // Method to process the authentication of the user
    private void ProcessAuthentication(bool success) {
        if (success) {
            Debug.Log("Authenticated");
            authenticationSuccess = true;
        } else {
            Debug.Log("Failed to Authenticate");
        }
    }

    // Method to handle achievement accomplishments
    public void UnlockAchievement(string achievementName) {
        string achievementID = "";

        switch (achievementName) {
            case "TutorialComplete":
                achievementID = "CgkIq63S6YYMEAIQAg";
                break;
            case "Scored10":
                achievementID = "CgkIq63S6YYMEAIQAw";
                break;
            case "Scored50":
                achievementID = "CgkIq63S6YYMEAIQBA";
                break;
            case "Scored100":
                achievementID = "CgkIq63S6YYMEAIQBQ";
                break;
            case "ViewCredits":
                achievementID = "CgkIq63S6YYMEAIQAQ";
                break;
            default:
                return;
        }

        Social.ReportProgress(achievementID, 100.0f, (bool success) => {
            if (success) {
                Debug.Log("Succeeded: " + achievementName);
            } else {
                Debug.Log("Failed: " + achievementName);
            }
        });
    }

    // Method to post a new score to the social leaderboards
    public void UpdateLeaderboard(int score) {
        Social.ReportScore(score, leaderboardName, (bool success) => {
            if (success) {
                Debug.Log("Succeeded in uploading score");
            } else {
                Debug.Log("Failed to upload score");
            }
        });
    }

    public void DisplayLeaderboardUI() {
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderboardName);
    }

    public void DisplayAchievementUI() {
        Social.ShowAchievementsUI();
    }

	// Update is called once per frame
	void Update () {
        //if (!authenticationSuccess) {
        //    pause.paused = true;
        //}
	}
}
