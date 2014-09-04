using UnityEngine;
using System.Collections;

public class DebugControls : MonoBehaviour
{
    // Declare variables
    public bool paused = false;

    // Serialized plane maxSliderValues
    [SerializeField] float moveMax = 10.0f;
    [SerializeField] float rotateMax = 10.0f;
    [SerializeField] float maxRotateMax = 10.0f;
    [SerializeField] float momentumReduxMax = 10.0f;
    [SerializeField] float maxMomentumMax = 10.0f;
    [SerializeField] float levelingForceMax = 10.0f;
    [SerializeField] float levelingDampMax = 10.0f;
    [SerializeField] float levelingDelayMax = 10.0f;
    [SerializeField] float oppositeDirMax = 10.0f;

    // Serialized gate maxSliderValues
    [SerializeField] float spawnTimeMax = 10.0f;
    [SerializeField] float spawnDistanceMax = 10.0f;
    [SerializeField] float rotatePercentageMax = 1.0f;
    [SerializeField] float minRotationSpeedMax = 10.0f;
    [SerializeField] float maxRotationSpeedMax = 10.0f;

    private bool debugWindow = false;
    private bool planeWindow = true;
    private PlaneMovement planeVars;
    private SpawnGates gateVars;

    void Awake()
    {
        planeVars = this.GetComponent<PlaneMovement>();
        gateVars = this.GetComponent<SpawnGates>();
    }

    // Use this for initialization
    void Start()
    {
        // Initialise plane values from player prefs
        planeVars.forwardSpeed = PlayerPrefs.GetFloat("movement");
        planeVars.rotationSpeed = PlayerPrefs.GetFloat("rotation");
        planeVars.maxRotationSpeed = PlayerPrefs.GetFloat("maxRotate");
        planeVars.momentumReduction = PlayerPrefs.GetFloat("momentumRedux");
        planeVars.maxMomentum = PlayerPrefs.GetFloat("maxMomentum");
        planeVars.levelingForce = PlayerPrefs.GetFloat("levelingForce");
        planeVars.levelingDampener = PlayerPrefs.GetFloat("levelingDamp");
        planeVars.levelingDelay = PlayerPrefs.GetFloat("levelingDelay");
        planeVars.oppositeDirectionPush = PlayerPrefs.GetFloat("oppositeDirPush");

        // Initialise gate values from player prefs
        gateVars.spawnTime = PlayerPrefs.GetFloat("spawnTime");
        gateVars.spawnDistance = PlayerPrefs.GetFloat("spawnDistance");
        gateVars.percentageToRotate = PlayerPrefs.GetFloat("rotationPercentage");
        gateVars.minSpeed = PlayerPrefs.GetFloat("minRotationSpeed");
        gateVars.maxSpeed = PlayerPrefs.GetFloat("maxRotationSpeed");
    }

