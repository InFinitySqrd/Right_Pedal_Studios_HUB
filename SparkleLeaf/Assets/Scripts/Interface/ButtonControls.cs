using UnityEngine;
using System.Collections;

public class ButtonControls : MonoBehaviour {
    private enum ButtonFunction {
        Play,
        Leaderboards,
        Settings,
        Back,
        MuteSFX,
        MuteBGM,
        Info,
        ExitCredits
    }
    [SerializeField] ButtonFunction buttonType;
	[SerializeField] Material settingsButton, backButton, BGMOn, BGMOff, SFXOn, SFXOff;
	[SerializeField] float buttonFadeMod = 2.0f;

    private Camera parentCamera;
    private bool inSubMenu = false;
	private float fadeSpeed = 0.0f;
	private bool bgmEnabled = true, sfxEnabled = true;
	private bool settingsOpen = false;

	private DebugControls pause;
	private LevelLost lostGame;
	private Tutorial tutorial;

	// Use this for initialization
	void Start () {
		settingsButton = this.renderer.material;
		fadeSpeed = this.GetComponent<MenuTween>().fadeSpeed;

		if (this.collider.name == "Settings") {
			settingsButton.color = new Color(settingsButton.color.r, settingsButton.color.g, settingsButton.color.b, 1.0f);
			backButton.color = new Color(backButton.color.r, backButton.color.g, backButton.color.b, 0.0f);
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
		Debug.Log (inSubMenu);
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
							if (pause != null && lostGame != null && tutorial != null && !lostGame.lost) {
								pause.paused = false;
								
								if (PlayerPrefs.GetInt("TutorialComplete") == 0) {
									tutorial.enabled = true;
								}

								Destroy(this.transform.root.gameObject);
							} else if (pause != null && lostGame != null && lostGame.lost) {
								Application.LoadLevel(Application.loadedLevel);
							}
							break;
                        case ButtonFunction.Leaderboards:
                            // Open the leaderboards scene
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
							if (true) {
	                            Application.LoadLevelAdditive("CreditsScreen");
	                            Destroy(this.transform.root.gameObject);
							}
                            break;
                        case ButtonFunction.MuteBGM:
                            // Mute background track
							if (true) {
								bgmEnabled = !bgmEnabled;
								
								if (bgmEnabled) {
									this.renderer.material = BGMOn;
								} else {
									this.renderer.material = BGMOff;
								}
							}
                            break;
                        case ButtonFunction.MuteSFX:
							// Mute all sound effects
							if (true) {
								sfxEnabled = !sfxEnabled;

								if (sfxEnabled) {
									this.renderer.material = SFXOn;
								} else {
									this.renderer.material = SFXOff;
								}
							}
                            break;
                        case ButtonFunction.ExitCredits:
                            // Bring up the regular menu screen
                            Application.LoadLevelAdditive("MenuScreen");
                            Destroy(this.transform.root.gameObject);
                            break;
                        default:
                            break;
                    }
                }
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
