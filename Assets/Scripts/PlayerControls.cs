using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    protected Rigidbody rb;
    protected Transform fist;
    protected bool grounded;
    public float moveSpeed = 2f;
    public float maxMoveSpeed = 3f;
    public float jumpHeight = 7f;
    public bool isPlayerOne = true;
    public float punchPower = 1f;

    protected string horizontal;
    protected KeyCode jumpKey;
    protected KeyCode punchKey;

    public float punchTimeSeconds = .3f;
    protected float punchTimer;

    void Awake () {
        rb = GetComponentInChildren<Rigidbody>();

        foreach(Transform t in GetComponentsInChildren<Transform>()) {
            if (t.CompareTag("Fist")) {
                fist = t;
                break;
            }
        }
        Assert.IsTrue(fist != null, "oh no. we didn't find our fist transform!");
        fist.gameObject.SetActive(false);

        if (isPlayerOne) {
            horizontal = "Horizontal";
            jumpKey = KeyCode.W;
            punchKey = KeyCode.E;
        } else {
            horizontal = "Horizontal2";
            jumpKey = KeyCode.I;
            punchKey = KeyCode.O;
        }
	}
	
	void Update () {
        if (grounded) {
            if (Input.GetKeyDown(jumpKey)) {
                rb.AddForce(new Vector3(0f, jumpHeight, 0f), ForceMode.Force);
                grounded = false;
            }
        }
        rb.AddForce(new Vector3(Input.GetAxis(horizontal) * moveSpeed, 0f, 0f), ForceMode.Force);

        Vector3 vel = rb.velocity;
        vel.x = Mathf.Clamp(vel.x, -1f * maxMoveSpeed, maxMoveSpeed);
        rb.velocity = vel;

        float timeSinceLast = Time.fixedTime - punchTimer;
        if (timeSinceLast > punchTimeSeconds) {
            fist.gameObject.SetActive(false);
            if (Input.GetKeyDown(punchKey)) {
                punch();
            }
        }
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
                print("got punched");
                getPunched(other.transform);
            } else {
                print("our own fist");
            }
        }
    }

    protected void getPunched(Transform fist) {
        
    }
}