    void OnGUI()
    {
        if (!debugWindow)
        {
            paused = false;

            if (Input.touchCount >= 3 || Input.GetKeyDown(KeyCode.Delete))
            {
                debugWindow = true;
            }
        }
        else
        {
            paused = true;

            // Draw all control options
            if (planeWindow)
            {
                // Draw controls for the plane variables
                DrawControlOptions();
                DrawSliders(0, "MoveSpeed", ref planeVars.forwardSpeed, moveMax);
                DrawSliders(1, "RotateSpeed", ref planeVars.rotationSpeed, rotateMax);
                DrawSliders(2, "MaxRotation", ref planeVars.maxRotationSpeed, maxRotateMax);
                DrawSliders(3, "MomentumReduct", ref planeVars.momentumReduction, momentumReduxMax);
                DrawSliders(4, "MaxMomentum", ref planeVars.maxMomentum, maxMomentumMax);
                DrawSliders(5, "LevelingForce", ref planeVars.levelingForce, levelingForceMax);
                DrawSliders(6, "LevelingDamp", ref planeVars.levelingDampener, levelingDampMax);
                DrawSliders(7, "LevelingDelay", ref planeVars.levelingDelay, levelingDelayMax);
                DrawSliders(8, "OppositeDirPush", ref planeVars.oppositeDirectionPush, oppositeDirMax);
            }
            else
            {
                // Draw controls for the gate spawning variables
                DrawSliders(0, "SpawnTime", ref gateVars.spawnTime, spawnTimeMax);
                DrawSliders(1, "SpawnDistance", ref gateVars.spawnDistance, spawnDistanceMax);
                DrawSliders(2, "RotatePercentage", ref gateVars.percentageToRotate, rotatePercentageMax);
                DrawSliders(3, "MinRotateSpeed", ref gateVars.minSpeed, minRotationSpeedMax);
                DrawSliders(4, "MaxRotateSpeed", ref gateVars.maxSpeed, maxRotationSpeedMax);
            }

            // Draw a control to switch between windows in the debug menu
            if (planeWindow)
            {
                if (GUI.Button(new Rect(0.0f, Screen.height - Screen.height / 8.0f, Screen.width / 4.0f, Screen.height / 8.0f), "Gate Controls"))
                {
                    planeWindow = false;
                }
            }
            else
            {
                if (GUI.Button(new Rect(0.0f, Screen.height - Screen.height / 8.0f, Screen.width / 4.0f, Screen.height / 8.0f), "Plane Controls"))
                {
                    planeWindow = true;
                }
            }

            // Draw the close button
            if (GUI.Button(new Rect(Screen.width - Screen.width / 4.0f, Screen.height - Screen.height / 8.0f, Screen.width / 4.0f, Screen.height / 8.0f), "Close Window"))
            {
                debugWindow = false;

                // Save all values to player prefs on close
                // Set values for the plane variables
                PlayerPrefs.SetFloat("movement", planeVars.forwardSpeed);
                PlayerPrefs.SetFloat("rotation", planeVars.rotationSpeed);
                PlayerPrefs.SetFloat("maxRotate", planeVars.maxRotationSpeed);
                PlayerPrefs.SetFloat("momentumRedux", planeVars.momentumReduction);
                PlayerPrefs.SetFloat("maxMomentum", planeVars.maxMomentum);
                PlayerPrefs.SetFloat("levelingForce", planeVars.levelingForce);
                PlayerPrefs.SetFloat("levelingDamp", planeVars.levelingDampener);
                PlayerPrefs.SetFloat("levelingDelay", planeVars.levelingDelay);
                PlayerPrefs.SetFloat("oppositeDirPush", planeVars.oppositeDirectionPush);

                // Set values for the gate variables
                PlayerPrefs.SetFloat("spawnTime", gateVars.spawnTime);
                PlayerPrefs.SetFloat("spawnDistance", gateVars.spawnDistance);
                PlayerPrefs.SetFloat("rotationPercentage", gateVars.percentageToRotate);
                PlayerPrefs.SetFloat("minRotationSpeed", gateVars.minSpeed);
                PlayerPrefs.SetFloat("maxRotationSpeed", gateVars.maxSpeed);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DrawControlOptions()
    {
        // Draw different control options that are available
        if (GUI.Button(new Rect(0, 0, Screen.width / 3.0f, Screen.height / 8.0f), "Hold Rot."))
        {
            planeVars.controlMethod = 1;
            PlayerPrefs.SetInt("Control Method", 1);
        }

        if (GUI.Button(new Rect(Screen.width / 3.0f, 0, Screen.width / 3.0f, Screen.height / 8.0f), "Slide Rot."))
        {
            planeVars.controlMethod = 2;
            PlayerPrefs.SetInt("Control Method", 2);
        }

        if (GUI.Button(new Rect(2.0f * Screen.width / 3.0f, 0, Screen.width / 3.0f, Screen.height / 8.0f), "Tilt Rot."))
        {
            planeVars.controlMethod = 3;
            PlayerPrefs.SetInt("Control Method", 3);
        }
    }

    private void DrawSliders(int lineNum, string labelName, ref float editedVar, float sliderMaxVal)
    {
        // Draw a slider on the screen to make a variable editable on device
        GUI.Box(new Rect(0.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f, Screen.width / 6.0f, Screen.height / 12.0f), labelName);

        float sliderValue = editedVar;
        editedVar = GUI.HorizontalSlider(new Rect(Screen.width / 6.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f, Screen.width - Screen.width / 3.0f, Screen.height / 12.0f), sliderValue, 0.0f, sliderMaxVal);

        GUI.Box(new Rect(Screen.width - Screen.width / 6.0f, Screen.height / 8.0f + lineNum * Screen.height / 12.0f, Screen.width / 6.0f, Screen.height / 12.0f), editedVar.ToString());
    }
}
