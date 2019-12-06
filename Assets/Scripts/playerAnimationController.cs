using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimationController : MonoBehaviour
{
    [Header("Settings")]
    public float moveThreshold;

    private CharacterController cc;
    private Animator anim;
    private bool attackPlaying;
    private playerAttackController attackScript;
    private playerHealth healthScript;
    private bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cc = transform.parent.gameObject.GetComponent<CharacterController>();
        attackScript = transform.parent.gameObject.GetComponent<playerAttackController>();
        healthScript = transform.parent.gameObject.GetComponent<playerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = cc.velocity.magnitude;
        if (attackScript.isAttacking && !attackPlaying && healthScript.isAlive) {
        	anim.Play("throw");
        	attackPlaying = true;
        }
        if (!attackScript.isAttacking) {
        	attackPlaying = false;
            if (isRunning)
                anim.Play("run");
            else
                anim.Play("idle");
        }
        if (currentSpeed > moveThreshold) {
            if (!isRunning) {
                anim.Play("run");
                print("Running");
                isRunning = true;
            }
        }
        else if (currentSpeed <= moveThreshold) {
            if (isRunning) {
                anim.Play("idle");
                isRunning = false;
            }
        }
    }
}
