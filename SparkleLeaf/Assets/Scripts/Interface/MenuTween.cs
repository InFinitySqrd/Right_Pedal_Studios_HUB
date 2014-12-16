using UnityEngine;
using System.Collections;

public class MenuTween : MonoBehaviour {
    // Declare variables
    [SerializeField] bool downTween = true;
    [SerializeField] float tweenSpeed = 1.0f;
    public float fadeSpeed = 0.6f;

	[SerializeField] float tweenLocationModifier = 0.0f;
    [SerializeField] bool subMenu = false;

    private Vector3 initialPos;
    private Vector3 fromPos;
    private bool tween = false;
    private bool reverse = false;

	// Use this for initialization
	void Start () {
        initialPos = this.transform.position;
        this.renderer.material.color = new Color(this.renderer.material.color.r, this.renderer.material.color.g, this.renderer.material.color.b, 0.0f);

        if (downTween) {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + this.transform.localScale.y / 2.0f, this.transform.position.z);
        } else {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - this.transform.localScale.y / 2.0f, this.transform.position.z);
        }

        fromPos = this.transform.position;

        if (!subMenu) {
            TriggerForwardTween();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (tween) {
	        //this.transform.position = Vector3.MoveTowards(this.transform.position, initialPos, Time.deltaTime * tweenSpeed);
			this.transform.position = Vector3.Lerp(this.transform.position, initialPos, Time.deltaTime * tweenSpeed);
            this.renderer.material.color = new Color(this.renderer.material.color.r, this.renderer.material.color.g, this.renderer.material.color.b, this.renderer.material.color.a + Time.deltaTime * fadeSpeed);
        } else if (reverse) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, fromPos, Time.deltaTime * tweenSpeed);
            this.renderer.material.color = new Color(this.renderer.material.color.r, this.renderer.material.color.g, this.renderer.material.color.b, this.renderer.material.color.a - Time.deltaTime * fadeSpeed);
        }

		//if (this.collider.name == "Settings" && this.renderer.material.color.a > 0.95f) {
		//	this.renderer.material.color = new Color(this.renderer.material.color.r, this.renderer.material.color.g, this.renderer.material.color.b, 1.0f);
		//	this.GetComponent<MenuTween>().enabled = false;
		//}
    }

    public void TriggerForwardTween() {
        tween = true;
        reverse = false;

        this.renderer.material.color = new Color(this.renderer.material.color.r, this.renderer.material.color.g, this.renderer.material.color.b, 0.0f);
    }

    public void TriggerReverseTween() {
        tween = false;
        reverse = true;

        this.renderer.material.color = new Color(this.renderer.material.color.r, this.renderer.material.color.g, this.renderer.material.color.b, 1.0f);
    }
}
