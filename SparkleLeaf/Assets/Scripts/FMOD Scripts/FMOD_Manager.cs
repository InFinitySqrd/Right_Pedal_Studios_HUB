using UnityEngine;
using System.Collections;

public class FMOD_Manager : MonoBehaviour {

	private static FMOD_Manager instance = null;
	public static FMOD_Manager Instance {
		get { return instance; }
	}

	//private FMOD.Studio.EventInstance FMOD_Gameplay;
	//private FMOD.Studio.ParameterInstance FMOD_MenuTransitions, FMOD_Tutorial, FMOD_Time, FMOD_Death;

	private bool isplaying = false;

	private float deathTimer = 0.0f;
	private bool playerDead = false;

	private bool fadeOutOfDeath = false;
	private float fadeOutTimer = 0.0f;

	private bool fadeUpTime = false;
	private float fadeUpTimer = 0.0f;
	private float fadeUpTimerTarget = 0.0f;
	private float fadeUpTargetValue = 0.0f;


	[SerializeField] float deathFadeLength = 0.0f;
	[SerializeField] float fadeFromDeathLength = 0.0f;
	[SerializeField] float fadeUpFromScoreLength = 0.0f;
	[SerializeField] float menuFadeUpLength = 0.0f;
	[SerializeField] float menuFadeDownLength = 0.0f;

	private float menuFadeUpTimer = 0.0f;
	private bool menuFadeUp = false;

	private float menuFadeDownTimer = 0.0f;
	private bool menuFadeDown = false;

	[SerializeField] int targetScoreOne = 0;
	[SerializeField] int targetScoreTwo = 0;
	[SerializeField] int targetScoreThree = 0;
	[SerializeField] int targetScoreFour = 0;

	void Awake() {
		if (instance != null && instance != this) {
						Destroy (this.gameObject);
						return;
				} else {
						instance = this;
				}
		DontDestroyOnLoad (this.gameObject);
	}

	// Use this for initialization
	void Start () {
		//FMOD_Gameplay = FMOD_StudioSystem.instance.GetEvent ("event:/Music/Gameplay");
		//FMOD_Gameplay.getParameter ("Menu Transitions", out FMOD_MenuTransitions);
		//FMOD_Gameplay.getParameter ("Title/Tutorial", out FMOD_Tutorial);
		//FMOD_Gameplay.getParameter ("Time", out FMOD_Time);
		//FMOD_Gameplay.getParameter ("Death", out FMOD_Death);

		//FMOD_Gameplay.start ();

	}
	
	// Update is called once per frame
	void Update () {
		if (playerDead) {
			if (deathTimer <= deathFadeLength) {
				deathTimer += Time.deltaTime;
				//FMOD_Death.setValue (deathTimer * 2);
			} 
			else { 
				playerDead = false;
				deathTimer = 0;
			}
		}

		if (fadeOutOfDeath) {
			if (fadeOutTimer <= fadeFromDeathLength) {
				fadeOutTimer += Time.deltaTime;
				//FMOD_Death.setValue(1 - fadeOutTimer);
			}

			else {
				//FMOD_Death.setValue(0);
				fadeOutOfDeath = false;
				fadeOutTimer = 0.0f;
			}
		}

		if (fadeUpTime) {

			if (fadeUpTimer <= fadeUpFromScoreLength) {
				fadeUpTimer += Time.deltaTime;

				//FMOD_Time.setValue(fadeUpTargetValue - 0.1f + (((fadeUpTimer / fadeUpFromScoreLength) / 10)));
				print (fadeUpTargetValue - 0.1f + (((fadeUpTimer / fadeUpFromScoreLength) / 10)));
			}

			else {
				fadeUpTimer = 0.0f;
				fadeUpTime = false;
				//FMOD_Time.setValue(fadeUpTargetValue);
				print (fadeUpTargetValue);
			}
		}

		if (menuFadeUp) {
			if (menuFadeUpTimer <= menuFadeUpLength) {
				menuFadeUpTimer += Time.deltaTime;
				//FMOD_MenuTransitions.setValue(Mathf.Clamp(menuFadeUpTimer / menuFadeUpLength, 0,1));
				print (menuFadeUpTimer / menuFadeUpLength);
			}

			else {
				//FMOD_MenuTransitions.setValue(1);
				menuFadeUp = false;
				menuFadeUpTimer = 0.0f;
			}
		}

		if (menuFadeDown) {
			if (menuFadeDownTimer <= menuFadeDownLength) {
				menuFadeDownTimer += Time.deltaTime;
				//FMOD_MenuTransitions.setValue(Mathf.Clamp(1 - menuFadeDownTimer / menuFadeDownLength, 0,1));
				print (menuFadeDownTimer / menuFadeDownLength);
			}
			
			else {
				//FMOD_MenuTransitions.setValue(0);
				menuFadeDown = false;
				menuFadeDownTimer = 0.0f;
			}
		}
	}

	public void StartGame() {
		//FMOD_Tutorial.setValue (1);
	}

	public void PlaneDeath() {
		playerDead = true;
	}

	public void ResumeGame() {
				if (isplaying) {
					fadeOutOfDeath = true;
					//FMOD_Time.setValue (0);
				}
		isplaying = true;
		}

	public void SetGameplayTime (int score) {

		if (score == targetScoreOne) {
						fadeUpTime = true;
						fadeUpTargetValue = 0.1f;
		} else if (score == targetScoreTwo) {
						fadeUpTime = true;
						fadeUpTargetValue = 0.2f;
		} else if (score == targetScoreThree) {
						fadeUpTime = true;
						fadeUpTargetValue = 0.3f;
		} else if (score == targetScoreFour) {
						fadeUpTime = true;
						fadeUpTargetValue = 0.4f;
		}


	}

	public void MenuTransition(bool enter) {
		if (enter) {
			menuFadeUp = true;
			menuFadeDown = false;
		} else {
			menuFadeDown = true;
			menuFadeUp = false;
		}
	}
}
