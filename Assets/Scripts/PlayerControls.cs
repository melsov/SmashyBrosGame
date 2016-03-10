using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System;

public class PlayerControls : MonoBehaviour {

    protected Rigidbody rb;
    protected Transform fist;
    protected Collider fistMesh;
    protected bool grounded;
    public float moveSpeed = 2f;
    public float maxMoveSpeed = 3f;
    public float jumpHeight = 7f;
    public bool isPlayerOne = true;
    public float punchPower = 88f;

    protected string horizontal;
    protected KeyCode jumpKey;
    protected KeyCode punchKey;
    protected PlayerControls otherGuysControls;

    public float punchTimeSeconds = .3f;
    protected float punchTimer;
    private float punchMassTax = .2f;
    private float punchDirection;
    public float punchSensitivity = 5f;
    public float sensitivityIncrement = 5f;
    private float reactToAPunchTime;

    private Collider _body;
    public Collider body {
        get { return _body; }
    }

    void Awake () {
        rb = GetComponentInChildren<Rigidbody>();

        foreach(Transform t in GetComponentsInChildren<Transform>()) {
            if (t.CompareTag("Fist")) {
                fist = t;
                fistMesh = fist.GetComponentInChildren<Collider>();
                break;
            }
        }
        Assert.IsTrue(fist != null, "oh no. we didn't find our fist transform!");
        fist.gameObject.SetActive(false);

        if (isPlayerOne) {
            horizontal = "Horizontal";
            jumpKey = KeyCode.W;
            punchKey = KeyCode.E;
            otherGuysControls = GameObject.Find("PlayerTwo").GetComponent<PlayerControls>();
        } else {
            horizontal = "Horizontal2";
            jumpKey = KeyCode.I;
            punchKey = KeyCode.O;
            otherGuysControls = GameObject.Find("PlayerOne").GetComponent<PlayerControls>();
        }
        Assert.IsTrue(otherGuysControls != null, "what???! didn't find the other guy");

        foreach(Collider c in GetComponentsInChildren<Collider>()) {
            if (c.CompareTag("Body")) {
                _body = c;
                break;
            }
        }
        Assert.IsTrue(_body != null, "huh?? didn't find body collider.");
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

        if (Mathf.Abs(Input.GetAxis(horizontal)) > Mathf.Epsilon) {
            float yAxisRotation = 90f - ( Mathf.Sign(Input.GetAxis(horizontal)) * 90f);
            rb.MoveRotation(Quaternion.Euler(0f, yAxisRotation, 0f));
        }



        if (reactToAPunchTime > 0f) {
            rb.AddForce(new Vector3(punchDirection * (punchPower + punchSensitivity) * Time.deltaTime * 20f, 0f, 0f), ForceMode.Force);
            reactToAPunchTime -= Time.deltaTime;
        }

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
        checkPunchConnected();
    }

    protected void checkPunchConnected() {
        if (fistMesh.bounds.Intersects(otherGuysControls.body.bounds)) {
            otherGuysControls.getPunched(fist.transform);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Environment")) {
            grounded = true;
        }

    }

    void OnTriggerEnter(Collider other) {
        //if(other.gameObject.CompareTag("FistMesh")) {
        //    if (other.transform.parent.gameObject != fist) {
        //        //getPunched(other.transform);
        //    } 
        //}
        if (other.gameObject.CompareTag("DeathArea")) {
            loseRound();
        }
    }

    private void loseRound() {
        throw new NotImplementedException();
    }

    protected void getPunched(Transform fist) {
        Vector3 difference = transform.position - fist.position;
        punchDirection = Mathf.Sign(difference.x);
        reactToAPunchTime = .2f;
        punchSensitivity += sensitivityIncrement;
    }
}
