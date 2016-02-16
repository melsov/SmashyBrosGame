using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    protected Rigidbody rb;
    protected bool grounded;
    public float moveSpeed = 2f;
    public float maxMoveSpeed = 3f;
    public float jumpHeight = 7f;

    void Awake () {
        rb = GetComponentInChildren<Rigidbody>();
	}
	
	void Update () {
        if (grounded) {
            if (Input.GetKeyDown(KeyCode.W)) {
                rb.AddForce(new Vector3(0f, jumpHeight, 0f), ForceMode.VelocityChange);
                grounded = false;
            }
        }
        rb.AddForce(new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0f, 0f), ForceMode.VelocityChange);
        Vector3 vel = rb.velocity;
        float xVel = Mathf.Clamp(vel.x, -1f * maxMoveSpeed, maxMoveSpeed);
        vel.x = xVel;
        rb.velocity = vel;
        
	}

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Environment")) {
            grounded = true;
        }
    }
}
