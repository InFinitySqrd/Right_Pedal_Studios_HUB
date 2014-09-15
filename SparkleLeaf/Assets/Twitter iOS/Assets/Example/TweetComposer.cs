using UnityEngine;
using System.Collections;

public class TweetComposer : MonoBehaviour {
    private bool availability;

    void Start() {
        availability = TwitterPlugin.isAvailable;
    }

    void OnGUI() {
        if (availability) {
            if (GUI.Button(new Rect(0.0f, 0.0f, Screen.width, 0.1f * Screen.height), "Send a tweet")) {
                TwitterPlugin.ComposeTweet("TEST テスト 실험 试验", "http://www.example.com/loooooooooooooooooooooooooooooooooooooong_url");
            }
            if (GUI.Button(new Rect(0.0f, 0.1f * Screen.height, Screen.width, 0.1f * Screen.height), "Send a tweet with a screenshot")) {
                TwitterPlugin.ComposeTweetWithScreenshot("TEST (with a screenshot)", "http://www.example.com/loooooooooooooooooooooooooooooooooooooong_url");
            }
        } else {
            GUI.Label(new Rect(0.0f, 0.0f, Screen.width, 0.15f * Screen.height), "Twitter API is not available.");
        }
    }
}

