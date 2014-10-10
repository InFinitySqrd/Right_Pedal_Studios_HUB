using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
using System.Collections;

public class GameCentreIntegration : MonoBehaviour {
    // Declare variables
    ILeaderboard leaderboard;

    void Start() {
        if (Application.platform != RuntimePlatform.IPhonePlayer) {
            this.enabled = false;
        }

        // Register the user
        Social.localUser.Authenticate(ProcessAuthentication);
        leaderboard = Social.CreateLeaderboard();
        leaderboard.id = "SilentGrove_Scores";
    }

    private void ProcessAuthentication(bool success) {
        if (success) {
            Debug.Log("Authenticated");
        } else {
            Debug.Log("Failed to Authenticate");
        }
    }

    public void GetLeaderboardResults() {
        leaderboard.LoadScores(result => ReadLeaderboard(result));
    }

    private void ReadLeaderboard(bool result) {
        if (leaderboard.scores.Length > 0) {
            Debug.Log("Loaded " + leaderboard.scores.Length + " scores");

            string myScores = "Leaderboard:\n";

            foreach (IScore score in leaderboard.scores) {
                myScores += "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
            }

            Debug.Log(myScores);
        } else {
            Debug.Log("No scores loaded");
        }
    }

    public void WriteLeaderboard(long scoreValue) {
        Debug.Log("Reporting score " + scoreValue + " to leaderboard " + leaderboard.id);

        Social.ReportScore(scoreValue, leaderboard.id, success => {
            Debug.Log(success ? "Success":"Failed");
        });
    }

    public void DisplayDefaultLeaderboard() {
        Social.ShowLeaderboardUI();
    }
}
