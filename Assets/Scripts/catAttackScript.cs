using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catAttackScript : MonoBehaviour
{
    [Header("Settings")]
    public float coolDownDuration;
    public float attackDuration;

    private float coolDown;
    private float attackTime;
    private bool isAttacking = false;
    private GameObject player;
    private float attackDistance;
    private catNavigation navScript;
    private BoxCollider attackCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        navScript = GetComponent<catNavigation>();
        attackDistance = navScript.stopDistance;
        attackCollider = transform.GetChild(0).GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, navScript.target.transform.position);

        if (distance <= attackDistance && !isAttacking) {
        	attackCollider.enabled = true;
        	isAttacking = true;
        	coolDown = coolDownDuration;
        	attackTime = attackDuration;
        }

        if (coolDown > 0) {
        	coolDown--;
        }
        if (coolDown == 0) {
        	isAttacking = false;
        }

        if (attackTime > 0) {
        	attackTime--;
        }
        if (attackTime == 0) {
        	attackCollider.enabled = false;
        }
    }
}
