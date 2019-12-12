using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerAttackController : MonoBehaviour
{
    [Header("Settings")]
    public GameObject[] balloonObjects;
    public float throwingSpeed;
    public int coolDownDuration;
    public int effectDelayDuration;

    [Header("Runtime Variables")]
    public int emptyBalloonCount;
    public bool isAttacking = false;

    private GameObject viewObject;
    private GameObject balloonSpawn;
    private int coolDown;
    private playerHealth healthScript;
    private int fullBalloonCount;
    private Text fullText;
    private Text emptyText;
    private gameManager managerScript;
    private GameObject fullAddEffect;
    private GameObject fullSubtractEffect;
    private GameObject emptyAddEffect;
    private GameObject emptySubtractEffect;
    private int fullAddEffectDelay = 0;
    private int fullSubtractEffectDelay = 0;
    private int emptyAddEffectDelay = 0;
    private int emptySubtractEffectDelay = 0;

    // Start is called before the first frame update
    void Start()
    {
        // find GameObjects and Components
        balloonSpawn = GameObject.FindGameObjectWithTag("balloonSpawn");
        viewObject = GameObject.FindGameObjectWithTag("cameraObject");
        healthScript = GetComponent<playerHealth>();
        managerScript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();

        fullText = GameObject.FindGameObjectWithTag("fullText").GetComponent<UnityEngine.UI.Text>();
        emptyText = GameObject.FindGameObjectWithTag("emptyText").GetComponent<UnityEngine.UI.Text>();

        fullAddEffect = GameObject.FindGameObjectWithTag("fullAddEffect");
        fullSubtractEffect = GameObject.FindGameObjectWithTag("fullSubtractEffect");
        emptyAddEffect = GameObject.FindGameObjectWithTag("emptyAddEffect");
        emptySubtractEffect = GameObject.FindGameObjectWithTag("emptySubtractEffect");

        // set variables
        fullBalloonCount = managerScript.playerFullBalloonStartCount;
        emptyBalloonCount = managerScript.playerEmptyBalloonStartCount;

        fullAddEffect.SetActive(false);
        fullSubtractEffect.SetActive(false);
        emptyAddEffect.SetActive(false);
        emptySubtractEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDown > 0) {
        	coolDown--;
        }
        if (coolDown == 0) {
        	isAttacking = false;
        }

        if (fullAddEffectDelay > 0) {
        	fullAddEffectDelay--;
        }
        if (fullAddEffectDelay == 0) {
        	fullAddEffect.SetActive(false);
        }

        if (fullSubtractEffectDelay > 0) {
        	fullSubtractEffectDelay--;
        }
        if (fullSubtractEffectDelay == 0) {
        	fullSubtractEffect.SetActive(false);
        }

        if (emptyAddEffectDelay > 0) {
        	emptyAddEffectDelay--;
        }
        if (emptyAddEffectDelay == 0) {
        	emptyAddEffect.SetActive(false);
        }

        if (emptySubtractEffectDelay > 0) {
        	emptySubtractEffectDelay--;
        }
        if (emptySubtractEffectDelay == 0) {
        	emptySubtractEffect.SetActive(false);
        }

        fullText.text = fullBalloonCount.ToString();
        emptyText.text = emptyBalloonCount.ToString();
    }

    void Tap() {
    	if (!isAttacking && 
        	healthScript.isAlive &&
        	fullBalloonCount > 0) {
        	Fire();
        }
    }

    void Fire() {
    	GameObject tmp = Instantiate(balloonObjects[Random.Range(0, balloonObjects.Length)],
    		transform.position,
    		Quaternion.identity
    	);

        managerScript.waterBalloonsThrown++;

    	tmp.GetComponent<Rigidbody>().AddForce(balloonSpawn.transform.forward * throwingSpeed);

    	fullBalloonCount--;

    	if (fullSubtractEffectDelay > 0) {
    		fullSubtractEffect.SetActive(false);
    	}
    	fullSubtractEffect.SetActive(true);
    	fullSubtractEffect.GetComponent<Animator>().Play("fullSubtractEffect");
    	fullSubtractEffect.GetComponent<UnityEngine.UI.Text>().text = "-1";
    	isAttacking = true;

    	coolDown = coolDownDuration;
    	fullSubtractEffectDelay = effectDelayDuration;
    }

    public void FillBalloon() {
    	emptyBalloonCount--;

    	fullBalloonCount++;

    	if (fullAddEffectDelay > 0) {
    		fullAddEffect.SetActive(false);
    	}
    	fullAddEffect.SetActive(true);
    	fullAddEffect.GetComponent<Animator>().Play("fullAddEffect");
    	fullAddEffect.GetComponent<UnityEngine.UI.Text>().text = "+1";

    	fullAddEffectDelay = effectDelayDuration;

    	if (emptySubtractEffectDelay > 0) {
    		emptySubtractEffect.SetActive(false);
    	}
    	emptySubtractEffect.SetActive(true);
    	emptySubtractEffect.GetComponent<Animator>().Play("emptySubtractEffect");
    	emptySubtractEffect.GetComponent<UnityEngine.UI.Text>().text = "-1";

    	emptySubtractEffectDelay = effectDelayDuration;
    }

    public void AddEmptyBalloons(int amount) {
    	emptyBalloonCount += amount;

    	if (emptyAddEffectDelay > 0) {
    		emptyAddEffect.SetActive(false);
    	}
    	emptyAddEffect.SetActive(true);
    	emptyAddEffect.GetComponent<Animator>().Play("emptyAddEffect");
    	emptyAddEffect.GetComponent<UnityEngine.UI.Text>().text = "+" + amount;

    	emptyAddEffectDelay = effectDelayDuration;
    }

    void OnTriggerEnter(Collider other) {
    	if (other.gameObject.CompareTag("emptyBalloonPickup")) {
    		AddEmptyBalloons(managerScript.pickUpBalloonAmount);
    		Destroy(other.gameObject);
    	}
        if (other.gameObject.CompareTag("healthPickup")) {
    		healthScript.AddHealth(managerScript.snackAmount);
    		Destroy(other.gameObject);
    	}
    }
}
