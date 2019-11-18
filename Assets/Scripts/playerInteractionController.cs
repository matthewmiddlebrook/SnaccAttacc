using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class playerInteractionController : MonoBehaviour
{
    [Header("Settings")]
    public int repeatedInputDelayDuration;
    public float movementThreshold;

    [Header("Runtime Variables")]
    public GameObject activeObject;

    private int delay;
    private bool readyForInput;
    private string type;
    private GameObject interactTextObject;
    private playerAttackController attackScript;
    private gameManager managerScript;
    private bool isInteracting = false;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        interactTextObject = GameObject.FindGameObjectWithTag("interactTextObject");
        interactTextObject.SetActive(false);
        attackScript = GetComponent<playerAttackController>();
        managerScript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteracting && readyForInput && rb.velocity.magnitude < movementThreshold) {
        	if (type == "barrier" && activeObject.GetComponent<barrierScript>().isInteractable) {
        		activeObject.GetComponent<barrierScript>().Interact();
        	}
        	else if (type == "refillStation" && attackScript.emptyBalloonCount > 0) {
        		attackScript.FillBalloon();
        	}
        	else if (type == "blockage" &&
        		activeObject.GetComponent<blockage>().isInteractable &&
        		managerScript.points >= activeObject.GetComponent<blockage>().blockagePrice) {
        		activeObject.GetComponent<blockage>().ClearBlockage();
        	}
        	else if (type == "balloonStation" &&
        		managerScript.points >= managerScript.balloonPackCost) {
        		managerScript.PurchaseBalloons();
        	}
        	else if (type == "healthStation" &&
        		managerScript.points >= managerScript.snackCost &&
        		GetComponent<playerHealth>().currentHealth < GetComponent<playerHealth>().maxHealth) {
        		managerScript.PurchaseSnack();
        	} else
			if (type == "door" && activeObject.GetComponent<DoorInteraction>().isInteractable) {
        		activeObject.GetComponent<DoorInteraction>().Interact();
        	}

        	delay = repeatedInputDelayDuration;
        	readyForInput = false;
        }

        if (delay > 0) {
        	delay--;
        }
        if (delay == 0) {
        	readyForInput = true;
        }

        if (activeObject != null) {
        	if (type == "barrier" && activeObject.GetComponent<barrierScript>().isInteractable) {
        		interactTextObject.SetActive(true);
        		interactTextObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = "Hold SPACE to repair!";
        	}
        	else if (type == "refillStation" && attackScript.emptyBalloonCount > 0) {
        		interactTextObject.SetActive(true);
        		interactTextObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = "Hold SPACE to fill balloons!";
        	}
        	else if (type == "blockage" && activeObject.GetComponent<blockage>().isInteractable) {
        		interactTextObject.SetActive(true);
        		interactTextObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = 
        			"Press SPACE to clear rubble (" + activeObject.GetComponent<blockage>().blockagePrice + ")";
        	}
        	else if (type == "balloonStation" &&
        		managerScript.points >= managerScript.balloonPackCost) {
        		interactTextObject.SetActive(true);
        		interactTextObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = 
        			"Press SPACE to purchase balloons! (" + managerScript.balloonPackCost + ")";
        	}
        	else if (type == "healthStation" &&
        		managerScript.points >= managerScript.snackCost &&
        		GetComponent<playerHealth>().currentHealth < GetComponent<playerHealth>().maxHealth) {
        		interactTextObject.SetActive(true);
        		interactTextObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = 
        			"Press SPACE to purchase snacks! (" + managerScript.snackCost + ")";
        	} else if (type == "door" && activeObject.GetComponent<DoorInteraction>().isInteractable) {
        		interactTextObject.SetActive(true);
        		interactTextObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = "Hold SPACE to open door!";
        	}
        	else {
        		interactTextObject.SetActive(false);
        	}
        }
        else {
        	interactTextObject.SetActive(false);
        }
    }

    void Hold() {
    	isInteracting = true;
    }

    void ReleaseHold() {
    	isInteracting = false;
    }

    void OnTriggerEnter(Collider other) {
    	if (other.gameObject.CompareTag("barrier")) {
    		type = "barrier";
    		activeObject = other.gameObject;
    	}
    	if (other.gameObject.CompareTag("refillStation")) {
    		type = "refillStation";
    		activeObject = other.gameObject;
			Outline[] outlines = other.gameObject.GetComponentsInChildren<Outline>();
			foreach (Outline o in outlines) {
				o.eraseRenderer = false;
			}
    	}
    	if (other.gameObject.CompareTag("blockage")) {
    		type = "blockage";
    		activeObject = other.gameObject;
    	}
    	if (other.gameObject.CompareTag("balloonStation")) {
    		type = "balloonStation";
    		activeObject = other.gameObject;
    	}
    	if (other.gameObject.CompareTag("healthStation")) {
    		type = "healthStation";
    		activeObject = other.gameObject;
    	}
		if (other.gameObject.CompareTag("door")) {
    		type = "door";
    		activeObject = other.gameObject;
    	}
    }

    void OnTriggerExit(Collider other) {
    	if (other.gameObject.CompareTag("barrier")) {
    		type = null;
    		activeObject = null;
    	}
    	if (other.gameObject.CompareTag("refillStation")) {
    		type = null;
    		activeObject = null;
			Outline[] outlines = other.gameObject.GetComponentsInChildren<Outline>();
			foreach (Outline o in outlines) {
				o.eraseRenderer = true;
			}
    	}
    	if (other.gameObject.CompareTag("blockage")) {
    		type = null;
    		activeObject = null;
    	}
    	if (other.gameObject.CompareTag("balloonStation")) {
    		type = null;
    		activeObject = null;
    	}
    	if (other.gameObject.CompareTag("healthStation")) {
    		type = null;
    		activeObject = null;
    	}
		if (other.gameObject.CompareTag("door")) {
    		type = null;
    		activeObject = null;
    	}
    }
}
