using UnityEngine;
using System.Collections;

public class FMOD_Manager : MonoBehaviour {

	private static FMOD_Manager instance = null;
	public static FMOD_Manager Instance {
		get { return instance; }
	}

	private FMOD.Studio.EventInstance FMOD_InstanceGameplay, FMOD_InstanceRotation, FMOD_InstanceDeath, FMOD_InstanceForest, FMOD_InstanceCleared, FMOD_InstanceHit;
	private FMOD.Studio.ParameterInstance FMOD_MenuTransitions, FMOD_Tutorial, FMOD_Time, FMOD_Death, FMOD_RotationWind, FMOD_RotationPaper, FMOD_ForestDeath, FMOD_ClearedSpeed, FMOD_HitMonster;

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
	[SerializeField] int targetScoreFive = 0;
	[SerializeField] int targetScoreSix = 0;

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
		// FMOD Instances
		FMOD_InstanceGameplay = FMOD_StudioSystem.instance.GetEvent ("event:/Music/Gameplay");
		FMOD_InstanceRotation = FMOD_StudioSystem.instance.GetEvent ("event:/Character/Paper_Plane/Rotation");
		FMOD_InstanceDeath = FMOD_StudioSystem.instance.GetEvent ("event:/Character/Paper_Plane/Death");
		FMOD_InstanceForest = FMOD_StudioSystem.instance.GetEvent ("event:/Ambience/Forest");
		FMOD_InstanceCleared = FMOD_StudioSystem.instance.GetEvent ("event:/SFX/Shadow_Monsters/Cleared");
		FMOD_InstanceHit = FMOD_StudioSystem.instance.GetEvent ("event:/SFX/Shadow_Monsters/Hit");

		// FMOD Parameters - Gameplay
		FMOD_InstanceGameplay.getParameter ("Menu Transitions", out FMOD_MenuTransitions);
		FMOD_InstanceGameplay.getParameter ("Title/Tutorial", out FMOD_Tutorial);
		FMOD_InstanceGameplay.getParameter ("Time", out FMOD_Time);
		FMOD_InstanceGameplay.getParameter ("Death", out FMOD_Death);

		// FMOD Parameters - Rotation
		FMOD_InstanceRotation.getParameter ("Wind", out FMOD_RotationWind);
		FMOD_InstanceRotation.getParameter ("Paper", out FMOD_RotationPaper);

		// FMOD Parameters - Forest
		FMOD_InstanceForest.getParameter ("Death", out FMOD_ForestDeath);

		// FMOD Parameters - Cleared
		FMOD_InstanceCleared.getParameter ("Speed", out FMOD_ClearedSpeed);

		// FMOD Parameters - Hit
		FMOD_InstanceHit.getParameter ("Monster", out FMOD_HitMonster);

		// FMOD Start the player
		FMOD_InstanceGameplay.start ();
		FMOD_InstanceRotation.start ();
		FMOD_InstanceForest.start ();

	}
	
	// Update is called once per frame
	void Update () {
		if (playerDead) {
			if (deathTimer <= deathFadeLength) {
				deathTimer += Time.deltaTime;
				FMOD_Death.setValue (deathTimer * 2);
			} 
			else { 
				playerDead = false;
				deathTimer = 0;
			}
		}

		if (fadeOutOfDeath) {
			if (fadeOutTimer <= fadeFromDeathLength) {
				fadeOutTimer += Time.deltaTime;
				FMOD_Death.setValue(1 - fadeOutTimer);
			}

			else {
				FMOD_Death.setValue(0);
				fadeOutOfDeath = false;
				fadeOutTimer = 0.0f;
			}
		}

		if (fadeUpTime) {

			if (fadeUpTimer <= fadeUpFromScoreLength) {
				fadeUpTimer += Time.deltaTime;

				FMOD_Time.setValue(fadeUpTargetValue - 0.1f + (((fadeUpTimer / fadeUpFromScoreLength) / 10)));
				print (fadeUpTargetValue - 0.1f + (((fadeUpTimer / fadeUpFromScoreLength) / 10)));
			}

			else {
				fadeUpTimer = 0.0f;
				fadeUpTime = false;
				FMOD_Time.setValue(fadeUpTargetValue);
				print (fadeUpTargetValue);
			}
		}

		if (menuFadeUp) {
			if (menuFadeUpTimer <= menuFadeUpLength) {
				menuFadeUpTimer += Time.deltaTime;
				FMOD_MenuTransitions.setValue(Mathf.Clamp(menuFadeUpTimer / menuFadeUpLength, 0,1));
				print (menuFadeUpTimer / menuFadeUpLength);
			}

			else {
				FMOD_MenuTransitions.setValue(1);
				menuFadeUp = false;
				menuFadeUpTimer = 0.0f;
			}
		}

		if (menuFadeDown) {
			if (menuFadeDownTimer <= menuFadeDownLength) {
				menuFadeDownTimer += Time.deltaTime;
				FMOD_MenuTransitions.setValue(Mathf.Clamp(1 - menuFadeDownTimer / menuFadeDownLength, 0,1));
				print (menuFadeDownTimer / menuFadeDownLength);
			}
			
			else {
				FMOD_MenuTransitions.setValue(0);
				menuFadeDown = false;
				menuFadeDownTimer = 0.0f;
			}
		}
	}

	public void StartGame() {
		FMOD_Tutorial.setValue (1);
	}

	public void PlaneDeath(float monsterType) {
		// 0 = Line 1 = Cross
		playerDead = true;
		FMOD_StudioSystem.instance.PlayOneShot ("event:/Character/Paper_Plane/Death", GameObject.FindGameObjectWithTag("MainCamera").transform.position);
		FMOD_InstanceHit.start ();
		FMOD_HitMonster.setValue (monsterType);

	}

	public void ResumeGame() {
				if (isplaying) {
					fadeOutOfDeath = true;
					FMOD_Time.setValue (0);
			FMOD_RotationWind.setValue(0);
			FMOD_ForestDeath.setValue (0);
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
		} else if (score == targetScoreFive) {
						fadeUpTime = true;
						fadeUpTargetValue = 0.5f;
		} else if (score == targetScoreSix) {
						fadeUpTime = true;
						fadeUpTargetValue = 0.6f;
		}
		FMOD_InstanceCleared.start ();
		if (score > 20) {

						FMOD_ClearedSpeed.setValue (1);
				} else {
			FMOD_ClearedSpeed.setValue(0);
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

	public void WindRotation(float currentMomentum) {
		FMOD_RotationWind.setValue(Mathf.Abs(currentMomentum));
		print (Mathf.Abs(currentMomentum));
	}

	public void ForestSetDeath (bool dead) {
		if (dead) {
						FMOD_ForestDeath.setValue (1);
				} else {
						FMOD_ForestDeath.setValue (0);
				}
	}

	public void PaperPlaneDeath() {

		print ("Plane dead");
	}
}
