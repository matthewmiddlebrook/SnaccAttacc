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

    private gameManager managerScript;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        navScript = GetComponent<catNavigation>();
        attackDistance = navScript.stopDistance;
        attackCollider = transform.GetChild(0).GetComponent<BoxCollider>();

        managerScript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
        audioSource = GameObject.FindGameObjectWithTag("catAudio").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!managerScript.isPaused) {
            float distance = Vector3.Distance(transform.position, navScript.target.transform.position);

            if (distance <= attackDistance && !isAttacking) {
                attackCollider.enabled = true;
                isAttacking = true;
                coolDown = coolDownDuration;
                attackTime = attackDuration;

                if (!audioSource.isPlaying) {
                    audioSource.clip = managerScript.catMeowSounds[Random.Range(0,managerScript.catMeowSounds.Length)];
                	audioSource.Play();
				}
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
}
