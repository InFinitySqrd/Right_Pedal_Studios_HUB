using UnityEngine;
using System.Collections;

public class MonsterPopUp : MonoBehaviour {
    // Declare public variables
    public bool flipped = false;

    // Declare variables
    [SerializeField] float flipDistance = 5.0f;
    [SerializeField] float flipSpeed = 2.0f;
    [SerializeField] float pivotPointYDistance = 2.0f;
	[SerializeField] float jiggleSpeed = 2.0f;
	[SerializeField] float firstJiggleRotation = 4.0f;
	[SerializeField] float jiggleDecrementor = 0.5f;
	[SerializeField] float jiggleCutOff = 0.5f;
    private Transform player;
    private PlaneMovement planeVars;
    private Transform gateParent;
    private Vector3 pivotPoint, differenceVector;
    private GameObject pivotObject;

	private Animator animate;

	private bool jiggleForward = true;
	private bool jiggled = false;
	private float jiggleRotation;

    private bool spawnAnimTriggered = false;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        planeVars = player.GetComponent<PlaneMovement>();
		jiggleRotation = firstJiggleRotation;
		animate = this.GetComponent<Animator>();
    }

	// Use this for initialization
	void Start () { 	    
        // Get the transform of the gate
        gateParent = this.transform.parent;

		// Set up an object to pivot around
		pivotObject = new GameObject();
		pivotObject.transform.position = Vector3.zero;

        // Set the position of this object
        pivotPoint = new Vector3(gateParent.transform.position.x, gateParent.transform.position.y + pivotPointYDistance, gateParent.transform.position.z);
		differenceVector = pivotPoint - gateParent.transform.position;
        pivotObject.transform.position = pivotPoint;
        
        // Set the model as a child of the pivoting object
        this.transform.parent = pivotObject.transform;

        // Set starting rotation
        pivotObject.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);

        // Destroy this new object when the model is no longer a child
		pivotObject.AddComponent<DestroyEmpty>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!jiggled) {	
			pivotObject.transform.position = gateParent.position + differenceVector;

            if (!spawnAnimTriggered && this.name == "lineMonster") {
                animate.SetTrigger("TriggerSpawnAnim");
                spawnAnimTriggered = true;
            }
		}

        if (!flipped && Vector3.Distance(gateParent.transform.position, player.position) <= flipDistance) {
            Flip();
        }

		if (flipped && !jiggled) {
			StartCoroutine(Jiggle());
		}
	}

    private void Flip() {
		if (pivotObject.transform.eulerAngles.x >= 0.0f && pivotObject.transform.eulerAngles.x <= 90.0f) {
			pivotObject.transform.Rotate(Vector3.left, Time.deltaTime * flipSpeed * planeVars.forwardSpeed);
		} else {
			flipped = true;
			pivotObject.transform.eulerAngles = Vector3.zero;

            if (this.name == "crossMonster") {
                animate.SetTrigger("TriggerSpawnAnim");
            }
		}
    }

	IEnumerator Jiggle() {
		if (jiggleForward) {
			if (pivotObject.transform.eulerAngles.x >= 360.0f - jiggleRotation || pivotObject.transform.eulerAngles.x <= 0.0f + jiggleRotation / jiggleDecrementor) {
				pivotObject.transform.Rotate(Vector3.left, Time.deltaTime * jiggleSpeed * planeVars.forwardSpeed);
			} else {
				pivotObject.transform.eulerAngles = new Vector3(360.0f - jiggleRotation, 0.0f, 0.0f);

				jiggleForward = false;
				jiggleRotation *= jiggleDecrementor;
			}
		} else {
			if (pivotObject.transform.eulerAngles.x >= 360.0f - jiggleRotation / jiggleDecrementor || pivotObject.transform.eulerAngles.x <= 0.0f + jiggleRotation) {
				pivotObject.transform.Rotate(Vector3.right, Time.deltaTime * jiggleSpeed * planeVars.forwardSpeed);
			}else {
				pivotObject.transform.eulerAngles = new Vector3(0.0f + jiggleRotation, 0.0f, 0.0f);

				jiggleForward = true;
				jiggleRotation *= jiggleDecrementor;
			}
		}

		if (jiggleRotation <= jiggleCutOff) {
			//StartCoroutine(SetRotationToDefault());
			this.transform.parent = gateParent;
			jiggled = true;
		}

		yield return null;
	}

	IEnumerator SetRotationToDefault() {
		if (pivotObject.transform.eulerAngles.x > 90.0f) {
			pivotObject.transform.Rotate(Vector3.left, Time.deltaTime * flipSpeed);
		} else if (pivotObject.transform.eulerAngles.x < 90.0f) {
			pivotObject.transform.Rotate(Vector3.right, Time.deltaTime * flipSpeed);
		}

		yield return null;
	}
}
