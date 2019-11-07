using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrierScript : MonoBehaviour
{
    [Header("Runtime Variables")]
    public bool noPlanks = false;
    public bool isInteractable = false;
    public int destructionDelayDuration;

    private Animator[] planks;
    private int currentPlank;
    private int delay = 0;

    // Start is called before the first frame update
    void Start()
    {
        planks = new Animator[5];

        for (int x = 0; x < 5; x++) {
        	planks[x] = transform.GetChild(x).GetComponent<Animator>();
        }

        currentPlank = 4;
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

        if (currentPlank < 4 && delay == 0) {
        	isInteractable = true;
        }
        else {
        	isInteractable = false;
        }

        if (delay > 0) {
        	delay--;
        }
    }

    void KnockOffPlank() {
	    if (delay == 0) {
	    	planks[currentPlank].Play("off");

	    	currentPlank--;

	    	delay = destructionDelayDuration;
	    }
    }

    void AddPlank() {
    	currentPlank++;

    	planks[currentPlank].Play("on");

    	GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>().AddPoints(GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>().pointsOnRebuild);
    }

    public void Interact() {
    	AddPlank();
    }

    void OnTriggerEnter(Collider other) {
    	if (other.gameObject.CompareTag("catAttackCollider")) {
    		KnockOffPlank();
    	}
    }
}
