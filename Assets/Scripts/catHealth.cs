using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catHealth : MonoBehaviour
{
    [Header("Settings")]
    public float maxHealth;
    public float playerDamage;
	public float currentHealth;
	public float runAwayTime;
	public int odds;
	public GameObject emptyBalloonPickupObject;

	private gameManager managerScript;

    // Start is called before the first frame update
    void Start()
    {
        managerScript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();

        maxHealth = managerScript.catMaxHealth;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeDamage() {
    	if (currentHealth > 0) {
	    	currentHealth -= playerDamage;

	    	if (currentHealth <= 0) {
	    		managerScript.AddPoints(managerScript.pointsOnDeath);
				Renderer r = gameObject.GetComponentInChildren<Renderer>();
        		r.material.SetFloat("_Metallic", .5f);
				r.material.SetFloat("_Glossiness", .75f);
	    		Destroy(gameObject, runAwayTime);

	    		if (Random.Range(0,100) > odds &&
	    			GetComponent<catNavigation>().target == GameObject.FindGameObjectWithTag("player")) {
	    			Instantiate(emptyBalloonPickupObject,
	    				transform.position,
	    				Quaternion.identity
	    			);
	    		}
	    	}
	    	else {
	    		managerScript.AddPoints(managerScript.pointsOnHit);
	    	}
	    }
    }

    void OnTriggerEnter(Collider other) {
    	if (other.gameObject.CompareTag("balloonAOE")) {
    		TakeDamage();
    	}
    }
}
