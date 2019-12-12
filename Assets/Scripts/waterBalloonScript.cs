using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterBalloonScript : MonoBehaviour
{
    [Header("Settings")]
    public GameObject balloonAOEObject;

    private Rigidbody rigidbody;
    private Vector3 savedVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Paused() {
        savedVelocity = rigidbody.velocity;
        rigidbody.velocity = Vector3.zero;
    }

    public void Unpaused() {
        rigidbody.velocity = savedVelocity;
    }

    void OnTriggerEnter(Collider other) {
    	if (!other.gameObject.CompareTag("player") 
            && !other.gameObject.CompareTag("balloonAOE") 
            && !other.gameObject.CompareTag("barrier") 
            && other.gameObject.layer != 8 ) { //Ignore environment objects
    		Instantiate(balloonAOEObject,
                transform.position,
                Quaternion.identity
            );
            Destroy(gameObject);
    	}
    }
}
