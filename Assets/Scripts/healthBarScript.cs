using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBarScript : MonoBehaviour
{
    private playerHealth healthScript;

    // Start is called before the first frame update
    void Start()
    {
        healthScript = GameObject.FindGameObjectWithTag("player").GetComponent<playerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
		transform.localScale = new Vector3((healthScript.currentHealth / healthScript.maxHealth) * 3, 0.2f, 1);
    }
}
