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
    [SerializeField] Material backTexture;
    [SerializeField] Material settingsTexture;

    private Camera parentCamera;
    private bool inSubMenu = false;

	// Use this for initialization
	void Start () {
        if (this.transform.parent.camera != null) {
            parentCamera = this.transform.parent.camera;
        } else {
            parentCamera = this.transform.parent.parent.camera;
        }
	}
	
	// Update is called once per frame
	void Update () {
        CheckClick();
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
                            Application.LoadLevel(Application.loadedLevel);
                            break;
                        case ButtonFunction.Leaderboards:
                            // Open the leaderboards scene
                            break;
                        case ButtonFunction.Settings:
                            // Open the settings scene
                            inSubMenu = !inSubMenu;
                            SubMenu(inSubMenu);

                            this.buttonType = ButtonFunction.Back;
                            this.renderer.material = backTexture;
                            break;
                        case ButtonFunction.Back:
                            // Return to the previous screen
                            inSubMenu = !inSubMenu;
                            SubMenu(inSubMenu);

                            this.buttonType = ButtonFunction.Settings;
                            this.renderer.material = settingsTexture;
                            break;
                        case ButtonFunction.Info:
                            // Switch the the credits and game info screen
                            Application.LoadLevelAdditive("CreditsScreen");
                            Destroy(this.transform.root.gameObject);
                            break;
                        case ButtonFunction.MuteBGM:
                            // Mute background track
                            break;
                        case ButtonFunction.MuteSFX:
                            // Mute all sound effects
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
}
