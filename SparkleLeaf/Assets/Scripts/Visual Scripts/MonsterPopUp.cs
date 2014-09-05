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

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

	// Use this for initialization
	void Start () { 	    
        // Get the transform of the gate
        gateParent = this.transform.parent;

        // Set pivot point position
        pivotObject = new GameObject();
        pivotPoint = new Vector3(gateParent.transform.position.x, gateParent.transform.position.y - pivotPointYDistance, gateParent.transform.position.z);
        differenceVector = gateParent.transform.position - pivotPoint;
        pivotObject.transform.position = pivotPoint;
        
        // Set the sprite's object to follow the pivot point
        this.transform.parent = pivotObject.transform;

        // Set starting rotation
        pivotObject.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);

        pivotObject.AddComponent<DestroyEmpty>();
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = gateParent.transform.position;
        pivotPoint = gateParent.transform.position + differenceVector;
        pivotObject.transform.position = pivotPoint;

        if (Vector3.Distance(gateParent.transform.position, player.position) <= flipDistance) {
            Flip();
        }
	}

    private void Flip() {
        if (pivotObject.transform.eulerAngles.x >= 0.0f && pivotObject.transform.eulerAngles.x <= 90.0f) {
            pivotObject.transform.Rotate(Vector3.left, Time.deltaTime * flipSpeed);
        } else {
            flipped = true;
            this.transform.parent = gateParent;
        }
    }
}
