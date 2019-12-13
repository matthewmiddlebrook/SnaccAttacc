using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrierScript : MonoBehaviour
{
    [Header("Runtime Variables")]
    public bool noPlanks = false;
    public bool isInteractable = false;
    public float destructionDelayDuration;

    private Animator[] planks;
    private int currentPlank;
    private float delay = 0;

    private gameManager managerScript;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        planks = new Animator[5];

        for (int x = 0; x < 5; x++) {
        	planks[x] = transform.GetChild(x).GetComponent<Animator>();
        }

        currentPlank = 4;

        managerScript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlank < 0) {
        	noPlanks = true;
        }
        else {
        	noPlanks = false;
        }

        if (currentPlank < 4 && delay <= 0) {
        	isInteractable = true;
        }
        else {
        	isInteractable = false;
        }

        if (delay > 0) {
        	delay-=Time.deltaTime;
        }
    }

    void KnockOffPlank() {
	    if (delay <= 0 && currentPlank >= 0) {
	    	planks[currentPlank].Play("off");

	    	currentPlank--;
            managerScript.planksKnockedOff++;

	    	delay = destructionDelayDuration;

            audioSource.clip = managerScript.woodPlankSounds[Random.Range(0,managerScript.woodPlankSounds.Length)];
            audioSource.Play();
	    }
    }

    void AddPlank() {
    	currentPlank++;
        managerScript.planksPutUp++;

    	planks[currentPlank].Play("on");

        audioSource.clip = managerScript.woodPlankSounds[Random.Range(0,managerScript.woodPlankSounds.Length)];
        audioSource.Play();

    	GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>().AddPoints(GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>().pointsOnRebuild);
    }

    public void Interact() {
    	AddPlank();
    }

    void OnTriggerEnter(Collider other) {
        if (!managerScript.isPaused) {
            if (other.gameObject.CompareTag("catAttackCollider")) {
                KnockOffPlank();
            }
        }
    }
}
