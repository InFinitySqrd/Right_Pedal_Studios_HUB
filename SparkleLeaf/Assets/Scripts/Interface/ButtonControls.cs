using UnityEngine;
using System.Collections;

public class ButtonControls : MonoBehaviour {
    private enum ButtonFunction {
        Play,
        Leaderboards,
        Settings,
        Back,
        MuteSFX,
        Info,
        ExitCredits,
        ShareMenu,
        TwitterShare,
        FacebookShare
    }
    [SerializeField] ButtonFunction buttonType;
	[SerializeField] Material settingsButton, backButton, BGMOn, BGMOff, SFXOn, SFXOff;
	[SerializeField] float buttonFadeMod = 2.0f;

    private Camera parentCamera;
    public bool inSubMenu = false;
	private ButtonControls subMenuParent;
	private float fadeSpeed = 0.0f;
	private bool bgmEnabled = true, sfxEnabled = true;
	private bool settingsOpen = false;

	private DebugControls pause;
	private LevelLost lostGame;
	private SpawnGates getScore;
	private Tutorial tutorial;

	private const string TwitterAddress = "http://twitter.com/intent/tweet";
	private const string TweetLanguage = "en";

    private bool shareUp = false;

    private FadeBetweenAudio audioFade;

    private GameObject audioManager;
    private GameObject FMODManager;

    private GooglePlayIntegration googlePlay;
    private GameCentreIntegration gameCentre;

	void Awake() {
/*		if (PlayerPrefs.GetInt("FacebookInitialised") == 0) {
			FB.Init(SetInit, OnHideUnity);
			PlayerPrefs.SetInt("FacebookInitialised", 1);
		}*/

        audioManager = GameObject.Find("AudioManager");
        //audioFade = audioManager.GetComponent<FadeBetweenAudio>();

        googlePlay = Camera.main.GetComponent<GooglePlayIntegration>();
        gameCentre = Camera.main.GetComponent<GameCentreIntegration>();

        //if (SFXOff != null && PlayerPrefs.GetInt("AudioEnabled") == 0) {
          //  sfxEnabled = false;
            //this.renderer.material = SFXOff;
            //SetAudioMute(true);
        //}
	}

	/*private void SetInit() {
		// Facebook is initialized
		enabled = true; 
	}*/

	private void OnHideUnity(bool isGameShown) {
		// Pause the game if Facebook tries to hide unity
		if (!isGameShown) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}
	}
