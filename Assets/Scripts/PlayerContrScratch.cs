using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class PlayerContrScratch : MonoBehaviour {

    protected Rigidbody rb;
    protected Transform fist;
    protected bool grounded;
    public float moveSpeed = 2f;
    public float maxMoveSpeed = 3f;
    public float jumpHeight = 7f;
    public bool isPlayerOne = true;
    public float punchPower = 88f;

    protected string horizontal;
    protected KeyCode jumpKey;
    protected KeyCode punchKey;

    public float punchTimeSeconds = .3f;
    protected float punchTimer;
    private float punchMassTax = .2f;
    private float reactToAPunchTime = 0f;
    private float punchDirection;
    public float punchSensitivity = 5f;
    public float sensitivityIncrement = 5f;

    void Awake () {
        //TODO: use GetComponent to set rb to the rigidbody
        //rb = ...

        // look through all children of the player game object
        // find the one with the "Fist" tag
        foreach(Transform t in GetComponentsInChildren<Transform>()) {
            if (t.CompareTag("Fist")) {
                fist = t;
                break;
            }
        }
        //be paranoid: make a fuss if we didn't find a transform with the "Fist" tag
        Assert.IsTrue(fist != null, "oh no. we didn't find our fist transform!");

        //hide the fist
        //TODO: set the fist's game object to inactive


        if (isPlayerOne) {
            horizontal = "Horizontal";
            jumpKey = KeyCode.W;
            punchKey = KeyCode.E;
        } else {
           // TODO: if this is player two
           // set horizontal to "Horizontal2"
           // set jumpKey to I
           // punchKey to E
        }
	}
	
	void Update () {
        // TODO: if grounded is true
        // and if the jump key is down...
        //     add an upward force to the rb. The force should be equal to 'jumpHeight' in the vertical direction and zero in the other directions
        //     set grounded to false

        //TODO: add a horizontal force that's proportional to horizontal axis input * moveSpeed 
        //(no need for an if statement with this one: if the player isn't pressing either of the horizontal keys
        //Input.GetAxis() will return zero and you'll go nowhere)

        //TODO: set a speed limit for horizontal movement.
        // find out what the current velocity of the rb is: put the current rb.velocity into a new vec3 called vel
        // look up Mathf.Clamp in Unity's online documentation and ...
        // use it to set vel.x to either itself or
        // -maxMoveSpeed if it's less than -maxMoveSpeed, +maxMoveSpeed if it's greater than +maxMoveSpeed.


	}

    protected void punch() {
        fist.gameObject.SetActive(true);
        punchTimer = Time.fixedTime;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Environment")) {
            grounded = true;
        }

    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("FistMesh")) {
            if (other.transform.parent.gameObject != fist) {
                getPunched(other.transform);
            } 
        }
    }

    protected void getPunched(Transform fist) {
        Vector3 difference = transform.position - fist.position;
        punchDirection = Mathf.Sign(difference.x);
        reactToAPunchTime = .3f;
        punchSensitivity += sensitivityIncrement;
    }
}
