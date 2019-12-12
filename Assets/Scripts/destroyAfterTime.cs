using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyAfterTime : MonoBehaviour
{
    [Header("Settings")]
    public float delay;

    private gameManager managerScript; 

    // Start is called before the first frame update
    void Start()
    {
        managerScript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (delay <= 0) {
            Destroy(gameObject);
        }
        delay-=Time.deltaTime;
    }
}
