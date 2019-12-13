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

    private float delay;
    private bool readyForInput;
    private string type;
    private GameObject interactTextObject;
    private playerAttackController attackScript;
    private gameManager managerScript;
    private bool isInteracting = false;
	private CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        interactTextObject = GameObject.FindGameObjectWithTag("interactTextObject");
        interactTextObject.SetActive(false);
        attackScript = GetComponent<playerAttackController>();
        managerScript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
		cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteracting && readyForInput && cc.velocity.magnitude < movementThreshold) {
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
        		activeObject.GetComponent<vendingScript>().Dispense();
				managerScript.PurchaseSnack();
        	}
			else if (type == "door" && activeObject.GetComponent<DoorInteraction>().isInteractable) {
        		activeObject.GetComponent<DoorInteraction>().Interact();
        	}

        	delay = repeatedInputDelayDuration;
        	readyForInput = false;
        }

        if (delay > 0) {
        	delay-=Time.deltaTime;
        }
        if (delay <= 0) {
        	readyForInput = true;
        }

        if (activeObject != null) {
        	if (type == "barrier" && activeObject.GetComponent<barrierScript>().isInteractable) {
        		interactTextObject.SetActive(true);
        		interactTextObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = "TAP AND HOLD TO REPAIR!";
        	}
        	else if (type == "refillStation" && attackScript.emptyBalloonCount > 0) {
        		interactTextObject.SetActive(true);
        		interactTextObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = "TAP AND HOLD TO FILL BALLOONS!";
        	}
        	else if (type == "blockage" && activeObject.GetComponent<blockage>().isInteractable) {
        		interactTextObject.SetActive(true);
        		interactTextObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = 
        			"TAP TO OPEN DOOR (" + activeObject.GetComponent<blockage>().blockagePrice + ")";
        	}
        	else if (type == "balloonStation" &&
        		managerScript.points >= managerScript.balloonPackCost) {
        		interactTextObject.SetActive(true);
        		interactTextObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = 
        			"TAP TO PURCHASE BALLOONS! (" + managerScript.balloonPackCost + ")";
        	}
        	else if (type == "healthStation" &&
        		managerScript.points >= managerScript.snackCost &&
        		GetComponent<playerHealth>().currentHealth < GetComponent<playerHealth>().maxHealth) {
        		interactTextObject.SetActive(true);
        		interactTextObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = 
        			"TAP TO PURCHASE SNACKS! (" + managerScript.snackCost + ")";
        	} else if (type == "door" && activeObject.GetComponent<DoorInteraction>().isInteractable) {
        		interactTextObject.SetActive(true);
        		interactTextObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = "TAP AND HOLD TO OPEN DOOR!";
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
			Outline[] outlines = other.gameObject.GetComponentsInChildren<Outline>();
			foreach (Outline o in outlines) {
				o.enabled = true;
			}
    	}
    	if (other.gameObject.CompareTag("refillStation")) {
    		type = "refillStation";
    		activeObject = other.gameObject;
			Outline[] outlines = other.gameObject.GetComponentsInChildren<Outline>();
			foreach (Outline o in outlines) {
				o.enabled = true;
			}
    	}
    	if (other.gameObject.CompareTag("blockage")) {
    		type = "blockage";
    		activeObject = other.gameObject;
			blockage b = other.gameObject.GetComponent<blockage>();
			foreach (GameObject d in b.doors) {
				if (d.GetComponent<DoorInteraction>().isInteractable)
					d.GetComponent<Outline>().enabled = true;
			}
    	}
    	if (other.gameObject.CompareTag("balloonStation")) {
    		type = "balloonStation";
    		activeObject = other.gameObject;
			Outline[] outlines = other.gameObject.GetComponentsInChildren<Outline>();
			foreach (Outline o in outlines) {
				o.enabled = true;
			}
    	}
    	if (other.gameObject.CompareTag("healthStation")) {
    		type = "healthStation";
    		activeObject = other.gameObject;
			Outline[] outlines = other.gameObject.GetComponentsInChildren<Outline>();
			foreach (Outline o in outlines) {
				o.enabled = true;
			}
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
			Outline[] outlines = other.gameObject.GetComponentsInChildren<Outline>();
			foreach (Outline o in outlines) {
				o.enabled = false;
			}
    	}
    	if (other.gameObject.CompareTag("refillStation")) {
    		type = null;
    		activeObject = null;
			Outline[] outlines = other.gameObject.GetComponentsInChildren<Outline>();
			foreach (Outline o in outlines) {
				o.enabled = false;
			}
    	}
    	if (other.gameObject.CompareTag("blockage")) {
    		type = null;
    		activeObject = null;
			blockage b = other.gameObject.GetComponent<blockage>();
			foreach (GameObject g in b.doors) {
				g.GetComponent<Outline>().enabled = false;
			}
    	}
    	if (other.gameObject.CompareTag("balloonStation")) {
    		type = null;
    		activeObject = null;
			Outline[] outlines = other.gameObject.GetComponentsInChildren<Outline>();
			foreach (Outline o in outlines) {
				o.enabled = false;
			}
    	}
    	if (other.gameObject.CompareTag("healthStation")) {
    		type = null;
    		activeObject = null;
			Outline[] outlines = other.gameObject.GetComponentsInChildren<Outline>();
			foreach (Outline o in outlines) {
				o.enabled = false;
			}
    	}
		if (other.gameObject.CompareTag("door")) {
    		type = null;
    		activeObject = null;
    	}
    }
}