/*
	private void AuthCallback(FBResult result) {
	}

    private void SendFacebookFeed() {
        FB.Feed(linkName: "Silent Grove",
                linkCaption: "I am playing Silent Grove",
                linkDescription: "I got " + getScore.score + " points in Silent Grove");
    }
*/
    private void ShareToTwitter(string text) {
        if (Application.platform == RuntimePlatform.Android) {
            Application.OpenURL(TwitterAddress + "?text=" + WWW.EscapeURL(text) + "&amp;lang=" + WWW.EscapeURL(TweetLanguage));
        } else if (TwitterPlugin.isAvailable && Application.platform == RuntimePlatform.IPhonePlayer) {
            TwitterPlugin.ComposeTweet(text);
        } else {
            Application.OpenURL(TwitterAddress + "?text=" + WWW.EscapeURL(text) + "&amp;lang=" + WWW.EscapeURL(TweetLanguage));
        }
    }

	// Use this for initialization
	void Start () {
		settingsButton = this.renderer.material;
		fadeSpeed = this.GetComponent<MenuTween>().fadeSpeed;
        
        FMODManager = GameObject.FindGameObjectWithTag ("FMOD_Manager");
        
        getScore = GameObject.FindGameObjectWithTag("Player").GetComponent<SpawnGates>();

        if (Application.platform == RuntimePlatform.Android) {
            googlePlay.UpdateLeaderboard(getScore.score);

            // Update achievements
            if (getScore.score >= 100) {
                googlePlay.UnlockAchievement("Scored100");
            } else if (getScore.score >= 50) {
                googlePlay.UnlockAchievement("Scored50");
            } else if (getScore.score >= 10) {
                googlePlay.UnlockAchievement("Scored10");
            }
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer) {
            gameCentre.WriteLeaderboard((long)getScore.score);
        }

        if (getScore.score == 0 && this.collider.name == "Share") {
            this.collider.enabled = false;
            this.renderer.enabled = false;
        }

		if (this.collider.name == "Settings") {
			settingsButton.color = new Color(settingsButton.color.r, settingsButton.color.g, settingsButton.color.b, 1.0f);
			backButton.color = new Color(backButton.color.r, backButton.color.g, backButton.color.b, 0.0f);
		}

		inSubMenu = false;
		if (this.transform.parent.name == "Settings") {
			subMenuParent = this.transform.parent.GetComponent<ButtonControls>();
		}

        if (this.transform.parent.camera != null) {
            parentCamera = this.transform.parent.camera;
        } else {
            parentCamera = this.transform.parent.parent.camera;
        }

		// Pause the game when the menu is drawn
		if (this.collider.name == "Play") {
			pause = GameObject.FindGameObjectWithTag("Player").GetComponent<DebugControls>();
			lostGame = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelLost>();
			tutorial = GameObject.FindGameObjectWithTag("Player").GetComponent<Tutorial>();

			if (pause != null) {
				pause.paused = true;
			} 
		}
	}
	
	// Update is called once per frame
	void Update () {
        CheckClick();

		if (this.collider.name == "Settings") {
			if (buttonType == ButtonFunction.Settings && settingsButton.color.a > 0.95f && backButton.color.a < 0.05f) {
				settingsButton.color = new Color(settingsButton.color.r, settingsButton.color.g, settingsButton.color.b, 1.0f);
				backButton.color = new Color(backButton.color.r, backButton.color.g, backButton.color.b, 0.0f);
			} else if (buttonType == ButtonFunction.Back && settingsButton.color.a < 0.05f && backButton.color.a > 0.95f) {
				settingsButton.color = new Color(settingsButton.color.r, settingsButton.color.g, settingsButton.color.b, 0.0f);
				backButton.color = new Color(backButton.color.r, backButton.color.g, backButton.color.b, 1.0f);
			}
		}
	}

    private void CheckClick() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = parentCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.name == this.collider.name) {
                    switch (buttonType) {
                        case ButtonFunction.Play:
                            // Restart the game
                            //Application.LoadLevel(Application.loadedLevel);
//                            audioFade.FadeAudio(true);

							if (pause != null && lostGame != null && tutorial != null && !lostGame.lost) {
								pause.paused = false;
								
								if (PlayerPrefs.GetInt("TutorialComplete") == 0) {
									tutorial.enabled = true;
								}
							//audioManager.GetComponent<FMOD_Manager>().ResumeGame();
							audioManager.GetComponent<FMOD_Manager>().ForestSetDeath(false);
							//print ("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");

								Destroy(this.transform.root.gameObject);
							} else if (pause != null && lostGame != null && lostGame.lost) {
								Application.LoadLevel(Application.loadedLevel);
							}
							break;
                        case ButtonFunction.Leaderboards:
                            // Display leaderboards
                            if (Application.platform == RuntimePlatform.Android) {
                                googlePlay.DisplayLeaderboardUI();
                            }

                            if (Application.platform == RuntimePlatform.IPhonePlayer) {
                                gameCentre.DisplayDefaultLeaderboard();
                            }
                            break;
                        case ButtonFunction.Settings:
                            // Open the settings scene
							if (settingsButton.color.a > 0.9f && backButton.color.a < 0.1f) {
	                            inSubMenu = true;
	                            SubMenu(inSubMenu);

								this.buttonType = ButtonFunction.Back;
								StartCoroutine(TweenButtonAlpha(true, settingsButton));						
								StartCoroutine(TweenButtonAlpha(false, backButton));
							}
                            break;
                        case ButtonFunction.Back:
                            // Return to the previous screen
							if (settingsButton.color.a < 0.1f && backButton.color.a > 0.9f) {
	                            inSubMenu = false;
	                            SubMenu(inSubMenu);

	                            this.buttonType = ButtonFunction.Settings;
								StartCoroutine(TweenButtonAlpha(false, settingsButton));						
								StartCoroutine(TweenButtonAlpha(true, backButton));
							}
                            break;
                        case ButtonFunction.Info:
                            // Switch the the credits and game info screen
							if (subMenuParent.inSubMenu) {
                                // Unlock the view credits achievement
                                googlePlay.UnlockAchievement("ViewCredits");

	                            Application.LoadLevelAdditive("CreditsScreen");
                                audioManager.GetComponent<FMOD_Manager>().MenuTransition(true);
	                            Destroy(this.transform.root.gameObject);
							}
                            break;
                        case ButtonFunction.MuteSFX:
							// Mute all sounds
							if (subMenuParent.inSubMenu) {
								sfxEnabled = !sfxEnabled;
								audioManager.GetComponent<FMOD_Manager>().mute(sfxEnabled);
								if (sfxEnabled) {
									this.renderer.material = SFXOn;
                                    //SetAudioMute(false);
                                    //PlayerPrefs.SetInt("AudioEnabled", 1);
								} else {
									this.renderer.material = SFXOff;
                                    //SetAudioMute(true);
                                    //PlayerPrefs.SetInt("AudioEnabled", 0);

								}
							}
                            break;
                        case ButtonFunction.ExitCredits:
                            // Bring up the regular menu screen
                            Application.LoadLevelAdditive("MenuScreen");
                            audioManager.GetComponent<FMOD_Manager>().MenuTransition(false);
                            Destroy(this.transform.root.gameObject);
                            break;
                        case ButtonFunction.ShareMenu:
                            // Load the share screen over the top of the menu
                            shareUp = !shareUp;

                            if (shareUp) {
                                Application.LoadLevelAdditive("ShareMenu");
                            } else {
                                Destroy(GameObject.Find("ShareCamera").gameObject);
                            }
                            break;
                        case ButtonFunction.TwitterShare:
                            // Use code to share player score on twitter
                            string tags = "@HUBGamesAus #SilentGrove";

                            if (getScore.score == 1) {
                                ShareToTwitter("I just scored " + getScore.score + " point in Silent Grove! " + tags);
                            } else {
                                ShareToTwitter("I just scored " + getScore.score + " points in Silent Grove! " + tags);
                            }
                            break;
                        case ButtonFunction.FacebookShare:
//                            CallToFacebook facebook = GameObject.Find("FacebookPost").GetComponent<CallToFacebook>();
//                            facebook.ShareClick();
						print("SORRY JAMES I NUKED FACEBOOK, WAS GIVING ME SHITTY ERRORS. Love Nathaniel :)");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    private void SetAudioMute(bool muted) {
        if (muted) {
            for (int i = 0; i < audioManager.transform.childCount; i++) {
                audioManager.transform.GetChild(i).audio.volume = 0.0f;
            }
        } else {
            for (int i = 0; i < audioManager.transform.childCount; i++) {
                audioManager.transform.GetChild(i).audio.volume = PlayerPrefs.GetFloat("AudioChild" + i);
            }
        }
    }

    private void SubMenu(bool menuState) {
        int numButtons = this.transform.childCount;

        if (menuState) {
            for (int i = 0; i < numButtons; i++) {
                MenuTween tween = this.transform.GetChild(i).GetComponent<MenuTween>();
                tween.TriggerForwardTween();
            }
        } else {
            for (int i = 0; i < numButtons; i++) {
                MenuTween tween = this.transform.GetChild(i).GetComponent<MenuTween>();
                tween.TriggerReverseTween();
            }
        }
    }

	IEnumerator TweenButtonAlpha(bool posTween, Material mat) {
		if (posTween) {
			while (mat.color.a > 0.0f) {
				mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a - Time.deltaTime * fadeSpeed * buttonFadeMod);
				yield return null;
			}
		} else {
			while (mat.color.a < 1.0f) {
				mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a + Time.deltaTime * fadeSpeed * buttonFadeMod);
				yield return null;
			}
		}
	}
}
