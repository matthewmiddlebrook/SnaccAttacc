﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class catNavigation : MonoBehaviour
{
    [Header("Settings")]
    public float stopDistance;

    [Header("Runtime Variables")]
  	public GameObject target;
    public GameObject spawn;

    private NavMeshAgent nav;
    private GameObject player;
    private float distance;
    private GameObject barrier;
    private catHealth healthScript;

    private gameManager managerScript;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("barrier");
        GameObject closest = tmp[0];
        for (int x = 0; x < tmp.Length; x++) {
        	if (Vector3.Distance(transform.position, tmp[x].transform.position) <
        		Vector3.Distance(transform.position, closest.transform.position)) {
        		closest = tmp[x];
        	}
        }
        
        barrier = closest;
        player = GameObject.FindGameObjectWithTag("player");
        target = barrier;

        managerScript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
        nav = GetComponent<NavMeshAgent>();
        if (managerScript.difficulty == 0) {
            nav.speed = managerScript.kittenSpeed;
        } else if (managerScript.difficulty == 1) {
            nav.speed = managerScript.catSpeed;
        } else {
            nav.speed = managerScript.wildcatSpeed;
        }

        healthScript = GetComponent<catHealth>();
        StartCoroutine("TargetingLoop");

    }

    // Update is called once per frame
    void Update()
    {
        if (!managerScript.isPaused) {
            distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance <= stopDistance) {
                nav.isStopped = true;
            }
            else {
                nav.isStopped = false;
            }

            if (healthScript.currentHealth <= 0) {
                target = spawn;
            }
            else if (barrier.GetComponent<barrierScript>().noPlanks) {
                target = player;
            }
            else {
                target = barrier;
            }
        } else {
            nav.isStopped = true;
        }
    }

    IEnumerator TargetingLoop() {
    	while (true) {
    		nav.SetDestination(target.transform.position);

    		yield return new WaitForSeconds(0.2f);
    	}
    }
}
