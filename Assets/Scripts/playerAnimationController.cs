using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimationController : MonoBehaviour
{
    [Header("Settings")]
    public float moveThreshold;

    private Rigidbody rb;
    private Animator anim;
    private bool attackPlaying;
    private playerAttackController attackScript;
    private playerHealth healthScript;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = transform.parent.gameObject.GetComponent<Rigidbody>();
        attackScript = transform.parent.gameObject.GetComponent<playerAttackController>();
        healthScript = transform.parent.gameObject.GetComponent<playerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = rb.velocity.magnitude;
        if (attackScript && attackScript.isAttacking && !attackPlaying && healthScript.isAlive) {
        	anim.Play("throw");
        	attackPlaying = true;
        }
        if (attackScript && !attackScript.isAttacking) {
        	attackPlaying = false;
        }
        if (currentSpeed > moveThreshold) {
        	anim.Play("run");
        }
        else if (currentSpeed <= moveThreshold) {
        	anim.Play("idle");
        }
    }
}
