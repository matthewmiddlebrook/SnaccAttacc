using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class catAnimationController : MonoBehaviour
{
    [Header("Settings")]
    public float moveThreshold;

    private NavMeshAgent cc;
    private Animator anim;
    private bool attackPlaying;
    private catAttackScript attackScript;
    private catHealth healthScript;
    private bool isRunning = false;

    private gameManager managerScript;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cc = transform.parent.gameObject.GetComponent<NavMeshAgent>();
        attackScript = transform.parent.gameObject.GetComponent<catAttackScript>();
        healthScript = transform.parent.gameObject.GetComponent<catHealth>();

        managerScript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!managerScript.isPaused) {
            anim.enabled = true;
            float currentSpeed = cc.velocity.magnitude;
            if (attackScript.isAttacking && !attackPlaying && healthScript.currentHealth > 0) {
                anim.Play("Attack");
                attackPlaying = true;
            }
            if (!attackScript.isAttacking) {
                attackPlaying = false;
                if (isRunning)
                    anim.Play("Run");
                else
                    anim.Play("Static");
            }
            if (currentSpeed > moveThreshold) {
                if (!isRunning) {
                    anim.Play("Run");
                    print("Cat Running");
                    isRunning = true;
                }
            }
            else if (currentSpeed <= moveThreshold) {
                if (isRunning) {
                    anim.Play("Static");
                    isRunning = false;
                }
            }
        } else {
             anim.enabled = false;
        }
    }
}
