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

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = transform.parent.gameObject.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = rb.velocity.magnitude;

        if (currentSpeed > moveThreshold) {
        	anim.Play("run");
        }
        else if (currentSpeed <= moveThreshold) {
        	anim.Play("idle");
        }
    }
}
