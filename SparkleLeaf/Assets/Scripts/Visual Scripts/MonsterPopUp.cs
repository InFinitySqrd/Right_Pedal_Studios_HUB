using UnityEngine;
using System.Collections;

public class MonsterPopUp : MonoBehaviour {
    // Declare public variables
    public bool flipped = false;

    // Declare variables
    [SerializeField] float flipDistance = 5.0f;
    [SerializeField] float flipSpeed = 2.0f;
    [SerializeField] float pivotPointYDistance = 2.0f;
    private Transform player;
    private Transform gateParent;
    private Vector3 pivotPoint, differenceVector;
    private GameObject pivotObject;

	private Animator animate;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;

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
		//pivotObject.AddComponent<DestroyEmpty>();
	}
	
	// Update is called once per frame
	void Update () {
        //this.transform.position = gateParent.position;
        pivotObject.transform.position = gateParent.position + differenceVector;

        /*if (!flipped) {
            pivotObject.transform.position = pivotPoint;
        }*/

        if (!flipped && Vector3.Distance(gateParent.transform.position, player.position) <= flipDistance) {
            Flip();
        }
	}

    private void Flip() {
		if (pivotObject.transform.eulerAngles.x >= 0.0f && pivotObject.transform.eulerAngles.x <= 90.0f) {
			pivotObject.transform.Rotate(Vector3.left, Time.deltaTime * flipSpeed);
		} else {
			flipped = true;
			animate.SetTrigger("TriggerSpawnAnim");
			this.transform.parent = gateParent;
		}
    }
}
